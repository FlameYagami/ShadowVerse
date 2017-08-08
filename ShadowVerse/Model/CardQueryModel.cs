using Wrapper.Constant;

namespace ShadowVerse.Model
{
    public class CardQueryModel
    {
        public CardQueryModel()
        {
            Type = StringConst.NotApplicable;
            Camp = StringConst.NotApplicable;
            Rarity = StringConst.NotApplicable;
            Pack = StringConst.NotApplicable;
            Cv = StringConst.NotApplicable;
            Cost = string.Empty;
        }

        public string Type { get; set; }
        public string Camp { get; set; }
        public string Rarity { get; set; }
        public string Pack { get; set; }
        public string Cv { get; set; }
        public string Cost { get; set; }
    }
}