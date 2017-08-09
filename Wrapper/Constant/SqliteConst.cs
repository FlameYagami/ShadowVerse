namespace Wrapper.Constant
{
    public class SqliteConst
    {
        // Common
        public const string TableName = "Card";
        public const string ColumnId = "Id";
        public const string ColumnType = "Type";
        public const string ColumnCamp = "Camp";
        public const string ColumnRarity = "Rarity";
        public const string ColumnPack = "Pack";
        public const string ColumnName = "Name";
        public const string ColumnCv = "Cv";
        public const string ColumnCost = "Cost";
        public const string ColumnAtk = "Atk";
        public const string ColumnLife = "Life";
        public const string ColumnEvoAtk = "Evo_Atk";
        public const string ColumnEvoLife = "Evo_Life";
        public const string ColumnSkill = "Skill";
        public const string ColumnFlavour = "Flavour";

        //DeckEditor
        public static string[] ColumKeyArray =
        {
            ColumnName, ColumnType, ColumnCamp, ColumnRarity, ColumnCost
        };
    }
}