using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Dialog;
using ShadowVerse.Model;
using Wrapper.Constant;
using Wrapper.Utils;

namespace ShadowVerse.View
{
    /// <summary>
    ///     PackCover.xaml 的交互逻辑
    /// </summary>
    public partial class PackCover : Window
    {
        public PackCover()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDlg("源文件不存在");
                return;
            }
            var packName = TxtPackName.Text.Trim();
            if (packName.Equals(""))
            {
                BaseDialogUtils.ShowDlg("请输入卡包名称");
                return;
            }
            var jsonString = FileUtils.GetFileContent(filePath);
            var nameDic = JsonUtils.GetDictionary(jsonString);
            // 生成覆写的数据库语句集合
            var insertSqlList = GetNameSqlList(nameDic);
            // 数据库覆写
            var isExecute = SqliteUtils.Execute(insertSqlList);
            BaseDialogUtils.ShowDlg(isExecute ? "Succeed" : "Failed");


            //            // 获取源文件所有的信息
            //            var dtSource = new DataSet();
            //            var isImport = ExcelHelper.ImportExcelToDataTable(filePath, packName, dtSource);
            //            if (!isImport)
            //            {
            //                BaseDialogUtils.ShowDlg("文件中数据异常");
            //                return;
            //            }
            //            // 确认状态
            //            if (!BaseDialogUtils.ShowDlgOkCancel("确认覆写?"))
            //                return;
            //            var sourceCardEntitys = GetSourceCardEntities(dtSource);
            //            // 生成覆写的数据库语句集合
            //            var insertSqlList = GetInsertSqlList(sourceCardEntitys);
            //            // 数据库覆写
            //            var isExecute = SqliteUtils.Execute(insertSqlList);
            //            BaseDialogUtils.ShowDlg(isExecute ? "Succeed" : "Failed");
        }

        public static List<string> GetClearSqlList()
        {
            var dataSet = new DataSet();
            SqliteUtils.FillDataToDataSet(SqlUtils.GetQueryAllSql(), dataSet);
            var idList = dataSet.Tables[SqliteConst.TableName].Rows.Cast<DataRow>()
                .Select(row => row["id"].ToString())
                .Where(id => id.EndsWith("1"))
                .ToList();
            return idList.Select(id => $"delete from card where id='{id}'").ToList();
        }

        public static List<string> GetInsertSqlList(IEnumerable<CardModel> cardModels)
        {
            return
                cardModels.Select(
                        model =>
                                $"insert into card (id,type,camp,cost,atk,life,evo_atk,evo_life,rarity,pack) values ('{model.Id}','{model.Type}','{model.Camp}','{model.Cost}','{model.Atk}','{model.Life}','{model.EvoAtk}','{model.EvoLife}','{model.Rarity}','{model.Pack}')")
                    .ToList();
        }

        public static List<string> GetNameSqlList(Dictionary<string, string> nameDic)
        {
            return nameDic.Select(model => $"update card set name='{model.Value}' where id='{model.Key}'").ToList();
        }

        public static List<string> GetAbilitySqlList(Dictionary<string, string> nameDic)
        {
            return nameDic.Select(model => $"update card set ability='{model.Value}' where id='{model.Key}'").ToList();
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            TxtFilePath.Text = ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK ? string.Empty : ofd.FileName;
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private IEnumerable<CardModel> GetSourceCardEntities(DataSet dataSet)
        {
            return dataSet.Tables[0].Rows.Cast<DataRow>().Select(row => new CardModel
            {
                Id = int.Parse(row["id"].ToString()),
                Type = row["id"].ToString().Substring(4, 1),
                Camp = row["id"].ToString().Substring(3, 1),
                Pack = row["id"].ToString().Substring(0, 3),
                Cost = row["cost"].ToString(),
                Atk = row["atk"].ToString(),
                Life = row["life"].ToString(),
                EvoAtk = row["evo_atk"].ToString(),
                EvoLife = row["evo_atk"].ToString(),
                Rarity = row["rarity"].ToString()
            }).ToList();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var sqlList = GetClearSqlList();
            // 数据库覆写
            var isExecute = SqliteUtils.Execute(sqlList);
            BaseDialogUtils.ShowDlg(isExecute ? "Succeed" : "Failed");
        }

        private void BtnAbilitCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDlg("源文件不存在");
                return;
            }
            var jsonString = FileUtils.GetFileContent(filePath);
            var abilityDic = JsonUtils.GetDictionary(jsonString);
            var tempAbilityDic = new Dictionary<string, string>();
            var tempAbilityList = new List<string>();
            foreach (var dic in abilityDic)
                if (dic.Key.EndsWith("_01") || dic.Key.EndsWith("_02"))
                    if (dic.Key.EndsWith("_01"))
                    {
                        tempAbilityList.Clear();
                        tempAbilityList.Add(dic.Value);
                    }
                    else
                    {
                        tempAbilityList.Add(dic.Value);
                        tempAbilityDic.Add(dic.Key.Substring(0, 9), JsonUtils.JsonSerializer(tempAbilityList));
                    }
                else
                    tempAbilityDic.Add(dic.Key, dic.Value);
            var sqlList = GetAbilitySqlList(tempAbilityDic);
            // 数据库覆写
            var isExecute = SqliteUtils.Execute(sqlList);
            BaseDialogUtils.ShowDlg(isExecute ? "Succeed" : "Failed");
        }

        private void BtnNameCover_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}