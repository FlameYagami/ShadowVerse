using System;
using System.Data;
using System.IO;
using System.Windows;
using Common;
using Wrapper;
using SqlUtils = ShadowVerse.Utils.SqlUtils;

namespace ShadowVerse.ViewModel
{
    public class AllViewModel
    {
        public static DataSet DsAllCache = new DataSet();
        public DelegateCommand CmdExit { get; set; }

        public AllViewModel(Window window)
        {
            Window = window;
            CmdExit = new DelegateCommand { ExecuteCommand = Exit_Click };
            DataManager.FillDataToDataSet(DsAllCache, SqlUtils.GetQueryAllSql());
            if (!Directory.Exists(PathManager.DeckFolderPath))
                Directory.CreateDirectory(PathManager.DeckFolderPath);
        }

        public static Window Window { get; set; }

        public void Exit_Click(object obj)
        {
            Environment.Exit(0);
        }
    }
}