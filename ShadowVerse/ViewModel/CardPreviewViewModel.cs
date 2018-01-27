using System.Collections.Generic;
using ShadowVerse.Constant;
using ShadowVerse.Model;
using Wrapper.Constant;

namespace ShadowVerse.ViewModel
{
    public class CardPreviewViewModel
    {
        public CardPreviewViewModel(List<CardPreviewModel> previewList)
        {
            CardPreviewList = previewList;
            CardPreviewCount = StringConst.QueryResult + CardPreviewList.Count;
        }

        public string CardPreviewCount { get; set; }
        public List<CardPreviewModel> CardPreviewList { get; set; }
    }
}