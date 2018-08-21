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
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Reflection;
using System.Threading;
using BearClaw.Strategy;
using System.Windows.Documents;
using Microsoft.Win32;

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

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainWindow));

        //private int _naviUrlIndex;

        //private int _keywordListIndex;

        private int _queryIndex;                    //最大值为策略列表长度*关键字长度;

        private ObservableCollection<Jobs> _jobCollections;

        private int _queryCount;

        private DispatcherTimer _timer = new DispatcherTimer();


        private void Init()
        { 
            //初始化黑名单数据
            webBrowser.LoadCompleted += (s, e) =>
            {
                _queryCount++;
                tblQueryCount.Text = string.Format("[{0}]", _queryCount);
                try
                {
                    Debug.Print("curr uri:{0}", e.Uri);
                    HTMLDocument htmlDocument = (HTMLDocument)webBrowser.Document;

                    var domain = e.Uri.Host;
                    var innerHtml = (htmlDocument.documentElement).innerHTML;

                    new Thread(() =>
                    {
                        ParseHtml(domain, innerHtml);
                    }).Start();
                }
                catch (Exception ex)
                {
                    log.Error("webBrowser.LoadCompleted:", ex);
                }
            };

            webBrowser.Navigating += (s, e) =>
            {
                FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    object browser = fi.GetValue(webBrowser);
                    if (browser != null)
                        browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { true });
                }
            };

            rbnBegin.Checked += (s, e) =>
            {
                //_naviUrlIndex = 0;
                _queryIndex = 0;
                _timer.IsEnabled = true;
                mpbMain.Visibility = Visibility.Visible;
            };

            rbnStop.Checked += (s, e) =>
            {
                _timer.IsEnabled = false;
                MailTask.Release(sendResult => {
                    if (sendResult.Count > 0)
                    {
                        Db.CreateUpdate(sendResult);
                        Dispatcher.Invoke(() => {
                            foreach (var job in sendResult)
                            {
                                _jobCollections.Insert(0, job);
                            }
                            tblCount.Text = _jobCollections.Count.ToString();
                        });
                    }
                });
                mpbMain.Visibility = Visibility.Collapsed;
            };

            _jobCollections = Db.GetJobs();
            dgMain.ItemsSource = _jobCollections;
            tblCount.Text = _jobCollections.Count.ToString();

            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Tick += (s, e) =>
            {
                BeginQueryTask();
                //指向下一个网址
                //_naviUrlIndex = _naviUrlIndex < MyStrategy.List.Count - 1 ? _naviUrlIndex + 1 : 0;
                _queryIndex = _queryIndex < MyStrategy.List.Count * MyStrategy.KewWordList.Count - 1 ? _queryIndex + 1 : 0;
            };
            _timer.Start();
            _timer.IsEnabled = false;

            log.Info("初始化MainWindow 完成");
        }


        /// <summary>
        /// 开始请求页面
        /// </summary>
        private void BeginQueryTask()
        {
            //开始索引下一个Url
            if (webBrowser.IsLoaded)
            {
                var strategyIndex = _queryIndex / MyStrategy.List.Count;
                var keywordIndex = _queryIndex % MyStrategy.KewWordList.Count;
                log.Info("strategyIndex=" + strategyIndex + ",keywordIndex=" + keywordIndex);
                MyStrategy myStrategy = MyStrategy.List[strategyIndex];
                var keyword = MyStrategy.KewWordList[keywordIndex];
                log.Info("strategyIndex[" + myStrategy.GetDomain() + "]=" + strategyIndex + ",keywordIndex=" + keywordIndex + ",keyword="+ keyword);
                var uri = myStrategy.GetUri(keyword);
                webBrowser.Navigate(uri);
            }
        }

        /// <summary>
        /// 解析获取到的Html信息
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        public void ParseHtml(string domain,string htmlText)
        {
            if (MyStrategy.Dictionary.ContainsKey(domain)) {

                var myStrategy = MyStrategy.Dictionary[domain];
                var list = myStrategy.Strategy(htmlText);

                List<Jobs> createResult = Db.Valid(list);
                log.DebugFormat("网址{0}的查询结果为{1}", myStrategy.GetDomain(), createResult.Count);

                MailTask.Put(_queryCount - 1, createResult,sendResult => {
                    if (sendResult.Count > 0) {
                        Db.CreateUpdate(sendResult);
                        Dispatcher.Invoke(() => {
                            foreach (var job in sendResult)
                            {
                                _jobCollections.Insert(0, job);
                            }
                            tblCount.Text = _jobCollections.Count.ToString();
                        });
                    }
                });
            }
            else
            {
                log.ErrorFormat("document.domain = {0} 不存在", domain);
            }
        }

        private void BtnBlackMenu_OnClick(object sender, RoutedEventArgs e)
        {
            fyBlackMenu.IsOpen = !fyBlackMenu.IsOpen;
            rbnStop.IsChecked = true;
        }


        private void DataGridHyperlinkColumn_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (e.OriginalSource is Hyperlink && dg.SelectedItem is Jobs) {
                var row = dg.SelectedItem as Jobs;
                Process.Start(row.Url);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //定义打开的默认文件夹位置
            saveFileDialog.FileName = "检索公司历史记录_" + DateTime.Now.ToString("D");
            saveFileDialog.Filter = "Excel文件|*.xlsx"; //打开对话框显示文件筛选器

            if (saveFileDialog.ShowDialog() == true)
            {
                Excels.Export(saveFileDialog.FileName, _jobCollections);
            }
        }

    }

}
