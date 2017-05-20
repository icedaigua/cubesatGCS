using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO.Ports;


namespace HouseKeeping_Wpf
{
    /// <summary>
    /// uSerial.xaml 的交互逻辑
    /// </summary>
    public partial class uSerial : UserControl
    {

        public bool rec_show
        {
            get
            {
                return (cB_rec_show.IsChecked == true);
            }
        }

        public bool rec_show_hex
        {
            get
            {
                return (cB_rec_show_hex.IsChecked == true);
            }
        }


        public string Port_ID
        {
            get { return cB_port_id.Text; }
        }

        public uSerial()
        {
            InitializeComponent();
            serial_para_intiz();
        }


        private void serial_para_intiz()
        {
            string[] port_name = SerialPort.GetPortNames();

            if (port_name.Length == 0)
            {
                List<String> port_list = new List<String> { "无串口" };
                cB_port_id.ItemsSource = port_list;

            }
            else
            {
                Array.Sort(port_name);
                List<String> port_list = new List<String>(port_name);
                cB_port_id.ItemsSource = port_name;
                cB_port_id.SelectedIndex = cB_port_id.Items.Count > 0 ? 0 : -1;
            }
        }

        #region 显示接收
        private void cB_rec_show_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void cB_rec_show_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        #endregion
    }
}
