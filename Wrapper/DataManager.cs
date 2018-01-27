using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Wrapper
{
    public class DataManager
    {
        public const string BahamutDbName = "Bahamut.db";

        private static string _tableName = "TableName";
        private static string _dbName = "Data.db";
        private static string _dbPath = PathManager.RootPath + _dbName;
        private static string _dbCfgKey = "DatabasePassword";

        /// <summary>总数据缓存</summary>
        public static DataSet DsAllCache = new DataSet();

        public static void Init(string dbName, string dbPath, string dbCfgKey)
        {
            _dbName = dbName;
            _dbPath = dbPath;
            _dbCfgKey = dbCfgKey;
        }

        public static void SetTableName(string tableName)
        {
            _tableName = tableName;
        }

        public static bool ReFillDataToDataSet(DataSet dataSet, string sql, string dsTableName)
        {
            dataSet.Clear();
            return SqliteUtils.FillDataToDataSet(dataSet, dsTableName, _dbPath, _dbCfgKey, sql);
        }

        public static bool FillDataToDataSet(DataSet dataSet, string sql, bool defTableName = true)
        {
            return defTableName
                ? SqliteUtils.FillDataToDataSet(dataSet, _tableName, _dbPath, _dbCfgKey, sql)
                : SqliteUtils.FillDataToDataSet(dataSet, _dbPath, _dbCfgKey, sql);
        }

        public static bool FillDataToDataSet(DataSet dataSet, string sql, string dsTableName)
        {
            return SqliteUtils.FillDataToDataSet(dataSet, dsTableName, _dbPath, _dbCfgKey, sql);
        }

        public static bool Execute(string sql)
        {
            return SqliteUtils.Execute(_dbPath, _dbCfgKey, sql);
        }

        public static bool Execute(List<string> sqlList)
        {
            return SqliteUtils.Execute(_dbPath, _dbCfgKey, sqlList);
        }

        public static bool Encrypt()
        {
            return SqliteUtils.Encrypt(_dbPath, _dbCfgKey);
        }

        public static bool Encrypt(DataSet dataSet, string sql, string dtTableName)
        {
            return SqliteUtils.Encrypt(dataSet, dtTableName, _dbPath, _dbCfgKey, sql);
        }

        public static bool Encrypt(DataSet dataSet, string sql, bool defTableName = true)
        {
            return defTableName
                ? SqliteUtils.Encrypt(dataSet, _tableName, _dbPath, _dbCfgKey, sql)
                : SqliteUtils.Encrypt(dataSet, null, _dbPath, _dbCfgKey, sql);
        }

        public static bool Decrypt()
        {
            return SqliteUtils.Decrypt(_dbPath, _dbCfgKey);
        }
    }
}
