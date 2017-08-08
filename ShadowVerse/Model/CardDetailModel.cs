﻿using System.Collections.Generic;
using System.Windows.Media;

namespace ShadowVerse.Model
{
    public class CardDetailModel : CardQueryModel
    {
        public string ImageCostPath { get; set; }
        public SolidColorBrush ImageRarity { get; set; }
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
    }
}