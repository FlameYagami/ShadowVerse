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
            DataContext = new DeckEditorViewModel(this);
            GvDeck.DataContext = new DeckViewModel();
            GridQuery.DataContext = new QueryModelView(GvCardPreview, CmbOrder);
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
        }

        private void CmbDeck_DropDownOpened(object sender, EventArgs e)
        {
        }

        private void LvDeckItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            Debug.Assert(grid != null);
            var id = int.Parse(grid.Tag.ToString());
            ((DeckViewModel) GvDeck.DataContext).DeleteCard(id);
        }

        private void LvDeckItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var deckModel = LvDeck.SelectedItem as DeckModel;
            if (deckModel == null) return;
            if (e.ClickCount == 2)
                ((DeckViewModel) GvDeck.DataContext).AddCard(deckModel.Id);
            else
                GvCardDetail.DataContext = new CardDetailViewModle(deckModel.Id);
        }

        private void CmbOrder_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void CardPreview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cardPreviewModel = LvPreview.SelectedItem as CardPreviewModel;
            if (cardPreviewModel != null)
                GvCardDetail.DataContext = new CardDetailViewModle(cardPreviewModel.Id);
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