using System;
using System.Data;
using System.IO;
using System.Windows;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ShadowVerse.Utils
{
    internal class ExcelHelper
    {
        public static bool ExportPackToExcel(string filePath, DataSet dataSet)
        {
            foreach (DataTable dt in dataSet.Tables)
                try
                {
                    var icolIndex = 0;
                    var iRowIndex = 1;
                    var iCellIndex = 0;
                    var workbook = new HSSFWorkbook();
                    var sheet = workbook.CreateSheet(dt.TableName);
                    var headerRow = sheet.CreateRow(0);
                    foreach (DataColumn item in dt.Columns)
                    {
                        var cell = headerRow.CreateCell(icolIndex);
                        cell.SetCellValue(item.ColumnName);
                        icolIndex++;
                    }
                    foreach (DataRow rowitem in dt.Rows)
                    {
                        var dataRow = sheet.CreateRow(iRowIndex);
                        foreach (DataColumn colitem in dt.Columns)
                        {
                            var cell = dataRow.CreateCell(iCellIndex);
                            cell.SetCellValue(rowitem[colitem].ToString());
                            iCellIndex++;
                        }
                        iCellIndex = 0;
                        iRowIndex++;
                    }
                    var file = new FileStream(filePath, FileMode.OpenOrCreate);
                    workbook.Write(file);
                    file.Flush();
                    file.Close();
                    file.Dispose();
                }
                catch (Exception)
                {
                    return false;
                }
            return true;
        }

        /// <summary>
        ///     将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filePath">excel路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="dataSet">Output 数据集</param>
        /// <returns>返回的DataTable</returns>
        public static bool ImportExcelToDataTable(string filePath, string sheetName, DataSet dataSet)
        {
            var dt = new DataTable();
            try
            {
                ISheet sheet = null;
                IWorkbook fileWorkbook = null;
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if ((filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0) ||
                    (filePath.IndexOf(".xlsm", StringComparison.Ordinal) > 0))
                    fileWorkbook = new XSSFWorkbook(fs); // 2007版本
                else
                    fileWorkbook = new HSSFWorkbook(fs); // 2003版本
                sheet = fileWorkbook.GetSheet(sheetName);
                if (sheet == null)
                    return false;
                sheet = fileWorkbook.GetSheet(sheetName);

                if (sheet == null)
                    return false;
                var firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号，即总的列数  
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    var cell = firstRow.GetCell(i);
                    var cellValue = cell?.StringCellValue;
                    var column = cellValue == null ? new DataColumn("Column" + i) : new DataColumn(cellValue);
                    dt.Columns.Add(column);
                }
                var startRow = sheet.FirstRowNum + 1;

                //最后一列的标号  
                var rowCount = sheet.LastRowNum;
                for (var i = startRow; i <= rowCount; ++i)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　  

                    var dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null  
                            dataRow[j] = row.GetCell(j).ToString();
                    dt.Rows.Add(dataRow);
                }
                dataSet.Tables.Add(dt);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }
    }
}