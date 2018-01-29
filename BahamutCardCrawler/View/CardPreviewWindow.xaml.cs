using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BahamutCardCrawler.Model;
using BahamutCardCrawler.Utils;
using BahamutCardCrawler.ViewModel;
using Common;
using Dialog;
using Wrapper;

namespace BahamutCardCrawler.View
{
    /// <summary>
    ///     CardPreviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CardPreviewWindow
    {
        public CardPreviewWindow()
        {
            InitializeComponent();
        }

        private void CardPreviewWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LogUtils.Show();
            DataManager.Init(DataManager.BahamutDbName, PathManager.RootPath + DataManager.BahamutDbName, "");
            if (DataManager.FillDataToDataSet(DataManager.DsAllCache, SqlExUtils.GetQueryAllSql(),
                DataManager.BahamutDbName))
            {
                AppbarView.DataContext = new AppbarVm(this);
                ContentView.DataContext = new CardPreviewVm();
                CardUtils.InitCgPath();
            }
            else
            {
                BaseDialogUtils.ShowDialogOk("数据库异常");
            }
        }

        private void Title_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (null == image) return;
            var md5 = image.Tag.ToString();
            ((CardPreviewVm)ContentView.DataContext).ShowImages(md5, false);
        }
    }
}