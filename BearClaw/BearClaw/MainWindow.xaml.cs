using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using BearClaw.Common;
using BearClaw.Models;
using MahApps.Metro.Controls;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace BearClaw
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Init();
            }
        }

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private ObservableCollection<JobEntity> _comparedCollection;
        private int _queryCount;

        private void Init()
        {
            //初始化黑名单数据
            blackMenuView.InitData();
            
            webBrowser.LoadCompleted += (s, e) =>
            {
                tblQueryCount.Text = (++_queryCount).ToString();
                ParseHtml((HTMLDocument)webBrowser.Document);
                if (_comparedCollection.Count > 0)
                {
                    prMain.Visibility = System.Windows.Visibility.Collapsed;
                    rbnStop.IsChecked = true;
                }
            };

            _timer.Interval = new TimeSpan(0,0,Properties.Settings.Default.Frequency);
            _timer.Tick += (s, e) => BeginQueryTask();

            rbnBegin.Checked += (s, e) =>
            {
                _queryCount = 0;
                BeginQueryTask();
                _timer.Start();
            };

            rbnStop.Checked += (s, e) =>
            {
                prMain.Visibility = System.Windows.Visibility.Collapsed;
                _timer.Stop();
            };
        }

        /// <summary>
        /// 开始请求页面
        /// </summary>
        private void BeginQueryTask()
        {
            Uri uri;
            var monitorUrl = string.Format(Properties.Settings.Default.MonitorUrl,DateTime.Now.Ticks);
            if (Uri.TryCreate(monitorUrl, UriKind.RelativeOrAbsolute, out uri))
            {
                //webBrowser.Source = uri;
                webBrowser.Navigate(uri);
                prMain.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// 解析获取到的Html信息
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        public void ParseHtml(HTMLDocument htmlDocument)
        {

            var strContent = ((IHTMLElement)htmlDocument.documentElement).innerText;
            var beginIndex = strContent.IndexOf("个记录") + 3;
            var length = strContent.IndexOf("全选") - beginIndex;
            
            strContent = strContent.Substring(beginIndex, length);

            var list = strContent.Split(new [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var newDic = new Dictionary<string,JobEntity>();

            newDic.Clear();

            foreach (var item in list)
            {
                if (item.Trim().Length > 0 && item.Contains("发布于"))
                {
                    var subList = item.Split(new[] { "发布于" }, StringSplitOptions.RemoveEmptyEntries);
                    var strArray = subList[0].Split(new []{" ","  "},StringSplitOptions.RemoveEmptyEntries);

                    var strName =  strArray.Length > 1 ? strArray[1] : string.Empty;

                    if (!newDic.ContainsKey(strName))
                    {
                        var jobEntity = new JobEntity { StrName = strName };
                        newDic.Add(strName,jobEntity);
                    }
                }
            }

            _comparedCollection = Methods.Compare(blackMenuView.CollectionData, newDic.Values);
            lbMain.ItemsSource = _comparedCollection;
            if (_comparedCollection.Count > 0)
            {
                var strMessage = string.Join("\r\n", from a in _comparedCollection select a.StrName);
                if (Properties.Settings.Default.EnableTip)
                {
                    App.AppComponent.ShowTip(strMessage, "新找到的公司名称");
                }
                if (Properties.Settings.Default.EnableEmail)
                {
                    Methods.SendMail(strMessage);
                }
            }
        }

        private void BtnBlackMenu_OnClick(object sender, RoutedEventArgs e)
        {
            fyBlackMenu.IsOpen = true;
        }

        private void BtnAddToBlackMenu_OnClick(object sender, RoutedEventArgs e)
        {
            rbnStop.IsChecked = true;
            var jobEntity = ((Button) sender).DataContext as JobEntity;
            blackMenuView.CollectionData.Add(jobEntity);
            _comparedCollection.Remove(jobEntity);

        }

        //private void BtnSendConfig_OnClick(object sender, RoutedEventArgs e)
        //{

        //}
    }

}
