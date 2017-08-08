namespace ShadowVerse.Model
{
    public class CardModel
    {
        public int Id { get; set; }
        public int TypeCode { get; set; }
        public int CampCode { get; set; }
        public int RarityCode { get; set; }
        public int PackCode { get; set; }
        public string Name { get; set; }
        public string Cv { get; set; }
        public int Cost { get; set; }
        public int Atk { get; set; }
        public int EvoAtk { get; set; }
        public int Life { get; set; }
        public int EvoLife { get; set; }
        public string SkillJson { get; set; }
    }
}