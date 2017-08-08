﻿using System;
using System.Data;
using System.IO;
using System.Windows;
using ShadowVerse.Command;
using ShadowVerse.Utils;
using Wrapper;
using Wrapper.Utils;

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
            SqliteUtils.FillDataToDataSet(SqlUtils.GetQueryAllSql(), DsAllCache);
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