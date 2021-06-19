using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls;

namespace TSFCS.DMDS.Client.View
{
    /// <summary>
    /// DefineView.xaml 的交互逻辑
    /// </summary>
    public partial class DefineView : MetroWindow
    {
        public DefineView()
        {
            InitializeComponent();

            Messenger.Default.Register<bool>(this, "Define", HandleDefine);
            Messenger.Default.Register<string>(this, "Define", HandleDefine);
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void HandleDefine(bool isFinished)
        {
            this.Close();  //关闭窗口
        }

        private void HandleDefine(string info)
        {
            if ("Loaded".Equals(info))
            {
            }
        }


        
    }
}
