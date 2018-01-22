using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using BahamutCardCrawler.Model;
using Common;
using Dialog;
using HtmlAgilityPack;
using MaterialDesignThemes.Wpf;
using Dialog.View;
using DelegateCommand = ShadowVerse.Command.DelegateCommand;

namespace BahamutCardCrawler
{
    public class MainVm
    {
        public DelegateCommand CmdRare { get; set; }
        public DelegateCommand CmdExit { get; set; }
        public ObservableCollection<CardModel> CardModels { get; set; }

        public MainVm()
        {
            LogUtils.Show();
            CardModels = new ObservableCollection<CardModel>();
            CmdRare = new DelegateCommand {ExecuteCommand = Rare_Click };
            CmdExit = new DelegateCommand { ExecuteCommand = Exit_Click };
        }

        public void Exit_Click(object obj)
        {
            Environment.Exit(0);
        }

        public void Rare_Click(object obj)
        {
            var url = obj.ToString();
            GetData(url);
        }

        private async void GetData(string url)
        {
            await DialogHost.Show(new DialogProgress("获取信息中"), (s, e) =>
            {
                Task.Run(() =>
                {
                    var web = new HtmlWeb();
                    var frame = web.Load(url);
                    var nodes =
                        frame.DocumentNode.SelectNodes(@"//table").First(x => x.Attributes["id"].Value.Equals("content_block_1"));
                    var childNodes = nodes.SelectNodes(@"//a").Where(x => null != x.Attributes["href"] && 3 == x.ChildNodes.Count).ToList();

                    return (from childNode in childNodes
                            let hrefUrl = childNode.Attributes["href"].Value
                            let imageUrl = childNode.ChildNodes[0].Attributes["src"].Value
                            let name = childNode.ChildNodes[2].InnerText
                            select new CardModel() { HrefUrl = hrefUrl, ImageUrl = imageUrl, Name = name }).ToList();
                }).ToObservable().ObserveOnDispatcher().Subscribe(result =>
                {
                    e.Session.Close(false);
                    CardModels.Clear();
                    result.ForEach(CardModels.Add);
                });
            }, (s, e) => { });
        }

        public async void ShowImages(string hrefUrl)
        {
            await DialogHost.Show(new DialogProgress("获取信息中"), (s, e) =>
            {
                Task.Run(() =>
                {
                    var web = new HtmlWeb();
                    var frame = web.Load(hrefUrl);
                    return frame.DocumentNode.SelectNodes(@"//img")
                        .Where(x => null != x.Attributes["width"] && null != x.Attributes["height"])
                        .Where(x => int.Parse(x.Attributes["width"].Value).Equals(160) && int.Parse(x.Attributes["height"].Value).Equals(200))
                        .Select(x => x.Attributes["src"].Value).ToList();
                }).ToObservable().ObserveOnDispatcher().Subscribe(result =>
                {
                    e.Session.UpdateContent(new CardDetail(result));
                });
            }, (s, e) => { });
        }
    }
}
