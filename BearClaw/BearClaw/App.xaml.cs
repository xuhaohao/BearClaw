using FileManage.Comm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace BearClaw
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public static NotifyIconComponent AppComponent { set; get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindowClosing;
            MainWindow.Show();//初始化主界面
            AppComponent = new NotifyIconComponent();
            AppComponent.OnOpenMainView += (obj, args) =>
            {
                //处理现实主界面事件
                if (WindowState.Minimized == MainWindow.WindowState)
                {
                    //恢复窗体
                    MainWindow.WindowState = WindowState.Normal;
                    MainWindow.ShowInTaskbar = true;
                    AppComponent.ShowTip("重新打开程序","提示");
                }
            };
            
        }

        void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;//拦截关闭事件
            MainWindow.WindowState = WindowState.Minimized;
            MainWindow.ShowInTaskbar = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            AppComponent.Dispose();
        }
    }
}
