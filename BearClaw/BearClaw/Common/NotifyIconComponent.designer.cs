namespace FileManage.Comm
{
    partial class NotifyIconComponent
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyIconComponent));
            this.taskNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.taskContextMenuStript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenMainView = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseApp = new System.Windows.Forms.ToolStripMenuItem();
            this.taskContextMenuStript.SuspendLayout();
            // 
            // taskNotifyIcon
            // 
            this.taskNotifyIcon.ContextMenuStrip = this.taskContextMenuStript;
            this.taskNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskNotifyIcon.Icon")));
            this.taskNotifyIcon.Text = "狗熊发财";
            this.taskNotifyIcon.Visible = true;
            this.taskNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.taskNotifyIcon_MouseDoubleClick);
            // 
            // taskContextMenuStript
            // 
            this.taskContextMenuStript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMainView,
            this.CloseApp});
            this.taskContextMenuStript.Name = "taskContextMenuStrip";
            this.taskContextMenuStript.Size = new System.Drawing.Size(175, 48);
            // 
            // OpenMainView
            // 
            this.OpenMainView.Name = "OpenMainView";
            this.OpenMainView.Size = new System.Drawing.Size(174, 22);
            this.OpenMainView.Text = "打开程序";
            // 
            // CloseApp
            // 
            this.CloseApp.Name = "CloseApp";
            this.CloseApp.Size = new System.Drawing.Size(174, 22);
            this.CloseApp.Text = "关闭程序";
            this.taskContextMenuStript.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon taskNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip taskContextMenuStript;
        private System.Windows.Forms.ToolStripMenuItem OpenMainView;
        private System.Windows.Forms.ToolStripMenuItem CloseApp;
    }
}
