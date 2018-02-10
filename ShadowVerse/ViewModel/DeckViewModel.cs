using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using Dialog;
using ShadowVerse.Constant;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using ShadowVerse.View;
using Visifire.Charts;
using Wrapper;
using Wrapper.Constant;

namespace ShadowVerse.ViewModel
{
    public class DeckViewModel : BaseModel
    {
        public DeckViewModel()
        {
            CreateChartColumn();
            DeckList = new ObservableCollection<DeckModel>();
            DeckNameList = new ObservableCollection<string>();
            CmdDeckSave = new DelegateCommand {ExecuteCommand = DeckSave_Click};
            CmdDeckResave = new DelegateCommand {ExecuteCommand = DeckResave_Click};
            CmdDeckDelete = new DelegateCommand {ExecuteCommand = DeckDelete_Click};
            CmdDeckClear = new DelegateCommand {ExecuteCommand = DeckClear_Click};
            FollowerCount = 0;
            AmuletCount = 0;
            SpellCount = 0;
        }

        public string DeckName { get; set; }
        public DelegateCommand CmdDeckSave { get; set; }
        public DelegateCommand CmdDeckResave { get; set; }
        public DelegateCommand CmdDeckDelete { get; set; }
        public DelegateCommand CmdDeckClear { get; set; }
        public ObservableCollection<DeckModel> DeckList { get; set; }
        public ObservableCollection<string> DeckNameList { get; set; }
        public int FollowerCount { get; set; }
        public int AmuletCount { get; set; }
        public int SpellCount { get; set; }

        public void DeckNameLoad()
        {
            var deckFolder = new DirectoryInfo(PathManager.DeckFolderPath);
            var deckFiles = deckFolder.GetFiles(); //遍历文件
            var deckNameList = deckFiles
                .Where(deckFile => StringConst.DeckExtension.Equals(deckFile.Extension))
                .Select(deckName => Path.GetFileNameWithoutExtension(deckName.FullName))
                .ToList();
            DeckNameList.Clear();
            foreach (var s in deckNameList)
                DeckNameList.Add(s);
        }

        public void DeckLoad()
        {
            OnPropertyChanged(nameof(DeckName));
            if ((DeckName == null) || DeckName.Equals(string.Empty)) return;
            DeckList.Clear();
            var deckPath = CardUtils.GetDeckPath(DeckName);
            try
            {
                var sr = File.OpenText(deckPath);
                var numberListString = sr.ReadToEnd().Trim();
                sr.Close();
                var idList = JsonUtils.Deserialize<List<int>>(numberListString);
                foreach (var id in idList)
                    AddCard(id);
            }
            catch (Exception exception)
            {
                BaseDialogUtils.ShowDialogAuto(exception.Message);
            }
        }

        public void DeckSave_Click(object obl)
        {
            OnPropertyChanged(nameof(DeckName));
            if (DeckName.Equals(string.Empty))
            {
                BaseDialogUtils.ShowDialogAuto(StringConst.DeckNameNone);
                return;
            }
            var deckPath = CardUtils.GetDeckPath(DeckName);
            var deckBuilder = new StringBuilder();
            var deckNumberList = new List<int>();
            deckNumberList.AddRange(DeckList.Select(deck => deck.Id).ToList());
            deckBuilder.Append(JsonUtils.Serializer(deckNumberList));
            var isSaveSucceed = FileUtils.SaveFile(deckPath, deckBuilder.ToString());
            BaseDialogUtils.ShowDialogAuto(isSaveSucceed ? StringConst.SaveSucceed : StringConst.SaveFailed);
        }

        public void DeckResave_Click(object obj)
        {
            OnPropertyChanged(nameof(DeckName));
            if (DeckName.Equals(string.Empty))
            {
                BaseDialogUtils.ShowDialogAuto(StringConst.DeckNameNone);
                return;
            }
            var deckPath = CardUtils.GetDeckPath(DeckName);
            if (File.Exists(deckPath))
            {
                BaseDialogUtils.ShowDialogAuto(StringConst.DeckNameExist);
                return;
            }
            DeckSave_Click(DeckName);
        }

        public void DeckClear_Click(object obj)
        {
            DeckList.Clear();
        }

        public async void DeckDelete_Click(object obj)
        {
            OnPropertyChanged(nameof(DeckName));
            if (DeckName.Equals(string.Empty)) return;
            if (!await BaseDialogUtils.ShowDialogConfirm(StringConst.DeleteHint)) return;
            var deckPath = CardUtils.GetDeckPath(DeckName);
            if (!File.Exists(deckPath)) return;
            File.Delete(deckPath);
            DeckList.Clear();
        }

        public void AddCard(int id)
        {
            if (!CheckCard(id)) return;
            var cardModel = CardUtils.GetCardModel(id);
            var deckModel = new DeckModel
            {
                Id = id,
                TypeCode = cardModel.TypeCode,
                CampCode = cardModel.CampCode,
                Cost = cardModel.Cost,
                Name = cardModel.Name,
                ImagePath = CardUtils.GetThumbnailPath(id)
            };
            DeckList.Add(deckModel);
            DeckOrder();
            UpdateChartColumn(DeckStatistical());
            GetTypeCountList();
        }

        public void DeleteCard(int id)
        {
            var tempModel = DeckList.AsParallel().AsEnumerable().First(model => model.Id.Equals(id));
            DeckList.Remove(tempModel);
            UpdateChartColumn(DeckStatistical());
            GetTypeCountList();
        }

        private void DeckOrder()
        {
            var tempDeckList = DeckList.AsEnumerable().OrderByDescending(deck => deck.CampCode)
                .ThenByDescending(deck => deck.TypeCode)
                .ThenByDescending(deck => deck.Cost)
                .ThenBy(deck => deck.Name)
                .ToList();
            DeckList.Clear();
            tempDeckList.ForEach(DeckList.Add);
        }

        private bool CheckCard(int id)
        {
            var name = CardUtils.GetName(id);
            var canAdd = !CardUtils.IsToekn(id)
                         && (DeckList.Count < 40)
                         && (DeckList.AsParallel().Count(deckModel => name.Equals(deckModel.Name)) < 3);
            return canAdd;
        }

        private void GetTypeCountList()
        {
            FollowerCount = DeckList.Count(deck => deck.TypeCode == StringConst.FollowerCode);
            AmuletCount =
                DeckList.Count(
                    deck => (deck.TypeCode == StringConst.AmuletCode) || (deck.TypeCode == StringConst.AmuletExCode));
            SpellCount = DeckList.Count(deck => deck.TypeCode == StringConst.SpellCode);
            OnPropertyChanged(nameof(FollowerCount));
            OnPropertyChanged(nameof(AmuletCount));
            OnPropertyChanged(nameof(SpellCount));
        }

        public Dictionary<int, int> DeckStatistical()
        {
            var dekcStatisticalDic = new Dictionary<int, int>();
            var costList = DeckList.Select(deck => deck.Cost);
            var costDeckList = new List<int>();
            costDeckList.AddRange(costList);
            for (var i = 0; i != 9; i++)
                dekcStatisticalDic.Add(i + 1, costDeckList.Count(cost => cost.Equals(i + 1)));
            return dekcStatisticalDic;
        }

        public void CreateChartColumn()
        {
            var chartDeck = ((DeckEditorWindow)AppbarVm.Window).ChartDeck;

            // 创建一个新的数据线。               
            var dataSeries = new DataSeries
            {
                RenderAs = RenderAs.StackedColumn,
                LabelEnabled = true,
                LabelText = "#YValue"
            };
            // 设置数据点 
            var i = 1;
            while (i++ < 10)
            {
                // 创建一个数据点的实例。                   
                var dataPoint = new DataPoint
                {
                    // 设置X轴点
                    XValue = i,
                    //设置Y轴点      
                    YValue = 0
                };
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }
            // 添加数据线到数据序列。                
            chartDeck.Series.Add(dataSeries);
        }

        public void UpdateChartColumn(Dictionary<int, int> statisticsDic)
        {
            var chartDeck = ((DeckEditorWindow)AppbarVm.Window).ChartDeck;
            // 创建一个新的数据线。               
            var dataSeries = new DataSeries
            {
                RenderAs = RenderAs.StackedColumn,
                LabelEnabled = true,
                LabelText = "#YValue"
            };
            foreach (var item in statisticsDic)
            {
                // 创建一个数据点的实例。                   
                var dataPoint = new DataPoint
                {
                    // 设置X轴点
                    XValue = int.Parse(item.Key.ToString()),
                    //设置Y轴点      
                    YValue = int.Parse(item.Value.ToString())
                };
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }
            // 添加数据线到数据序列。   
            chartDeck.Series.Clear();
            chartDeck.Series.Add(dataSeries);
        }
    }
}