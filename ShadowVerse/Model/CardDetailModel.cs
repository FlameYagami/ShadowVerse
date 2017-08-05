using System.Collections.Generic;

namespace ShadowVerse.Model
{
    public class CardDetailModel : CardModel
    {
        public List<string> AtkAndLifeList { get; set; }
        public List<string> SkillList { get; set; }
        public List<string> ImagePathList { get; set; }
        public List<string> EvoDescriptionList { get; set; }
    }
}