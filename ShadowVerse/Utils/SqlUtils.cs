using ShadowVerse.Constant;
using Wrapper.Constant;

namespace ShadowVerse.Utils
{
    public class SqlUtils : Wrapper.SqlUtils
    {
        public static string GetQueryAllSql()
        {
            return $"SELECT * FROM {SqliteConst.TableName} ORDER BY {SqliteConst.ColumnId} ASC";
        }

        /// <summary>
        ///     获取头部查询语句
        /// </summary>
        /// <returns></returns>
        public static string GetHeaderSql()
        {
            return $"SELECT * FROM {SqliteConst.TableName} WHERE 1=1";
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
            return $" ORDER BY {SqliteConst.ColumnId} ASC";
        }

        /// <summary>
        ///     获取数值排序方式查询语句
        /// </summary>
        /// <returns></returns>
        private static string GetOrderValueSql()
        {
            return $" ORDER BY {SqliteConst.ColumnCamp} DESC,{SqliteConst.ColumnCost} DESC,{SqliteConst.ColumnAtk} DESC,{SqliteConst.ColumnLife} DESC,{SqliteConst.ColumnName} DESC";
        }
    }
}