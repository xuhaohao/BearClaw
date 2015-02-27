using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BearClaw.Common;
using BearClaw.Models;

namespace BearClaw
{
    /// <summary>
    /// BlackMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class BlackMenuView : UserControl
    {
        public BlackMenuView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                App.Current.Exit += (s, e) =>
                {
                    var strContent = Methods.Serialize(CollectionData);
                    File.WriteAllText(StrPath, strContent);
                };

                dgMain.ItemsSource = CollectionData;
            }
        }

        public void InitData()
        {
            if (File.Exists(StrPath))
            {
                var strContent = File.ReadAllText(StrPath);
                if (!string.IsNullOrEmpty(strContent))
                {
                    var list = Methods.Desrialize<List<JobEntity>>(strContent);
                    if (list != null)
                    {
                        foreach (var jobEntity in list)
                        {
                            CollectionData.Add(jobEntity);
                        }
                    }
                }
            }
        }


        private static readonly string StrPath = string.Format(@"{0}setting.data", AppDomain.CurrentDomain.BaseDirectory);

        public ObservableCollection<JobEntity> CollectionData = new ObservableCollection<JobEntity>();

    }
}
