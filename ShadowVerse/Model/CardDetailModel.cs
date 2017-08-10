using System.Collections.Generic;
using System.Windows.Media;

namespace ShadowVerse.Model
{
    public class CardDetailModel : CardQueryModel
    {
        public string ImageAtkPath { get; set; }
        public string ImageLifePath { get; set; }
        public string ImageCostPath { get; set; }
        public LinearGradientBrush BgRarity { get; set; }
        public string Atk { get; set; }
        public string EvoAtk { get; set; }
        public string Life { get; set; }
        public string EvoLife { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> SkillList { get; set; }
        public List<string> ImagePathList { get; set; }
        public string ImageCurrentPath { get; set; }
        public List<string> EvoDescriptionList { get; set; }
        public List<string> FlavourList { get; set; }
    }
}