using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShadowVerse.Model;
using ShadowVerse.ViewModel;

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
            DataContext = new AllViewModel(this);
            GvDeck.DataContext = new DeckViewModel();
            GridQuery.DataContext = new CardQueryModelView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            Debug.Assert(image != null);
            var id = int.Parse(image.Tag.ToString());
            ((DeckViewModel) GvDeck.DataContext).DeleteCard(id);
        }

        private void LvDeckItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            Debug.Assert(image != null);
            var id = int.Parse(image.Tag.ToString());
            if (e.ClickCount == 1)
            {
                GvCardDetail.DataContext = new CardDetailViewModle(id);
            }
            if (e.ClickCount == 2)
            {
                ((DeckViewModel)GvDeck.DataContext).AddCard(id);
            }
        }

        private void CardPreview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var previewModel = LvPreview.SelectedItem as CardPreviewModel;
            Debug.Assert(previewModel != null);
            GvCardDetail.DataContext = new CardDetailViewModle(previewModel.Id);
        }

        private void CardPreviewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            Debug.Assert(grid != null);
            var id = int.Parse(grid.Tag.ToString());
            ((DeckViewModel) GvDeck.DataContext).AddCard(id);
        }
    }
}