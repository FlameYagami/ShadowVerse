using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Wrapper.Constant;
using Wrapper.Utils;

namespace Wrapper
{
    public class DataManager
    {
        public const string DbName = "Data.db";
        public const string DbCfgKey = "DatabasePassword";

        /// <summary>总数据缓存</summary>
        public static DataSet DsAllCache = new DataSet();

        public static string DatabasePath = PathManager.RootPath + DbName;

        public static bool FillDataToDataSet()
        {
            DsAllCache.Clear();
            return SqliteUtils.FillDataToDataSet(DsAllCache, SqliteConst.TableName, DatabasePath, DbCfgKey,
                SqlUtils.GetQueryAllSql());
        }

        public static bool FillDataToDataSet(DataSet dataSet, string sql, bool defTableName = true)
        {
            return defTableName
                ? SqliteUtils.FillDataToDataSet(dataSet, SqliteConst.TableName, DatabasePath, DbCfgKey, sql)
                : SqliteUtils.FillDataToDataSet(dataSet, DatabasePath, DbCfgKey, sql);
        }

        public static bool FillDataToDataSet(DataSet dataSet, string sql, string dsTableName)
        {
            return SqliteUtils.FillDataToDataSet(dataSet, dsTableName, DatabasePath, DbCfgKey, sql);
        }

        public static bool Execute(string sql)
        {
            return SqliteUtils.Execute(DatabasePath, DbCfgKey, sql);
        }

        public static bool Execute(List<string> sqlList)
        {
            return SqliteUtils.Execute(DatabasePath, DbCfgKey, sqlList);
        }

        public static bool Encrypt()
        {
            return SqliteUtils.Encrypt(DatabasePath, DbCfgKey);
        }

        public static bool Encrypt(DataSet dataSet, string sql, string dtTableName)
        {
            return SqliteUtils.Encrypt(dataSet, dtTableName, DatabasePath, DbCfgKey, sql);
        }

        public static bool Encrypt(DataSet dataSet, string sql, bool defTableName = true)
        {
            return defTableName
                ? SqliteUtils.Encrypt(dataSet, SqliteConst.TableName, DatabasePath, DbCfgKey, sql)
                : SqliteUtils.Encrypt(dataSet, null, DatabasePath, DbCfgKey, sql);
        }

        public static bool Decrypt()
        {
            return SqliteUtils.Decrypt(DatabasePath, DbCfgKey);
        }
    }
}
