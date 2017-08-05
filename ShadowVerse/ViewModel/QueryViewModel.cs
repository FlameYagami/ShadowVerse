using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ShadowVerse.Command;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using Wrapper.Constant;
using Wrapper.Utils;

namespace ShadowVerse.ViewModel
{
    public class QueryModelView : BaseViewModle
    {
        public QueryModelView(Grid gvCardPreview, ComboBox cmbPreOrder)
        {
            CmdQuery = new DelegateCommand {ExecuteCommand = Query_Click};
            CmbPreOrder = cmbPreOrder;
            CardModel = new CardModel();
            GvCardPreview = gvCardPreview;
        }

        public ComboBox CmbPreOrder { get; set; }
        public CardModel CardModel { get; set; }
        public DelegateCommand CmdQuery { get; set; }
        private Grid GvCardPreview { get; }

        public void Query_Click(object obj)
        {
            OnPropertyChanged(nameof(CardModel));
            var sql = GetQuerySql();
            var dataSet = new DataSet();
            SqliteUtils.FillDataToDataSet(sql, dataSet);
            var previewList = GetCardPreviewList(dataSet);
            GvCardPreview.DataContext = new CardPreviewViewModel(previewList);
        }

        private string GetQuerySql()
        {
            var previewOrderType = CardUtils.GetPreOrderType(CmbPreOrder.Text);
            var builder = new StringBuilder();
            builder.Append(SqlUtils.GetHeaderSql()); // 基础查询语句
            builder.Append(SqlUtils.GetAllKeySql(CardModel.Key)); // 关键字
            builder.Append(SqlUtils.GetAccurateSql(CardModel.Type, SqliteConst.ColumnType)); // 种类
            builder.Append(SqlUtils.GetAccurateSql(CardModel.Camp, SqliteConst.ColumnCamp)); // 阵营
            builder.Append(SqlUtils.GetAccurateSql(CardModel.Rarity, SqliteConst.ColumnRarity)); // 罕贵
            builder.Append(SqlUtils.GetAccurateSql(CardModel.Cv, SqliteConst.ColumnCv)); // 声优
            builder.Append(SqlUtils.GetPackSql(CardModel.Pack, SqliteConst.ColumnPack)); // 卡包
            builder.Append(SqlUtils.GetIntervalSql(CardModel.Cost, SqliteConst.ColumnCost)); // 费用
            builder.Append(SqlUtils.GetFooterSql(previewOrderType)); // 排序
            return builder.ToString(); // 完整的查询语句
        }

        public List<CardPreviewModel> GetCardPreviewList(DataSet dsPartCache)
        {
            var idList = dsPartCache.Tables[SqliteConst.TableName].Rows.Cast<DataRow>()
                .Select(row => int.Parse(row[SqliteConst.ColumnId].ToString()))
                .ToList();
            var cardModelList = idList.Select(CardUtils.GetCardModel).ToList();
            var previewModelList = cardModelList.Select(card => new CardPreviewModel
            {
                Id = card.Id,
                Name = card.Name,
                CostAndAtkAndLife = $"{card.Cost} / {card.Atk} / {card.Life}",
                ImagePath = CardUtils.GetThumbnailPath(card.Id)
            }).ToList();
            return previewModelList;
        }
    }
}