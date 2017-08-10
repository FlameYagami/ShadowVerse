using System.Collections.Generic;

namespace Wrapper.Constant
{
    public class Dictionary
    {
        // Common
        public static Dictionary<int, string> ImageCostDic = new Dictionary<int, string>
        {
            {0, string.Empty},
            {1, PathManager.Cost1Path},
            {2, PathManager.Cost2Path},
            {3, PathManager.Cost3Path},
            {4, PathManager.Cost4Path},
            {5, PathManager.Cost5Path},
            {6, PathManager.Cost6Path},
            {7, PathManager.Cost7Path},
            {8, PathManager.Cost8Path},
            {9, PathManager.Cost9Path},
            {10, PathManager.Cost10Path}
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