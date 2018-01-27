using System.Collections.Generic;
using System.Data;
using System.Linq;
using ShadowVerse.Constant;
using ShadowVerse.Model;
using ShadowVerse.ViewModel;
using Wrapper;
using Wrapper.Constant;

namespace ShadowVerse.Utils
{
    public class CardUtils : SqliteConst
    {
        public static CardModel GetCardModel(int id)
        {
            var row = AllViewModel.DsAllCache.Tables[TableName].Rows
                .Cast<DataRow>()
                .AsParallel()
                .First(column => int.Parse(column[ColumnId].ToString()) == id);
            return new CardModel
            {
                Id = id,
                TypeCode = int.Parse(row[ColumnType].ToString()),
                CampCode = int.Parse(row[ColumnCamp].ToString()),
                RarityCode = int.Parse(row[ColumnRarity].ToString()),
                PackCode = int.Parse(row[ColumnPack].ToString()),
                Cost = int.Parse(row[ColumnCost].ToString()),
                Atk = int.Parse(row[ColumnAtk].ToString()),
                Life = int.Parse(row[ColumnLife].ToString()),
                EvoAtk = int.Parse(row[ColumnEvoAtk].ToString()),
                EvoLife = int.Parse(row[ColumnEvoLife].ToString()),
                Cv = row[ColumnCv].ToString(),
                Name = row[ColumnName].ToString(),
                SkillJson = row[ColumnSkill].ToString(),
                FlavourJosn = row[ColumnFlavour].ToString()
            };
        }

        /// <summary>
        ///     获取卡编相关的大图路径集合
        /// </summary>
        /// <param name="id">卡编</param>
        /// <returns></returns>
        public static List<string> GetPicturePathList(int id)
        {
            return GetPathList(id, PathManager.PicturePath);
        }

        private static List<string> GetPathList(int id, string parentPath)
        {
            var unMonsterPathList = new List<string> {$"{parentPath}/{id}0{StringConst.ImageExtension}"};
            var allThumbnailPathList = new List<string>
            {
                $"{parentPath}/{id}0{StringConst.ImageExtension}",
                $"{parentPath}/{id}1{StringConst.ImageExtension}"
            };
            return IsFollower(id) ? allThumbnailPathList : unMonsterPathList;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsFollower(int id)
        {
            return int.Parse(id.ToString().Substring(5, 1)) == StringConst.FollowerCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsToekn(int id)
        {
            return id.ToString().StartsWith("9");
        }

        /// <summary>
        ///     获取声优集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCvList()
        {
            var packlist = new List<string> {StringConst.NotApplicable};
            var tempList = AllViewModel.DsAllCache.Tables[TableName].AsEnumerable().AsParallel()
                .Select(column => column[ColumnCv].ToString())
                .Distinct()
                .OrderBy(value => value.ToString().Length)
                .ToList();
            packlist.AddRange(tempList);
            return packlist;
        }

        /// <summary>
        ///     获取卡组路径
        /// </summary>
        /// <param name="deckName">卡组名称</param>
        /// <returns>卡组路径</returns>
        public static string GetDeckPath(string deckName)
        {
            return PathManager.DeckFolderPath + deckName + StringConst.DeckExtension;
        }

        /// <summary>
        ///     获取缩略图路径
        /// </summary>
        /// <param name="id">卡编</param>
        /// <returns>缩略图路径</returns>
        public static string GetThumbnailPath(int id)
        {
            var tumbnailPath = $"{PathManager.ThumbnailPath + id}0{StringConst.ImageExtension}";
            return tumbnailPath;
        }

        /// <summary>
        ///     获取卡名
        /// </summary>
        /// <param name="id"></param>
        /// <returns>卡名</returns>
        public static string GetName(int id)
        {
            var row = AllViewModel.DsAllCache.Tables[TableName].Rows.Cast<DataRow>().AsParallel()
                .First(tempRow => int.Parse(tempRow[ColumnId].ToString()).Equals(id));
            return row[ColumnName].ToString();
        }
    }
}