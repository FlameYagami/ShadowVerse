using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BahamutCardCrawler.Constant;
using BahamutCardCrawler.Model;
using BahamutCardCrawler.Utils;
using BahamutCardCrawler.View;
using Common;
using Dialog;
using Dialog.View;
using HtmlAgilityPack;
using MaterialDesignThemes.Wpf;
using Wrapper;

namespace BahamutCardCrawler.ViewModel
{
    public class CardPreviewVm : BaseModel
    {
        private IDisposable _dispose;
        public PrgModel PrgModel { get; set; }

        public CardPreviewVm()
        {
            CardModels = new ObservableCollection<CardModel>();
            CmdRare = new DelegateCommand {ExecuteCommand = Rarity_Click};
            CmdDownloadImages = new DelegateCommand {ExecuteCommand = DownloadImages_Click };
            PrgModel = new PrgModel();
        }

        public DelegateCommand CmdRare { get; set; }
        public DelegateCommand CmdDownloadImages { get; set; }
        public ObservableCollection<CardModel> CardModels { get; set; }

        /// <summary>
        ///     罕贵度点击事件
        /// </summary>
        /// <param name="obj"></param>
        public void Rarity_Click(object obj)
        {
            var key = int.Parse(obj.ToString());
            var url = Dic.HrefDic[key];
            GetData(key, url);
        }

        private async void GetData(int cgKey, string url, bool isUpdate = false)
        {
            await DialogHost.Show(new DialogProgress("数据读取中..."), (object s, DialogOpenedEventArgs e) =>
            {
                Task.Run(() =>
                {
                    // 从数据库中取出图鉴列表
                    var cardModels = CardUtils.GetCardModels(cgKey);
                    // 非手动刷新,直接调用本地缓存
                    if ((0 != cardModels.Count) && !isUpdate)
                        return cardModels;
                    // 首次默认爬取网页数据
                    var webModels = GetCardPreviewModels(url, cgKey);
                    SyncIconData(cardModels.Select(model => model.Md5).ToList(), webModels);
                    return webModels;
                }).ToObservable().ObserveOnDispatcher().Subscribe(result =>
                {
                    CardModels.Clear();
                    result.ForEach(CardModels.Add);
                    DownloadIcon(result);
                    e.Session.Close();
                });
            });
        }

        public async void ShowImages(string md5, bool isUpdate)
        {
            var cardModel = CardUtils.GetCardModel(md5);
            if (string.IsNullOrWhiteSpace(cardModel.ImagesUrl) || isUpdate)
            {
                await DialogHost.Show(new DialogProgress("数据读取中..."), (object s, DialogOpenedEventArgs e) =>
                {
                    Task.Run(() =>
                        {
                            var imagesUrl = GetCardDetailUrls(cardModel.HrefUrl);
                            SyncImagesData(imagesUrl, cardModel);
                        })
                        .ToObservable().ObserveOnDispatcher().Subscribe(result =>
                        {
                            e.Session.UpdateContent(new CardDetailDialog(md5));
                            DownloadImages(cardModel);
                        });
                });
            }
            else
            {
                var vm = new CardDetailDialog(md5);
                await DialogHost.Show(vm);
            }
        }

        // 同步图标数据至数据库
        private static void SyncIconData(ICollection<string> dbMd5, IEnumerable<CardModel> cardModels)
        {
            var sqls = cardModels.Select(cardModel =>
                dbMd5.Contains(cardModel.Md5)
                    ? SqlExUtils.GetUpdateIconSql(cardModel)
                    : SqlExUtils.GetAddIconSql(cardModel)).Distinct().ToList();
            var result = DataManager.Execute(sqls);
            LogUtils.Write($"SyncIconData:{result}");
            DataManager.ReFillDataToDataSet(DataManager.DsAllCache, SqlExUtils.GetQueryAllSql(),
                DataManager.BahamutDbName);
            CardUtils.InitCardModels(true);
        }

        // 同步图鉴数据至数据库
        private static void SyncImagesData(ICollection<string> imagesUrl, CardModel cardModel)
        {
            cardModel.ImagesUrl = JsonUtils.Serializer(imagesUrl);
            cardModel.ImagesStats = 0;
            var updateSql = SqlExUtils.GetUpateImagesSql(cardModel);
            var reuslt = DataManager.Execute(updateSql);
            LogUtils.Write($"UpdateImages:{reuslt}");
            DataManager.ReFillDataToDataSet(DataManager.DsAllCache, SqlExUtils.GetQueryAllSql(),
                DataManager.BahamutDbName);
            CardUtils.InitCardModels(true);
        }

        // 下载图标
        private void DownloadIcon(IEnumerable<CardModel> cardModels)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var downloadModels = cardModels
                    .Where(model => !File.Exists(CardUtils.GetIconPath(model)))
                    .ToList();
                foreach (var downloadModel in downloadModels)
                    DownloadImage(downloadModel.IconUrl, CardUtils.GetIconPath(downloadModel));
            });
        }

        // 下载图鉴
        private void DownloadImages(CardModel cardModel)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var imagesUrlDic =
                    CardUtils.GetImagesDic(cardModel)
                        .Where(pair => !File.Exists(pair.Value))
                        .ToList();
                foreach (var imagesUrlPair in imagesUrlDic)
                    DownloadImage(imagesUrlPair.Key, imagesUrlPair.Value);
            });
        }

        public void DownloadImage(string url, string path)
        {
            try
            {
                var cln = new WebClient();
                cln.DownloadFile(url, path);
                LogUtils.Write($"DownloadImage Succeed:{url}");
            }
            catch (Exception e)
            {
                LogUtils.Write($"DownloadImage Failed:{e.Message}");
            }
        }

        public void DownloadImages_Click(object obj)
        {
            if (0 == CardModels.Count)
            {
                BaseDialogUtils.ShowDialogAuto("请选择图鉴目录");
                return;
            }
            GetCardDetialUrls(CardModels.ToList());
        }

        /// <summary>
        ///     获取所有卡牌模型下的卡图
        /// </summary>
        /// <param name="cardModels"></param>
        /// <returns></returns>
        public void GetCardDetialUrls(List<CardModel> cardModels)
        {
            PrgModel.Start();
            var imagesUrl = new List<string>();
            var taskDic = new Dictionary<int, bool>();

            Parallel.For(0, cardModels.Count, i =>
            {
                var page = Convert.ToInt32(i);
                taskDic.Add(page, false);
                GetCardDetailUrls(cardModels[page]).ObserveOnDispatcher().Subscribe(urls =>
                {
                    imagesUrl.AddRange(CardUtils.GetImagesDic(cardModels[page], urls)
                            .Where(pair => !File.Exists(pair.Value))
                            .Select(pair => pair.Value)
                            .ToList());
                    taskDic[page] = true;
                    var persent = taskDic.Values.Count(x => x)/float.Parse(cardModels.Count.ToString());
                    PrgModel.Update(Convert.ToInt32(persent*100),
                        $"解析中：{taskDic.Values.Count(x => x)} / {cardModels.Count}");
                });
            });
            _dispose = Observable.Interval(TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Subscribe(time =>
            {
                if (cardModels.Count != taskDic.Values.Count(flag => flag)) return;
                _dispose.Dispose();
            });
        }

        /// <summary>
        ///     获取一个链接下的图标模型集合
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cgKey"></param>
        /// <returns></returns>
        private static List<CardModel> GetCardPreviewModels(string url, int cgKey)
        {
            var race = CardUtils.GetRace(cgKey);
            var rarity = CardUtils.GetRarity(cgKey);
            var web = new HtmlWeb {OverrideEncoding = Encoding.GetEncoding("EUC-JP")};
            var frame = web.Load(url);
            var nodes =
                frame.DocumentNode.SelectNodes(@"//table")
                    .First(x => x.Attributes["id"].Value.Equals("content_block_1"));
            var childNodes =
                nodes.SelectNodes(@"//a")
                    .Where(x => (null != x.Attributes["href"]) && (3 == x.ChildNodes.Count))
                    .ToList();

            return (from childNode in childNodes
                    let hrefUrl = childNode.Attributes["href"].Value
                    let imageUrl = childNode.ChildNodes[0].Attributes["src"].Value
                    let name = childNode.ChildNodes[2].InnerText
                    select new CardModel
                    {
                        Md5 = Md5Utils.GetMd5(name),
                        Name = name,
                        IconUrl = imageUrl,
                        IconStats = 0,
                        HrefUrl = hrefUrl,
                        ImagesUrl = string.Empty,
                        ImagesStats = 0,
                        Race = race,
                        Rarity = rarity
                    })
                .ToList();
        }

        /// <summary>
        ///     获取一个链接下的图鉴集合
        /// </summary>
        /// <param name="cardModel"></param>
        /// <returns></returns>
        private IObservable<List<string>> GetCardDetailUrls(CardModel cardModel)
        {
            return Task.Run(() => GetCardDetailUrls(cardModel.HrefUrl)).ToObservable();
        }

        /// <summary>
        ///     获取一个连接下的卡图
        /// </summary>
        /// <param name="hrefUrl"></param>
        /// <returns></returns>
        private List<string> GetCardDetailUrls(string hrefUrl)
        {
            var web = new HtmlWeb();
            var frame = web.Load(hrefUrl);
            return frame.DocumentNode.SelectNodes(@"//img")
                .Where(x => (null != x.Attributes["width"]) && (null != x.Attributes["height"]))
                .Where(
                    x =>
                        int.Parse(x.Attributes["width"].Value).Equals(160) &&
                        int.Parse(x.Attributes["height"].Value).Equals(200))
                .Select(x => x.Attributes["src"].Value).ToList();
        }
    }
}