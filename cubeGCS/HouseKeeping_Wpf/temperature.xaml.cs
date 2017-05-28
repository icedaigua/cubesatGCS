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
using CubeCOM;


namespace HouseKeeping_Wpf
{
    /// <summary>
    /// temperature.xaml 的交互逻辑
    /// </summary>
    public partial class temperature : UserControl
    {
        public temperature()
        {
            InitializeComponent();
        }


        public void displayTemperature(cubeCOMM.down_obc_ST obcInfo)
        {

            //tBk_tempe_ax100.Text = obcInfo.temp_batt_board_2.ToString("F2");
            //tBk_tempe_battery.Text = obcInfo.temp_batt_board_1.ToString("F2");
            tBk_tempe_hk.Text = ((obcInfo.temp_hk * 2370 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2");

        }
    }
}
