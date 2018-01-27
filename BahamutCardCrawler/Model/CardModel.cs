namespace BahamutCardCrawler.Model
{
    public class CardModel
    {
        public string Md5 { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public int IconStats { get; set; }
        public string HrefUrl { get; set; }
        public string ImagesUrl { get; set; }
        public int ImagesStats { get; set; }
        public int Race { get; set; }
        public int Rarity { get; set; }
    }
}