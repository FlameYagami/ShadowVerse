using System;
using System.Windows;
using BahamutCardCrawler.Model;
using Common;

namespace BahamutCardCrawler.ViewModel
{
    public class AppbarVm
    {
        private readonly Window _window;
        public  AppbarModel AppbarModel { get; set; }

        public DelegateCommand CmdMinimize { get; set; }
        public DelegateCommand CmdRestore { get; set; }
        public DelegateCommand CmdMaximize { get; set; }
        public DelegateCommand CmdExit { get; set; }

        public AppbarVm(Window window)
        {
            _window = window;
            CmdRestore = new DelegateCommand { ExecuteCommand = Restore_Click };
            CmdMaximize = new DelegateCommand { ExecuteCommand = Maximize_Click };
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
            _window.WindowState = WindowState.Minimized;
        }

        public void Maximize_Click(object obj)
        {
            _window.Topmost = true;
            _window.WindowState = WindowState.Maximized;
            _window.Hide(); //先调用其隐藏方法 然后再显示出来,这样就会全屏,且任务栏不会出现.如果不加这句 可能会出现假全屏即任务栏还在下面.
            _window.Show();
            AppbarModel.Maximize();
        }

        public void Restore_Click(object obj)
        {
            _window.Topmost = false;
            _window.WindowState = WindowState.Normal;
            AppbarModel.Restore();
        }
    }
}
