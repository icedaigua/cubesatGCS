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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TSFCS.DMDS.Client.View
{
    /// <summary>
    /// VoltView.xaml 的交互逻辑
    /// </summary>
    public partial class VoltView : Page
    {
        public VoltView()
        {
            InitializeComponent();
            this.DataContext = new TSFCS.DMDS.Client.ViewModel.VoltViewModel();
        }
    }
}
