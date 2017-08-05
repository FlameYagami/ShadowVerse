using System;
using System.Text;
using Wrapper.Constant;
using Enum = CardEditor.Constant.Enum;

namespace Wrapper.Utils
{
    public class SqlUtils : SqliteConst
    {
        public static string GetQueryAllSql()
        {
            return $"SELECT * FROM {TableName} ORDER BY {ColumnId} ASC";
        }

        /// <summary>
        ///     获取头部查询语句
        /// </summary>
        /// <returns></returns>
        public static string GetHeaderSql()
        {
            return $"SELECT * FROM {TableName} WHERE 1=1";
        }

        /// <summary>
        ///     获取尾部查询语句
        /// </summary>
        /// <param name="previewOrderType">排序枚举类型</param>
        /// <returns></returns>
        public static string GetFooterSql(Enum.PreviewOrderType previewOrderType)
        {
            return previewOrderType.Equals(Enum.PreviewOrderType.Number)
                ? GetOrderNumberSql()
                : GetOrderValueSql();
        }

        /// <summary>
        ///     获取卡编排序方式查询语句
        /// </summary>
        /// <returns></returns>
        private static string GetOrderNumberSql()
        {
            return $" ORDER BY {ColumnId} ASC";
        }

        /// <summary>
        ///     获取数值排序方式查询语句
        /// </summary>
        /// <returns></returns>
        private static string GetOrderValueSql()
        {
            return $" ORDER BY {ColumnCamp} DESC,{ColumnCost} DESC,{ColumnAtk} DESC,{ColumnLife} DESC,{ColumnName} DESC";
        }

        /// <summary>
        ///     获取精确取值
        /// </summary>
        /// <param name="value"></param>
        /// <returns>数据库查询语句</returns>
        public static string GetAccurateValue(string value)
        {
            return StringConst.NotApplicable.Equals(value) ? string.Empty : value.Replace("'", "''");
            // 处理字符串中对插入语句影响的'号
        }

        /// <summary>
        ///     获取精确查询语句
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column">数据库字段</param>
        /// <returns>数据库查询语句</returns>
        public static string GetAccurateSql(string value, string column)
        {
            return !StringConst.NotApplicable.Equals(value) ? $" AND {column}='{value}'" : string.Empty;
        }

        /// <summary>
        ///     获取模糊查询语句
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column">数据库字段</param>
        /// <returns>数据库查询语句</returns>
        public static string GetSimilarSql(string value, string column)
        {
            return !string.Empty.Equals(value) ? $" AND {column} LIKE '%{value}%'" : string.Empty;
        }

        /// <summary>
        ///     获取数值查询语句（适用范围:费用、力量）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column">数据库字段</param>
        /// <returns>数据库查询语句</returns>
        public static string GetIntervalSql(string value, string column)
        {
            return !string.Empty.Equals(value)
                ? (value.Contains(StringConst.Hyphen)
                    ? $" AND {column}>='{value.Split('-')[0]}' AND {column}<='{value.Split('-')[1]}'"
                    : $" AND {column}='{value}'")
                : string.Empty;
        }

        /// <summary>
        ///     获取卡包查询语句
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string GetPackSql(string value, string column)
        {
            if (StringConst.NotApplicable.Equals(value))
                return string.Empty;
            value = value.IndexOf(StringConst.Series, StringComparison.Ordinal) >= 0 ? value.Substring(0, 1) : value;
            return $" AND {column} LIKE '%{value}%'";
        }

        public static string GetExportSql(string pack)
        {
            return $"SELECT * FROM {TableName} WHERE {ColumnPack} LIKE '%{pack}%' {GetOrderNumberSql()}";
        }


//        /// <summary>
//        ///     获取能力类型的查询语句
//        /// </summary>
//        /// <param name="abilityTypeDic">能力类型字典</param>
//        /// <returns></returns>
//        public static string GetAbilityTypeSql(Dictionary<string, bool> abilityTypeDic)
//        {
//            var value = new StringBuilder();
//            foreach (var entity in abilityTypeDic)
//                if (entity.Value)
//                    foreach (var abilityTypeItem in Dictionary.AbilityTypeDic)
//                        if (abilityTypeItem.Key.Equals(entity.Key))
//                            value.Append($" AND {ColumnSkill} LIKE '%{abilityTypeItem.Value}%'");
//            return value.ToString();
//        }

        /// <summary>
        ///     获取关键字段取值
        /// </summary>
        /// <returns></returns>
        public static string GetAllKeySql(string value)
        {
            if (value.Equals(string.Empty)) return string.Empty;
            var tempValue = new StringBuilder();
            var keyList = value.Split(' '); // 以空格分割关键字
            foreach (var key in keyList)
                tempValue.Append($" AND ( {ColumnName} LIKE '%{key}%' " + GetPartKeySql(key) + ")");
            return tempValue.ToString();
        }

        private static string GetPartKeySql(string value)
        {
            var tempValue = new StringBuilder();
            foreach (var column in ColumKeyArray)
                tempValue.Append($" OR {column} LIKE '%{value}%'");
            return tempValue.ToString();
        }
    }
}