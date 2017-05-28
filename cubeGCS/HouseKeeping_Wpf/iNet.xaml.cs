using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// iNet.xaml 的交互逻辑
    /// </summary>
    public partial class iNet : UserControl
    {


        public string iNet_IP
        {
            get { return cB_IP.Text; }
        }

        public string iNet_UP_NO
        {
            get { return cB_up_No.Text; }
        }

        public string iNet_DOWN_NO
        {
            get { return cB_down_No.Text; }
        }

        public iNet()
        {
            InitializeComponent();
        }
    }
}
