using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            CmdDownloadImages = new DelegateCommand {ExecuteCommand = DownloadImages_Click};
            CmdSyncIcon = new DelegateCommand {ExecuteCommand = SyncIcon_Click };
            PrgModel = new PrgModel();
        }

        public void SyncIcon_Click(object obj)
        {

        }

        public DelegateCommand CmdRare { get; set; }
        public DelegateCommand CmdDownloadImages { get; set; }
        public DelegateCommand CmdSyncIcon { get; set; }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="isUpdate">是否需要从Web更新</param>
        public async void ShowImages(string md5, bool isUpdate)
        {
            var cardModel = CardUtils.GetCardModel(md5);
            await DialogHost.Show(new DialogProgress("数据读取中..."), (object s, DialogOpenedEventArgs e) =>
            {
                GetCardDetailUrls(cardModel, isUpdate).ObserveOnDispatcher().Subscribe(pair =>
                {
                    // 满足条件之一则更新数据库：1、数据源是否来自Web 2、是否需要从Web更新
                    if (pair.Value || isUpdate)
                    {
                        SyncImagesData(pair.Key, cardModel);
                        DataManager.ReFillDataToDataSet(DataManager.DsAllCache, SqlExUtils.GetQueryAllSql(), DataManager.BahamutDbName);
                        CardUtils.InitCardModels(true);
                    }
                    e.Session.UpdateContent(new CardDetailDialog(md5));
                    DownloadImages(cardModel);
                });
            });
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
        private static void SyncImagesData(ICollection<string> imagesUrl, CardModel cardModel, bool isUpdateDb = true)
        {
            cardModel.ImagesUrl = JsonUtils.Serializer(imagesUrl);
            cardModel.ImagesStats = 0;
            var updateSql = SqlExUtils.GetUpateImagesSql(cardModel);
            var reuslt = DataManager.Execute(updateSql);
            if (!reuslt) LogUtils.Write($"UpdateImages:{updateSql}");
            if (!isUpdateDb) return;
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
        private bool DownloadImages(CardModel cardModel)
        {
            var result = true;
            var imagesUrlDic =
                CardUtils.GetImagesDic(cardModel)
                    .Where(pair => !File.Exists(pair.Value))
                    .ToList();
            foreach (var imagesUrlPair in imagesUrlDic)
                result = DownloadImage(imagesUrlPair.Key, imagesUrlPair.Value);
            return result;
        }

        public bool DownloadImage(string url, string path)
        {
            try
            {
                var cln = new WebClient();
                cln.DownloadFile(url, path);
            }
            catch (Exception e)
            {
                LogUtils.Write($"DownloadImage Failed:{e.Message}");
                return false;
            }
            return true;
        }

        public void DownloadImages_Click(object obj)
        {
            if (0 == CardModels.Count)
            {
                BaseDialogUtils.ShowDialogAuto("请选择图鉴目录");
                return;
            }
            DownloadImages(CardModels.ToList(), false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardModels"></param>
        /// <param name="isUpdate">是否需要从Web更新</param>
        public void DownloadImages(List<CardModel> cardModels, bool isUpdate = false)
        {
            PrgModel.Start();
            var taskDic = new Dictionary<int, bool>();
            var sqls = new List<string>();
            for (var i = 0; i != cardModels.Count; i++)
            {
                var page = Convert.ToInt32(i);
                taskDic.Add(page, false);
                GetCardDetailUrls(cardModels[page], isUpdate).Select(pair =>
                {
                    // 数据模型填充
                    cardModels[page].ImagesUrl = JsonUtils.Serializer(pair.Key);
                    cardModels[page].ImagesStats = 0;
                    // 满足条件之一则更新数据库：1、数据源是否来自Web 2、是否需要从Web更新
                    if (pair.Value || isUpdate)
                    {
                        // 获取更新数据库的语句
                        var updateSql = SqlExUtils.GetUpateImagesSql(cardModels[page]);
                        sqls.Add(updateSql);
                    }
                    // 下载图鉴
                    DownloadImages(cardModels[page]);
                    return pair.Key;
                }).ObserveOnDispatcher().Subscribe(urls =>
                {
                    // UI更新状态      
                    taskDic[page] = true;
                    var persent = taskDic.Values.Count(x => x)/float.Parse(cardModels.Count.ToString());
                    PrgModel.Update(Convert.ToInt32(persent*100),
                        $"已完成：{taskDic.Values.Count(x => x)} / {cardModels.Count}");
                });
            }
            // 轮询任务字典，确保整个任务执行完毕，将数据提交至观测者
            _dispose = Observable.Interval(TimeSpan.FromSeconds(1)).ObserveOnDispatcher().Subscribe(time =>
            {
                if (cardModels.Count != taskDic.Values.Count(x => x))
                {
                    if (sqls.Count <= 20) return;
                    DataManager.Execute(sqls.GetRange(0, 20));
                    sqls.RemoveRange(0, 20);
                    return;
                }
                _dispose.Dispose();
                PrgModel.Finish();

                DataManager.Execute(sqls.GetRange(0, sqls.Count));
                sqls.RemoveRange(0, sqls.Count);
                DataManager.ReFillDataToDataSet(DataManager.DsAllCache, SqlExUtils.GetQueryAllSql(),
                    DataManager.BahamutDbName);
                CardUtils.InitCardModels(true);
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
        /// <param name="isUpdate">是否需要从Web更新</param>
        /// <returns></returns>
        private IObservable<KeyValuePair<List<string>, bool>> GetCardDetailUrls(CardModel cardModel, bool isUpdate = false)
        {
            return Task.Run(() =>
                {
                    var dbImagesUrl = CardUtils.GetCardImagesUrl(cardModel.Md5);
                    var webImagesUrl = GetCardDetailUrls(cardModel.HrefUrl);
                    if (0 == dbImagesUrl.Count || isUpdate)
                    {
                        return new KeyValuePair<List<string>, bool>(webImagesUrl, true);
                    }
                    return new KeyValuePair<List<string>, bool>(dbImagesUrl,false);
                }
            ).ToObservable();
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