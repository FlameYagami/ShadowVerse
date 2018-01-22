using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ShadowVerse.Command;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using ShadowVerse.View;
using Wrapper;
using Wrapper.Constant;
using Wrapper.Utils;

namespace ShadowVerse.ViewModel
{
    public class CardQueryModelView : BaseViewModle
    {
        public CardQueryModelView()
        {
            CardQueryModel = new CardQueryModel();
            CmdQuery = new DelegateCommand {ExecuteCommand = Query_Click};
            CmdReset = new DelegateCommand {ExecuteCommand = Reset_Click};
            // Cmb数据绑定
            CvList = CardUtils.GetCvList();
            CampList = Dic.CampCodeDic.Values.ToList();
            RarityList = Dic.RarityCodeDic.Values.ToList();
            PackList = Dic.PackCodeDic.Values.ToList();
            TypeList = Dic.TypeCodeDic.Values.Distinct().ToList();
        }

        public List<string> TypeList { get; set; }
        public List<string> CvList { get; set; }
        public List<string> CampList { get; set; }
        public List<string> RarityList { get; set; }
        public List<string> PackList { get; set; }
        public DelegateCommand CmdReset { get; set; }
        public DelegateCommand CmdQuery { get; set; }
        public CardQueryModel CardQueryModel { get; set; }

        public void Query_Click(object obj)
        {
            OnPropertyChanged(nameof(CardQueryModel));
            var sql = GetQuerySql();
            var dataSet = new DataSet();
            DataManager.FillDataToDataSet(dataSet, sql);
            var previewList = GetCardPreviewList(dataSet);
            ((DeckEditorWindow) AllViewModel.Window).GvCardPreview.DataContext = new CardPreviewViewModel(previewList);
        }

        public void Reset_Click(object obj)
        {
            CardQueryModel = new CardQueryModel();
            OnPropertyChanged(nameof(CardQueryModel));
        }

        private string GetQuerySql()
        {
            // 对应Cmb的值转换成数据库中查询的代码
            var typeCode =
                Dic.TypeCodeDic.FirstOrDefault(type => type.Value.Equals(CardQueryModel.Type)).Key.ToString();
            var campCode =
                Dic.CampCodeDic.FirstOrDefault(camp => camp.Value.Equals(CardQueryModel.Camp)).Key.ToString();
            var rarityCode =
                Dic.RarityCodeDic.FirstOrDefault(rarity => rarity.Value.Equals(CardQueryModel.Rarity))
                    .Key.ToString();
            var packCode =
                Dic.PackCodeDic.FirstOrDefault(pack => pack.Value.Equals(CardQueryModel.Pack)).Key.ToString();
            // 动态添加查询语句
            var builder = new StringBuilder();
            builder.Append(SqlUtils.GetHeaderSql()); // 基础查询语句
            builder.Append(SqlUtils.GetAccurateSql(typeCode, SqliteConst.ColumnType)); // 种类
            builder.Append(SqlUtils.GetAccurateSql(campCode, SqliteConst.ColumnCamp)); // 阵营
            builder.Append(SqlUtils.GetAccurateSql(rarityCode, SqliteConst.ColumnRarity)); // 罕贵
            builder.Append(SqlUtils.GetAccurateSql(packCode, SqliteConst.ColumnPack)); // 卡包
            builder.Append(SqlUtils.GetAccurateSql(CardQueryModel.Cv, SqliteConst.ColumnCv)); // 声优
            builder.Append(SqlUtils.GetIntervalSql(CardQueryModel.Cost, SqliteConst.ColumnCost)); // 费用
            builder.Append(SqlUtils.GetFooterSql()); // 排序
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