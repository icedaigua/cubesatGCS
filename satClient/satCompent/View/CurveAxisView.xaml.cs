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
    /// CurveAxisView.xaml 的交互逻辑
    /// </summary>
    public partial class CurveAxisView : MetroWindow
    {
        public CurveAxisView(int curve)
        {
            InitializeComponent();

            this.DataContext = new TSFCS.DMDS.Client.ViewModel.CurveAxisViewModel(curve);

            Messenger.Default.Register<bool>(this, "Curve", HandleCurveAxis);

            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void HandleCurveAxis(bool isFinished)
        {
            this.Close();  //关闭窗口
        }
    }
}
