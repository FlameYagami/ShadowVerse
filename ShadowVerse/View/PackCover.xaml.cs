using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Common;
using Dialog;
using ShadowVerse.Model;
using Wrapper;
using Wrapper.Constant;
using Wrapper.Utils;
using ExcelHelper = ShadowVerse.Utils.ExcelHelper;

namespace ShadowVerse.View
{
    /// <summary>
    ///     PackCover.xaml 的交互逻辑
    /// </summary>
    public partial class PackCover
    {
        public PackCover()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnBaseCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("源文件不存在");
                return;
            }
            var packName = TxtPackName.Text.Trim();
            if (packName.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("请输入表格名称");
                return;
            }
            // 获取源文件所有的信息
            var dtSource = new DataSet();
            var isImport = ExcelHelper.ImportExcelToDataTable(filePath, packName, dtSource);
            if (!isImport)
            {
                BaseDialogUtils.ShowDialogAuto("文件中数据异常");
                return;
            }
            // 确认状态
            if (!await BaseDialogUtils.ShowDialogConfirm("确认覆写?"))
                return;
            var sourceCardEntitys = GetSourceCardEntities(dtSource);
            // 生成覆写的数据库语句集合
            var insertSqlList = GetInsertSqlList(sourceCardEntitys);
            // 数据库覆写
            var isExecute = DataManager.Execute(insertSqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }

        public static List<string> GetClearSqlList()
        {
            var dataSet = new DataSet();
            DataManager.FillDataToDataSet(dataSet,SqlUtils.GetQueryAllSql());
//            var idList = dataSet.Tables[SqliteConst.TableName].Rows.Cast<DataRow>()
//                .Select(row => row[SqliteConst.ColumnId].ToString())
//                .Where(id => id.EndsWith("1"))
//                .ToList();
//            return idList.Select(id => $"delete from {SqliteConst.TableName} where {SqliteConst.ColumnId}='{id}'").ToList();
            var idList = dataSet.Tables[SqliteConst.TableName].Rows.Cast<DataRow>()
                .Select(row => row[SqliteConst.ColumnId].ToString())
                .ToList();
            var sqlList =
                idList.AsParallel()
                    .Select(
                        id =>
                            $"update {SqliteConst.TableName} set " +
                            $"{SqliteConst.ColumnCamp}='{id.Substring(3, 1)}'," +
                            $"{SqliteConst.ColumnRarity}='{id.Substring(4, 1)}'," +
                            $"{SqliteConst.ColumnType}='{id.Substring(5, 1)}' " +
                            $"where {SqliteConst.ColumnId}='{id}'")
                    .ToList();
            return sqlList;
        }

        public static List<string> GetInsertSqlList(IEnumerable<CardModel> cardModels)
        {
            return
                cardModels.Select(
                        model =>
                            $"insert into {SqliteConst.TableName} ({SqliteConst.ColumnId},{SqliteConst.ColumnType},{SqliteConst.ColumnCamp},{SqliteConst.ColumnCost}," +
                            $"{SqliteConst.ColumnAtk},{SqliteConst.ColumnLife},{SqliteConst.ColumnEvoAtk},{SqliteConst.ColumnEvoLife},{SqliteConst.ColumnRarity},{SqliteConst.ColumnPack}" +
                            $") values ('" +
                            $"{model.Id}','{model.TypeCode}','{model.CampCode}','{model.Cost}','{model.Atk}','{model.Life}','{model.EvoAtk}','{model.EvoLife}','{model.RarityCode}','{model.PackCode}')")
                    .ToList();
        }

        public static List<string> GetUpdateSqlList(string column, Dictionary<string, string> nameDic)
        {
            return
                nameDic.Select(
                        model =>
                                $"update {SqliteConst.TableName} set {column}='{model.Value}' where {SqliteConst.ColumnId}='{model.Key}'")
                    .ToList();
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
                Id = int.Parse(row[SqliteConst.ColumnId].ToString()),
                TypeCode = int.Parse(row[SqliteConst.ColumnId].ToString().Substring(4, 1)),
                CampCode = int.Parse(row[SqliteConst.ColumnId].ToString().Substring(3, 1)),
                PackCode = int.Parse(row[SqliteConst.ColumnId].ToString().Substring(0, 3)),
                Cost = int.Parse(row[SqliteConst.ColumnCost].ToString()),
                Atk = int.Parse(row[SqliteConst.ColumnAtk].ToString()),
                Life = int.Parse(row[SqliteConst.ColumnLife].ToString()),
                EvoAtk = int.Parse(row[SqliteConst.ColumnEvoAtk].ToString()),
                EvoLife = int.Parse(row[SqliteConst.ColumnEvoLife].ToString()),
                RarityCode = int.Parse(row[SqliteConst.ColumnRarity].ToString())
            }).ToList();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var sqlList = GetClearSqlList();
            // 数据库覆写
            var isExecute = DataManager.Execute(sqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }

        private void BtnAbilitCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("源文件不存在");
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
                        tempAbilityList.Add(dic.Value.ToString());
                    }
                    else
                    {
                        tempAbilityList.Add(dic.Value.ToString());
                        tempAbilityDic.Add(dic.Key.Substring(0, 9), JsonUtils.Serializer(tempAbilityList));
                    }
                else
                    tempAbilityDic.Add(dic.Key, dic.Value.ToString());
            var sqlList = GetUpdateSqlList(SqliteConst.ColumnSkill, tempAbilityDic);
            // 数据库覆写
            var isExecute = DataManager.Execute(sqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }

        private void BtnNameCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("源文件不存在");
                return;
            }
            var jsonString = FileUtils.GetFileContent(filePath);
            var cvDic = JsonUtils.GetDictionary(jsonString);
            // 生成覆写的数据库语句集合
            var insertSqlList = GetUpdateSqlList(SqliteConst.ColumnName, cvDic);
            // 数据库覆写
            var isExecute = DataManager.Execute(insertSqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }

        private void BtnCvCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("源文件不存在");
                return;
            }
            var jsonString = FileUtils.GetFileContent(filePath);
            var cvDic = JsonUtils.GetDictionary(jsonString);
            // 生成覆写的数据库语句集合
            var insertSqlList = GetUpdateSqlList(SqliteConst.ColumnCv, cvDic);
            // 数据库覆写
            var isExecute = DataManager.Execute(insertSqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }

        private void BtnLinesCover_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TxtFilePath.Text.Trim();
            if (filePath.Equals(""))
            {
                BaseDialogUtils.ShowDialogAuto("源文件不存在");
                return;
            }
            var jsonString = FileUtils.GetFileContent(filePath);
            var flavourDic = JsonUtils.GetDictionary(jsonString);
            var tempFlavourDic = new Dictionary<string, string>();
            var tempFlavourList = new List<string>();
            foreach (var dic in flavourDic)
                if (dic.Key.EndsWith("_01") || dic.Key.EndsWith("_02"))
                    if (dic.Key.EndsWith("_01"))
                    {
                        tempFlavourList.Clear();
                        tempFlavourList.Add(dic.Value.ToString());
                    }
                    else
                    {
                        tempFlavourList.Add(dic.Value.ToString());
                        tempFlavourDic.Add(dic.Key.Substring(0, 9), JsonUtils.Serializer(tempFlavourList));
                    }
                else
                    tempFlavourDic.Add(dic.Key, dic.Value.ToString());
            var sqlList = GetUpdateSqlList(SqliteConst.ColumnFlavour, tempFlavourDic);
            // 数据库覆写
            var isExecute = DataManager.Execute(sqlList);
            BaseDialogUtils.ShowDialogAuto(isExecute ? "Succeed" : "Failed");
        }
    }
}