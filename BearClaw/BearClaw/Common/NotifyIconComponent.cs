using System;
using System.ComponentModel;

namespace FileManage.Comm
{
    using System.Windows.Forms;

    public partial class NotifyIconComponent : Component
    {
        public NotifyIconComponent()
        {
            InitializeComponent();
            taskNotifyIcon.Visible = true;
            OpenMainView.Click += OpenMainViewClick;
            CloseApp.Click += CloseAppClick;
        }

        public EventHandler OnOpenMainView;//点击打开主界面时触发的事件

        void OpenMainViewClick(object sender, EventArgs e)
        {
            if (null != OnOpenMainView)
                OnOpenMainView(this, EventArgs.Empty);
        }

        public void ShowTip(string msg,string tipTitle="Tip")
        {
            taskNotifyIcon.ShowBalloonTip(3000, tipTitle, msg,  ToolTipIcon.Info); 
        }

        void CloseAppClick(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public NotifyIconComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void taskNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}