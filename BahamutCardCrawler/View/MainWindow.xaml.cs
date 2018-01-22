using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BahamutCardCrawler.Constant;
using Wrapper;

namespace BahamutCardCrawler.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVm();
            InitCgPath();
        }

        private static void InitCgPath()
        {
            foreach (var race in Dic.RaceDic)
            {
                foreach (var rare in Dic.RareDic)
                {
                    var path = PathManager.Cg + race.Value + "\\" + rare.Value;
                    Dic.CgPathDic.Add(int.Parse(race.Key + rare.Key.ToString()), path);
                    if (Directory.Exists(path)) continue;
                    Directory.CreateDirectory(path);
                }
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
            var hrefUrl = image.Tag.ToString();
            ((MainVm)DataContext).ShowImages(hrefUrl);
        }
    }
}
