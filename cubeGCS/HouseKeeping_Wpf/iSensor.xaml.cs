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
    /// iSensor.xaml 的交互逻辑
    /// </summary>
    public partial class iSensor : UserControl
    {
        public iSensor()
        {
           
            InitializeComponent();
            iSensorInitz();
        }


        private void iSensorInitz()
        {
            sw_mw_a.SetSwitchName("动量轮A");
            sw_mw_b.SetSwitchName("动量轮B");
            sw_hmr_a.SetSwitchName("磁强计A");
            sw_hmr_b.SetSwitchName("磁强计B");
            sw_gps_a.SetSwitchName("GPSA");
            sw_gps_b.SetSwitchName("GPSB");

            sw_ants_1.SetSwitchName("天线1");
            sw_ants_2.SetSwitchName("天线2");
            sw_ants_3.SetSwitchName("天线3");
            sw_ants_4.SetSwitchName("天线4");

            sw_ants_power.SetSwitchName("天线电源");
            sw_ants_arm.SetSwitchName("天线ARM");


            sw_adcs.SetSwitchName("ADCS");

        }
    }
}
