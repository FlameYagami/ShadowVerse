using System;
using System.Windows;
using BahamutCardCrawler.Model;
using Common;

namespace ShadowVerse.ViewModel
{
    public class AppbarVm
    {
        public static Window Window;
        public  AppbarModel AppbarModel { get; set; }

        public DelegateCommand CmdMinimize { get; set; }

        public DelegateCommand CmdExit { get; set; }

        public AppbarVm(Window window)
        {
            Window = window;
            CmdExit = new DelegateCommand { ExecuteCommand = Exit_Click };
            CmdMinimize = new DelegateCommand { ExecuteCommand = Minimize_Click };
            AppbarModel = new AppbarModel();
        }


        public void Exit_Click(object obj)
        {
            Environment.Exit(0);
        }

        public void Minimize_Click(object obj)
        {
            Window.WindowState = WindowState.Minimized;
        }
    }
}
