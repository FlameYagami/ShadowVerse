using System.Collections.Generic;
using System.Data;
using System.Linq;
using CardEditor.Constant;
using ShadowVerse.Enums;
using ShadowVerse.Model;
using ShadowVerse.ViewModel;
using Wrapper;
using Wrapper.Constant;

namespace ShadowVerse.Utils
{
    public class CardUtils : SqliteConst
    {
        /// <summary>
        ///     获取排序的枚举类型
        /// </summary>
        /// <param name="order">排序方式</param>
        /// <returns></returns>
        public static Enum.PreviewOrderType GetPreOrderType(string order)
        {
            return order.Equals(StringConst.OrderNumber)
                ? Enum.PreviewOrderType.Number
                : Enum.PreviewOrderType.Value;
        }

        public static CardModel GetCardModel(int id)
        {
            var row = DeckEditorViewModel.DsAllCache.Tables[TableName].Rows
                .Cast<DataRow>()
                .AsParallel()
                .First(column => int.Parse(column[ColumnId].ToString()) == id);
            var isMonster = IsMonster(int.Parse(row[ColumnId].ToString()));
            return new CardModel
            {
                Id = int.Parse(row[ColumnId].ToString()),
                Type = row[ColumnType].ToString(), // E
                Camp = row[ColumnCamp].ToString(), // E
                Rarity = row[ColumnRarity].ToString(), // E
                Pack = row[ColumnPack].ToString(), // E
                Cost = row[ColumnCost].ToString(),
                Atk =
                    isMonster
                        ? row[ColumnAtk].ToString()
                        : row[ColumnAtk].ToString().Equals("0") ? StringConst.Hyphen : row[ColumnAtk].ToString(),
                Life = row[ColumnLife].ToString().Equals("0") ? StringConst.Hyphen : row[ColumnLife].ToString(),
                EvoAtk =
                    isMonster
                        ? row[ColumnEvoAtk].ToString()
                        : row[ColumnEvoAtk].ToString().Equals("0") ? StringConst.Hyphen : row[ColumnEvoAtk].ToString(),
                EvoLife = row[ColumnEvoLife].ToString().Equals("0") ? StringConst.Hyphen : row[ColumnEvoLife].ToString(),
                Cv = row[ColumnCv].ToString(),
                Name = row[ColumnName].ToString(),
                Skill = row[ColumnSkill].ToString()
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

        /// <summary>
        ///     获取卡编相关的小图路径集合
        /// </summary>
        /// <param name="id">卡编</param>
        /// <returns></returns>
        public static List<string> GetThumbnailPathList(int id)
        {
            return GetPathList(id, PathManager.ThumbnailPath);
        }

        private static List<string> GetPathList(int id, string parentPath)
        {
            var unMonsterPathList = new List<string> {$"{PathManager.ThumbnailPath}/{id}0{StringConst.ImageExtension}"};
            var allThumbnailPathList = new List<string>
            {
                $"{parentPath}/{id}0{StringConst.ImageExtension}",
                $"{parentPath}/{id}1{StringConst.ImageExtension}"
            };
            return IsMonster(id) ? allThumbnailPathList : unMonsterPathList;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsMonster(int id)
        {
            return int.Parse(id.ToString().Substring(5, 1)) == (int) Type.Monster;
        }

        /// <summary>
        ///     获取声优集合
        /// </summary>
        /// <returns></returns>
        public static List<object> GetCvList()
        {
            var packlist = new List<object> {StringConst.NotApplicable};
            var tempList = DeckEditorViewModel.DsAllCache.Tables[TableName].AsEnumerable().AsParallel()
                .Select(column => column[ColumnCv])
                .Distinct()
                .OrderBy(value => value.ToString().Length)
                .ToList();
            packlist.AddRange(tempList);
            return packlist;
        }

        /// <summary>
        ///     获取阵营的图片地址
        /// </summary>
        /// <param name="camp">阵营类型</param>
        /// <returns></returns>
        public static List<string> GetCampPathList(string camp)
        {
            var campUriList = new List<string>();
            var campArray = camp.Split('/');
            foreach (var tempcamp in campArray)
                foreach (var campItem in Dictionary.ImgCampPathDic)
                    if (campItem.Key.Equals(tempcamp))
                    {
                        campUriList.Add(campItem.Value);
                        break;
                    }
            while (campUriList.Count < 5)
                campUriList.Add(string.Empty);
            return campUriList;
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
            var row = DeckEditorViewModel.DsAllCache.Tables[TableName].Rows.Cast<DataRow>().AsParallel()
                .First(tempRow => int.Parse(tempRow[ColumnId].ToString()).Equals(id));
            return row[ColumnName].ToString();
        }

        public static string GetPackName(int pack)
        {
            return Dictionary.PackDic.First(item => item.Key.Equals(pack)).Value;
        }
    }
}