using System.Collections.ObjectModel;
using System.Linq;
using ShadowVerse.Model;
using ShadowVerse.Utils;

namespace ShadowVerse.ViewModel
{
    public class DeckViewModel : BaseModel
    {
        public DeckViewModel()
        {
            DeckList = new ObservableCollection<DeckModel>();
        }

        public ObservableCollection<DeckModel> DeckList { get; set; }

        public void AddCard(int id)
        {
            if (!CheckCard(id)) return;
            var cardModel = CardUtils.GetCardModel(id);
            var deckModel = new DeckModel
            {
                Id = id,
                Type = cardModel.Type,
                Cost = cardModel.Cost,
                Name = cardModel.Name,
                ImagePath = CardUtils.GetThumbnailPath(id)
            };
            DeckList.Add(deckModel);
        }

        public void DeleteCard(int id)
        {
            DeckList.Remove(DeckList.First(model => model.Id.Equals(id)));
            OnPropertyChanged(nameof(DeckList));
        }

        private bool CheckCard(int id)
        {
            var name = CardUtils.GetName(id);
            var canAdd = (DeckList.AsParallel().Count(deckModel => name.Equals(deckModel.Name)) < 3) &&
                         (DeckList.Count < 40);
            return canAdd;
        }
    }
}