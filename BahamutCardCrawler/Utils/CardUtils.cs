using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BahamutCardCrawler.Constant;
using BahamutCardCrawler.Model;
using Common;
using Wrapper;

namespace BahamutCardCrawler.Utils
{
    public class CardUtils : SqliteConst
    {
        private static Dictionary<string, CardModel> _cardModelsDic = new Dictionary<string, CardModel>();

        public static void InitCardModels(bool isUpdate = false)
        {
            try
            {
                if ((0 != _cardModelsDic.Count) && !isUpdate) return;
                _cardModelsDic.Clear();
                _cardModelsDic = DataManager.DsAllCache.Tables[DataManager.BahamutDbName].Rows.Cast<DataRow>()
                    .AsParallel().AsQueryable()
                    .Select(column => new CardModel
                    {
                        Md5 = column[ColumnMd5].ToString(),
                        Name = column[ColumnName].ToString(),
                        IconUrl = column[ColumnIocnUrl].ToString(),
                        IconStats = int.Parse(column[ColumnIocnStats].ToString()),
                        HrefUrl = column[ColumnHrefUrl].ToString(),
                        ImagesUrl = column[ColumnImagesUrl].ToString(),
                        ImagesStats = int.Parse(column[ColumnImagesStats].ToString()),
                        Race = int.Parse(column[ColumnRace].ToString()),
                        Rarity = int.Parse(column[ColumnRarity].ToString())
                    }).ToDictionary(model => model.Md5, model => model);
            }
            catch (Exception ex)
            {
                LogUtils.Write($"InitCardModels Failed:{ex.Message}");
            }
        }

        public static void InitCgPath()
        {
            foreach (var race in Dic.RaceDic)
                foreach (var rarity in Dic.RarityDic)
                {
                    // 罕贵度目录
                    var imagePath = $"{PathManager.Cg + race.Value}\\{rarity.Value}\\";
                    // 图标目录
                    var iconPath = $"{imagePath}Icon";
                    // 创建罕贵度目录
                    if (!Directory.Exists(imagePath))
                        Directory.CreateDirectory(imagePath);
                    // 创建图标目录
                    if (!Directory.Exists(iconPath))
                        Directory.CreateDirectory(iconPath);
                    // 生成图片目录字典
                    var cgKey = race.Key*10 + rarity.Key;
                    Dic.IconPathDic.Add(cgKey, iconPath);
                    Dic.ImagesPathDic.Add(cgKey, imagePath);
                }
        }

        public static CardModel GetCardModel(string md5)
        {
            InitCardModels();
            return _cardModelsDic[md5];
//            var row = DataManager.DsAllCache.Tables[DataManager.BahamutDbName].Rows.Cast<DataRow>()
//                .AsParallel()
//                .FirstOrDefault(column => column[ColumnMd5].ToString().Equals(md5));
//            if (null == row) return null;
//            return new CardModel
//            {
//                Md5 = row[ColumnMd5].ToString(),
//                Name = row[ColumnName].ToString(),
//                IconUrl = row[ColumnIocnUrl].ToString(),
//                IconStats = int.Parse(row[ColumnIocnStats].ToString()),
//                HrefUrl = row[ColumnHrefUrl].ToString(),
//                ImagesUrl = row[ColumnImagesUrl].ToString(),
//                ImagesStats = int.Parse(row[ColumnImagesStats].ToString()),
//                Race = int.Parse(row[ColumnRace].ToString()),
//                Rarity = int.Parse(row[ColumnRarity].ToString())
//            };
        }

        public static List<CardModel> GetCardModels(int cgKey)
        {
            var race = cgKey/10;
            var rarity = cgKey%10;
            InitCardModels();
            return
                _cardModelsDic.Values.ToList().Where(model => (model.Race == race) && (model.Rarity == rarity)).ToList();
//            return DataManager.DsAllCache.Tables[DataManager.BahamutDbName].Rows.Cast<DataRow>()
//                .AsParallel()
//                .Where(
//                    column =>
//                        (int.Parse(column[ColumnRace].ToString()) == race) &&
//                        (int.Parse(column[ColumnRarity].ToString()) == rarity))
//                .Select(column => new CardModel
//                {
//                    Md5 = column[ColumnMd5].ToString(),
//                    Name = column[ColumnName].ToString(),
//                    IconUrl = column[ColumnIocnUrl].ToString(),
//                    IconStats = int.Parse(column[ColumnIocnStats].ToString()),
//                    HrefUrl = column[ColumnHrefUrl].ToString(),
//                    ImagesUrl = column[ColumnImagesUrl].ToString(),
//                    ImagesStats = int.Parse(column[ColumnImagesStats].ToString()),
//                    Race = int.Parse(column[ColumnRace].ToString()),
//                    Rarity = int.Parse(column[ColumnRarity].ToString())
//                })
//                .ToList();
        }

        public static List<string> GetCardImagesUrl(string md5)
        {
            InitCardModels();
            var imagesUrlJson = _cardModelsDic[md5].ImagesUrl;
            return string.IsNullOrWhiteSpace(imagesUrlJson) ? new List<string>() : JsonUtils.Deserialize<List<string>>(imagesUrlJson);
        }

        public static int GetRace(int cgKey)
        {
            return cgKey/10;
        }

        public static int GetRarity(int cgKey)
        {
            return cgKey%10;
        }

        public static string GetIconPath(int race, int rarity, string name)
        {
            return $"{PathManager.Cg}{Dic.RaceDic[race]}\\{Dic.RarityDic[rarity]}\\Icon\\{name}.jpg";
        }

        public static string GetIconPath(CardModel cardModel)
        {
            return
                $"{PathManager.Cg}{Dic.RaceDic[cardModel.Race]}\\{Dic.RarityDic[cardModel.Rarity]}\\Icon\\{cardModel.Name}.jpg";
        }

        /// <summary>
        ///     获取图鉴链接、路径的数据字典
        /// </summary>
        /// <param name="cardModel"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetImagesDic(CardModel cardModel)
        {
            var urlsDic = new Dictionary<string, string>();
            var imagesUrl = JsonUtils.Deserialize<List<string>>(cardModel.ImagesUrl);
            for (var i = 0; i != imagesUrl.Count; i++)
                urlsDic.Add(imagesUrl[i], GetImagePath(cardModel.Race, cardModel.Rarity, $"{cardModel.Name}_{i}"));
            return urlsDic;
        }

        /// <summary>
        ///     获取图鉴链接、路径的数据字典
        /// </summary>
        /// <param name="cardModel"></param>
        /// <param name="imagesUrl"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetImagesDic(CardModel cardModel, List<string> imagesUrl)
        {
            var urlsDic = new Dictionary<string, string>();
            for (var i = 0; i != imagesUrl.Count; i++)
                urlsDic.Add(imagesUrl[i], GetImagePath(cardModel.Race, cardModel.Rarity, $"{cardModel.Name}_{i}"));
            return urlsDic;
        }

        public static string GetImagePath(int race, int rarity, string nameEx)
        {
            return $"{PathManager.Cg + Dic.RaceDic[race]}\\{Dic.RarityDic[rarity]}\\" + nameEx + ".jpg";
        }
    }
}