using BahamutCardCrawler.ViewModel;

namespace BahamutCardCrawler.View
{
    /// <summary>
    ///     Images.xaml 的交互逻辑
    /// </summary>
    public partial class CardDetailDialog
    {
        public CardDetailDialog(string md5)
        {
            InitializeComponent();
            DataContext = new CardDetailVm(md5);
        }
    }
}