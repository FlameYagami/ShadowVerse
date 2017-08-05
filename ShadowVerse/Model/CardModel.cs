using Wrapper.Constant;

namespace ShadowVerse.Model
{
    public class CardModel
    {
        public CardModel()
        {
            Key = string.Empty;
            Cost = string.Empty;
            Type = StringConst.NotApplicable;
            Camp = StringConst.NotApplicable;
            Rarity = StringConst.NotApplicable;
            Pack = StringConst.NotApplicable;
            Cv = StringConst.NotApplicable;
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Camp { get; set; }
        public string Rarity { get; set; }
        public string Pack { get; set; }
        public string Name { get; set; }
        public string Cv { get; set; }
        public string Cost { get; set; }
        public string Atk { get; set; }
        public string EvoAtk { get; set; }
        public string Life { get; set; }
        public string EvoLife { get; set; }
        public string Skill { get; set; }
    }
}