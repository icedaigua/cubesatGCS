using System;
using System.Windows.Controls;
using CubeCOM;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// battery.xaml 的交互逻辑
    /// </summary>
    public partial class battery : UserControl
    {
        public battery()
        {
            InitializeComponent();
            label_battery_initz();
        }

        private void label_battery_initz()
        {

            String str = "";
            str += "                   |         |                            I(mA),   lup,   Ton(s),   Toff(s)\r\n";
            str += "              +-------------------+  1 (H1-47) --> EN: " + 0.ToString("") + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            str += "  1:          |                   |\r\n";
            str += " " + 0.ToString("D4") + " mV ->   |  Voltage          |  2 (H1-49) --> EN: " + 0.ToString() + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            str += " " + 0.ToString("D4") + " mA ->   |  " + 0.ToString("D4") + " mV          |\r\n";
            str += " " + 0.ToString("D4") + " mW ->   |                   |  3 (H1-51) --> EN: " + 0.ToString() + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";

            str += "  2:          |  Input            |\r\n";
            str += " " + 0.ToString("D4") + " mV ->   | " + 0.ToString("D4") + " mA " + 0.ToString("D4") + "   mW |  4 (H1-48) --> EN: " + 0.ToString() + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            str += " " + 0.ToString("D4") + " mA ->   |                   |\r\n";
            str += " " + 0.ToString("D4") + " mW ->   |  Output           |  5 (H1-50) --> EN: " + 0.ToString() + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";


            str += "  3:          | " + 0.ToString("D4") + " mA " + 0.ToString("D4") + "   mW |\r\n";
            str += " " + 0.ToString("D4") + " mV ->   |                   |  6 (H1-52) --> EN: " + 0.ToString() + "  " + "[ " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            str += " " + 0.ToString("D4") + " mA ->   |  Efficiency:      |\r\n";
            str += " " + 0.ToString("D4") + " mW ->   | In: " + 0.ToString("F2") + " %%       |  7         --> EN: " + 0.ToString() + "\r\n";

            str += "              |                   |\r\n";
            str += "              +-------------------+\r\n";

            tBk_batt_status.Text = str;
        }


        public void display_battery(cubeCOMM.down_obc_ST down_info)
        {


            String str = "";


            //UInt32 p_in_1 = Convert.ToUInt32(down_info.vboost_1 * down_info.curin_1 / 1000.0);
            //UInt32 p_in_2 = Convert.ToUInt32(down_info.vboost_2 * down_info.curin_2 / 1000.0);
            //UInt32 p_in_3 = Convert.ToUInt32(down_info.vboost_3 * down_info.curin_3 / 1000.0);
            //UInt32 p_sun = Convert.ToUInt32(down_info.cursun * down_info.vbatt / 1000.0);
            //UInt32 p_sys = Convert.ToUInt32(down_info.cursys * down_info.vbatt / 1000.0);
            ////UInt32 eff_in = 0;
            //UInt32 eff_in = Convert.ToUInt32((100.0 * (float)p_sun) / ((float)p_in_1 + (float)p_in_2 + (float)p_in_3));

            //Int16[] output = new Int16[7];

            //output[0] = (Int16)((down_info.status_output) & 0x0001);
            //output[1] = (Int16)(((down_info.status_output) & 0x0002) >> 1);
            //output[2] = (Int16)(((down_info.status_output) & 0x0004) >> 2);
            //output[3] = (Int16)(((down_info.status_output) & 0x0008) >> 3);
            //output[4] = (Int16)(((down_info.status_output) & 0x0010) >> 4);
            //output[5] = (Int16)(((down_info.status_output) & 0x0020) >> 5);
            //output[6] = (Int16)(((down_info.status_output) & 0x0040) >> 6);
            //// output[7] = (Int16)(((down_info.status_output) & 0x0080) >> 7);


            //str += "                   |         |                            I(mA),   lup,   Ton(s),   Toff(s)\r\n";
            //str += "              +-------------------+  1 (H1-47) --> EN: " + output[0].ToString("") + "  " + "[ " + down_info.curout_1.ToString("D4") + "    "
            //                + down_info.latchup_1.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";

            //str += "  1:          |                   |\r\n";
            //str += " " + down_info.vboost_1.ToString("D4") + " mV ->   |  Voltage          |  2 (H1-49) --> EN: " + output[1].ToString() + "  "
            //        + "[ " + down_info.curout_2.ToString("D4") + "    " + down_info.latchup_2.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";

            //str += " " + down_info.curin_1.ToString("D4") + " mA ->   |  " + down_info.vbatt.ToString("D4") + " mV          |\r\n";

            //str += " " + p_in_1.ToString("D4") + " mW ->   |                   |  3 (H1-51) --> EN: " + output[2].ToString() + "  " + "[ " + down_info.curout_3.ToString("D4")
            //            + "    " + down_info.latchup_3.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";

            //str += "  2:          |  Input            |\r\n";
            //str += " " + down_info.vboost_2.ToString("D4") + " mV ->   | " + down_info.cursun.ToString("D4") + " mA " + p_sun.ToString("D4") + "   mW |  4 (H1-48) --> EN: " + output[3].ToString() + "  "
            //            + "[ " + down_info.curout_4.ToString("D4") + "    " + down_info.latchup_4.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            //str += " " + down_info.curin_2.ToString("D4") + " mA ->   |                   |\r\n";
            //str += " " + p_in_2.ToString("D4") + " mW ->   |  Output           |  5 (H1-50) --> EN: " + output[4].ToString() + "  " + "[ "
            //            + down_info.curout_5.ToString("D4") + "    " + down_info.latchup_5.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";


            //str += "  3:          | " + down_info.cursys.ToString("D4") + " mA " + p_sys.ToString("D4") + "   mW |\r\n";
            //str += " " + down_info.vboost_3.ToString("D4") + " mV ->   |                   |  6 (H1-52) --> EN: " + output[5].ToString() + "  " + "[ "
            //            + down_info.curout_6.ToString("D4") + "    " + down_info.latchup_6.ToString("D4") + "    " + 0.ToString("D4") + "    " + 0.ToString("D4") + "]\r\n";
            //str += " " + down_info.curin_3.ToString("D4") + " mA ->   |  Efficiency:      |\r\n";
            //str += " " + p_in_3.ToString("D4") + " mW ->   | In: " + eff_in.ToString("F1") + " %%       |  7         --> EN: " + output[6].ToString() + "\r\n";

            //str += "              |                   |\r\n";
            str += "              +-------------------+\r\n";

            tBk_batt_status.Text = str;
        }




    }
}
