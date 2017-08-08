using System.Collections.Generic;

namespace Wrapper.Constant
{
    public class Dictionary
    {
        // Common
        public static Dictionary<int, string> ImageCostDic = new Dictionary<int, string>
        {
            {0, ""},
            {1, $"{PathManager.TexturesPath}cost_1.png"},
            {2, $"{PathManager.TexturesPath}cost_2.png"},
            {3, $"{PathManager.TexturesPath}cost_3.png"},
            {4, $"{PathManager.TexturesPath}cost_4.png"},
            {5, $"{PathManager.TexturesPath}cost_5.png"},
            {6, $"{PathManager.TexturesPath}cost_6.png"},
            {7, $"{PathManager.TexturesPath}cost_7.png"},
            {8, $"{PathManager.TexturesPath}cost_8.png"},
            {9, $"{PathManager.TexturesPath}cost_9.png"},
            {10, $"{PathManager.TexturesPath}cost_10.png"}
        };

        public static Dictionary<int, string> TypeCodeDic = new Dictionary<int, string>
        {
            {StringConst.NotApplicableCode, StringConst.NotApplicable},
            {StringConst.FollowerCode, StringConst.Follower},
            {StringConst.AmuletCode, StringConst.Amulet},
            {StringConst.AmuletExCode, StringConst.AmuletEx},
            {StringConst.SpellCode, StringConst.Spell}
        };

        public static Dictionary<int, string> CampCodeDic = new Dictionary<int, string>
        {
            {StringConst.NotApplicableCode, StringConst.NotApplicable},
            {StringConst.NeutralCode, StringConst.Neutral},
            {StringConst.ForestCode, StringConst.Forest},
            {StringConst.SwordCode, StringConst.Sword},
            {StringConst.RuneCode, StringConst.Rune},
            {StringConst.DragonCode, StringConst.Dragon},
            {StringConst.ShadowCode, StringConst.Shadow},
            {StringConst.BloodCode, StringConst.Blood},
            {StringConst.HavenCode, StringConst.Haven}
        };

        public static Dictionary<int, string> RarityCodeDic = new Dictionary<int, string>
        {
            {StringConst.NotApplicableCode, StringConst.NotApplicable},
            {StringConst.BronzeCode, StringConst.Bronze},
            {StringConst.SilverCode, StringConst.Silver},
            {StringConst.GoldCode, StringConst.Gold},
            {StringConst.LegendaryCode, StringConst.Legendary}
        };

        public static Dictionary<int, string> PackCodeDic = new Dictionary<int, string>
        {
            {StringConst.NotApplicableCode, StringConst.NotApplicable},
            {StringConst.PrimaireCode, StringConst.Primaire},
            {StringConst.StandardCode, StringConst.Standard},
            {StringConst.DarknessCode, StringConst.Darkness},
            {StringConst.BahamutCode, StringConst.Bahamut},
            {StringConst.TempestCode, StringConst.Tempest},
            {StringConst.DreamsCode, StringConst.Dreams}
        };
    }
}