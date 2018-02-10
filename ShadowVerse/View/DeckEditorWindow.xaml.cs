using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dialog;
using ShadowVerse.Model;
using ShadowVerse.ViewModel;
using Wrapper;
using SqlUtils = ShadowVerse.Utils.SqlUtils;

namespace ShadowVerse.View
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeckEditorWindow : Window
    {
        public DeckEditorWindow()
        {
            InitializeComponent();
            AppbarView.DataContext = new AppbarVm(this);
            GvDeck.DataContext = new DeckViewModel();
            GridQuery.DataContext = new CardQueryModelView();
            GvCardDetail.DataContext = new CardDetailViewModle();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataManager.FillDataToDataSet(DataManager.DsAllCache, SqlUtils.GetQueryAllSql()))
            {
                if (!Directory.Exists(PathManager.DeckFolderPath))
                    Directory.CreateDirectory(PathManager.DeckFolderPath);
            }
            else
            {
                BaseDialogUtils.ShowDialogOk("数据库初始化失败");
            }
        }

        private void Title_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CmbDeck_DropDownClosed(object sender, EventArgs e)
        {
            ((DeckViewModel) GvDeck.DataContext).DeckLoad();
        }

        private void CmbDeck_DropDownOpened(object sender, EventArgs e)
        {
            ((DeckViewModel) GvDeck.DataContext).DeckNameLoad();
        }

        private void LvDeckItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (null == image) return;
            var id = int.Parse(image.Tag.ToString());
            ((DeckViewModel) GvDeck.DataContext).DeleteCard(id);
        }

        private void LvDeckItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (null == image) return;;
            var id = int.Parse(image.Tag.ToString());
            if (e.ClickCount == 1)
            {
                ((CardDetailViewModle)GvCardDetail.DataContext).UpdateCardDetailModel(id);
            }
            if (e.ClickCount == 2)
            {
                ((DeckViewModel)GvDeck.DataContext).AddCard(id);
            }
        }

        private void CardPreview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var previewModel = LvPreview.SelectedItem as CardPreviewModel;
            if (null == previewModel) return;
            ((CardDetailViewModle) GvCardDetail.DataContext).UpdateCardDetailModel(previewModel.Id);
        }

        private void CardPreviewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            if (null == grid) return;
            var id = int.Parse(grid.Tag.ToString());
            ((DeckViewModel) GvDeck.DataContext).AddCard(id);
        }
    }
}