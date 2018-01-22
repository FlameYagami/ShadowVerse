using System.Collections.Generic;

namespace BahamutCardCrawler.ViewModel
{
    public class CardDetailVm
    {
        public List<string> Images { get; set; }

        public CardDetailVm(List<string> images)
        {
            Images = images;
        }
    }
}
