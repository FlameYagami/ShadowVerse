using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Wrapper.Constant;

namespace Wrapper.Utils
{
    public class SqliteUtils
    {
        public const string DatabaseName = "data.db";
        public const string DatabasePassword = "DatabasePassword";

        public static string DatabasePath = $"Data Source='{PathManager.RootPath + DatabaseName}'";

        /// <summary>数据填充至DataSet</summary>
        public static bool FillDataToDataSet(string sql, DataSet dts)
        {
            using (var con = new SQLiteConnection(DatabasePath))
            {
                try
                {
                    con.SetPassword("");
                    con.Open();
                    var cmd = new SQLiteCommand(sql, con);
                    var dap = new SQLiteDataAdapter(cmd);
                    dts.Clear();
                    dap.Fill(dts, SqliteConst.TableName);
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Execute(string sql)
        {
            using (var con = new SQLiteConnection(DatabasePath))
            {
                try
                {
                    con.SetPassword("");
                    con.Open();
                    var cmd = new SQLiteCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return false;
                }
                return true;
            }
        }

        public static bool Execute(List<string> sqlList)
        {
            using (var con = new SQLiteConnection(DatabasePath))
            {
                con.SetPassword("");
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(con))
                    {
                        try
                        {
                            cmd.Transaction = trans;
                            foreach (var sql in sqlList)
                            {
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        ///     加密数据库
        /// </summary>
        public static bool Encrypt(DataSet ds)
        {
            try
            {
                var pwdF = ConfigUtils.Get(DatabasePassword);
                var pwdT = StringUtils.Decrypt(pwdF);
                using (var con = new SQLiteConnection(DatabasePath))
                {
                    con.Open();
                    con.ChangePassword(pwdT);
                    con.Close();
                }
                FillDataToDataSet(SqlUtils.GetQueryAllSql(), ds); // 加密完数据库后，重新读取数据
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     解密数据库
        /// </summary>
        public static bool Decrypt()
        {
            try
            {
                var pwdF = ConfigUtils.Get(DatabasePassword);
                var pwdT = StringUtils.Decrypt(pwdF);
                using (var con = new SQLiteConnection(DatabasePath))
                {
                    con.SetPassword(pwdT);
                    con.Open();
                    con.ChangePassword(string.Empty);
                    con.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}