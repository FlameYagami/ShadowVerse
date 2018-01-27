namespace ShadowVerse.Constant
{
    public class StringConst
    {
        // Common
        public const string DbOpenError = "打开数据库->错误";
        public const string DeleteHint = "删除->确认";
        public const string DeleteSucceed = "删除->成功";
        public const string CoverHint = "覆写->确认";
        public const string SaveSucceed = "保存->成功";
        public const string SaveFailed = "保存->失败";
        public const string OrderNumber = "编号";
        public const string NotApplicable = "(N/A)";
        public const int NotApplicableCode = -1;
        public const string Hyphen = "-";
        public const string QueryResult = "检索结果: ";
        public const string DeckExtension = ".sv";
        public const string ImageExtension = ".jpg";
        public const string FileNotExits = "文件缺失";

        // DeckEditor
        public const string DeckNameNone = "卡组名称->未输入";
        public const string DeckNameExist = "卡组名称->已存在";
        public const string ResaveSucceed = "另存->成功";

        // Pack
        public const string Primaire = "永久卡";
        public const string Standard = "標準卡";
        public const string Darkness = "暗影進化";
        public const string Bahamut = "巴哈姆特";
        public const string Tempest = "諸神狂嵐";
        public const string Dreams = "夢境奇想";
        public const int PrimaireCode = 100;
        public const int StandardCode = 101;
        public const int DarknessCode = 102;
        public const int BahamutCode = 103;
        public const int TempestCode = 104;
        public const int DreamsCode = 105;

        // Camp
        public const string Neutral = "中立";
        public const string Forest = "精灵";
        public const string Sword = "皇家护卫";
        public const string Rune = "女巫";
        public const string Dragon = "龙";
        public const string Shadow = "死灵法师";
        public const string Blood = "血族";
        public const string Haven = "主教";
        public const int NeutralCode = 0;
        public const int ForestCode = 1;
        public const int SwordCode = 2;
        public const int RuneCode = 3;
        public const int DragonCode = 4;
        public const int ShadowCode = 5;
        public const int BloodCode = 6;
        public const int HavenCode = 7;

        // Rarity
        public const string Bronze = "铜";
        public const string Silver = "银";
        public const string Gold = "金";
        public const string Legendary = "虹";
        public const int BronzeCode = 1;
        public const int SilverCode = 2;
        public const int GoldCode = 3;
        public const int LegendaryCode = 4;

        // Type
        public const string Follower = "随从";
        public const string Spell = "咒术";
        public const string Amulet = "护符";
        public const string AmuletEx = "护符";
        public const int FollowerCode = 1;
        public const int SpellCode = 4;
        public const int AmuletCode = 2;
        public const int AmuletExCode = 3;
    }
}