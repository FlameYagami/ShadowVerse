using System;
using BahamutCardCrawler.Constant;
using BahamutCardCrawler.Model;

namespace BahamutCardCrawler.Utils
{
    public class SqlExUtils : SqliteConst
    {
        public static string GetQueryAllSql()
        {
            return $"SELECT * FROM {TableName} ORDER BY {ColumnRace} ASC, {ColumnRarity} ASC";
        }

        public static string GetCardsSql(int race, int rarity)
        {
            return
                $"SELECT * FROM {TableName} WHERE {ColumnRace}='{race}' AND {ColumnRarity}='{rarity}' ORDER BY {ColumnRace} ASC, {ColumnRarity} ASC";
        }

        public static string GetAddIconSql(CardModel model)
        {
            var sql = $"INSERT INTO {TableName} " +
                      $"({ColumnMd5},{ColumnName},{ColumnIocnUrl},{ColumnIocnStats},{ColumnHrefUrl},{ColumnRace},{ColumnRarity})" +
                      $"VALUES('{model.Md5}','{model.Name}','{model.IconUrl}','{model.IconStats}','{model.HrefUrl}',{model.Race},{model.Rarity})";
            Console.WriteLine(sql);
            return sql;
        }

        public static string GetUpdateIconSql(CardModel model)
        {
            var sql = $"UPDATE {TableName} SET " +
                      $"{ColumnName}='{model.Name}'，{ColumnIocnUrl}='{model.IconUrl}',{ColumnIocnStats}='{model.IconStats}'，" +
                      $"{ColumnHrefUrl}='{model.HrefUrl}'，{ColumnRace}={model.Race}，{ColumnRarity}={model.Rarity} " +
                      $"WHERE {ColumnMd5}='{model.Md5}'";
            return sql;
        }

        public static string GetUpateImagesSql(CardModel model)
        {
            return
                $"UPDATE {TableName} SET {ColumnImagesUrl}='{model.ImagesUrl}',{ColumnImagesStats}={model.ImagesStats} " +
                $"WHERE {ColumnMd5}='{model.Md5}'";
        }
    }
}