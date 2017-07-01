using System.Windows.Controls;

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
