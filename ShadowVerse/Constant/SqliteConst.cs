namespace ShadowVerse.Constant
{
    public class SqliteConst
    {
        // Common
        public const string TableName = "card";
        public const string ColumnId = "id";
        public const string ColumnType = "type";
        public const string ColumnCamp = "camp";
        public const string ColumnRarity = "rarity";
        public const string ColumnPack = "pack";
        public const string ColumnName = "name";
        public const string ColumnCv = "cv";
        public const string ColumnCost = "cost";
        public const string ColumnAtk = "atk";
        public const string ColumnLife = "life";
        public const string ColumnEvoAtk = "evo_atk";
        public const string ColumnEvoLife = "evo_life";
        public const string ColumnSkill = "skill";
        public const string ColumnFlavour = "flavour";

        //DeckEditor
        public static string[] ColumKeyArray =
        {
            ColumnName, ColumnType, ColumnCamp, ColumnRarity, ColumnCost
        };
    }
}