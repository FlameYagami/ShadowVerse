using Wrapper.Constant;

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
        /// <returns></returns>
        public static string GetFooterSql()
        {
            return GetOrderValueSql();
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
            return !"-1".Equals(value) && !StringConst.NotApplicable.Equals(value)
                ? $" AND {column}='{value}'"
                : string.Empty;
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
    }
}