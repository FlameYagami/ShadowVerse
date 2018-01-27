using System.Collections.Generic;
using System.Linq;
using BahamutCardCrawler.Model;
using BahamutCardCrawler.Utils;

namespace BahamutCardCrawler.ViewModel
{
    public class CardDetailVm
    {
        public CardDetailVm(string md5)
        {
            var cardModel = CardUtils.GetCardModel(md5);
            CardDetailModels =
                CardUtils.GetImagesDic(cardModel)
                    .Select(pair => new CardDetailModel {ImageUrl = pair.Key, ImagePath = pair.Value})
                    .ToList();
        }

        public List<CardDetailModel> CardDetailModels { get; set; }
    }
}