using System;
using System.Windows.Controls;

using CubeCOM;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// hk_obc_81.xaml 的交互逻辑
    /// </summary>
    public partial class hk_obc : UserControl
    {
        public hk_obc()
        {
            InitializeComponent();
        }

        public void display_obc_info(cubeCOMM.down_obc_ST down_info)
        {

            #region CPU   
            tB_sat_id.Text = "八一卫星";

            tB_reboot_count.Text = down_info.reboot_count.ToString();

            tB_rec_cmd_count.Text = down_info.rec_cmd_count.ToString();

            tB_down_count.Text = down_info.down_count.ToString();


            double seconds = down_info.last_reset_time;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(
            1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

            tB_last_reset_time.Text = dt.ToString();


            tB_work_mode.Text = down_info.work_mode.ToString();

            seconds = down_info.utc_time;
            secs = Convert.ToDouble(seconds);

            dt = new DateTime(
            1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

            tB_utc_time.Text = dt.ToString();

            tB_status_sensor_on_off.Text = down_info.status_sensor_on_off.ToString();

            tB_tmep_hk.Text = ((down_info.temp_hk * 2370 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2");
            //System.Diagnostics.Trace.WriteLine("ADCS" + down_info.temp_hk.ToString());

            #endregion

            #region 电源
            tB_sunC_1.Text = down_info.sun_c[0].ToString();
            tB_sunC_2.Text = down_info.sun_c[1].ToString();
            tB_sunC_3.Text = down_info.sun_c[2].ToString();
            tB_sunC_4.Text = down_info.sun_c[3].ToString();
            tB_sunC_5.Text = down_info.sun_c[4].ToString();
            tB_sunC_6.Text = down_info.sun_c[5].ToString();

            tB_sunV_1.Text = down_info.sun_v[0].ToString();
            tB_sunV_2.Text = down_info.sun_v[1].ToString();
            tB_sunV_3.Text = down_info.sun_v[2].ToString();
            tB_sunV_4.Text = down_info.sun_v[3].ToString();
            tB_sunV_5.Text = down_info.sun_v[4].ToString();
            tB_sunV_6.Text = down_info.sun_v[5].ToString();

            tB_BusC.Text = down_info.out_BusC.ToString();
            tB_BusV.Text = down_info.out_BusV.ToString();


            //tB_outC_1.Text = down_info.Vol_5_C[0].ToString();
            //tB_outC_2.Text = down_info.Vol_5_C[1].ToString();
            //tB_outC_3.Text = down_info.Vol_5_C[2].ToString();
            //tB_outC_4.Text = down_info.Vol_5_C[3].ToString();
            //tB_outC_5.Text = down_info.Vol_5_C[4].ToString();
            //tB_outC_6.Text = down_info.Vol_5_C[5].ToString();

            //tB_outBusC_1.Text = down_info.Bus_c_1.ToString();
            //tB_outBusC_2.Text = down_info.Bus_c_2.ToString();
            //tB_outBusC_3.Text = down_info.Bus_c_3.ToString();
            //tB_outBusC_4.Text = down_info.Bus_c_4.ToString();
            //tB_outBusC_5.Text = down_info.Bus_c_5.ToString();

            //tB_batt_tempe_1.Text = down_info.temp_batt_board_1.ToString();
            //tB_batt_tempe_2.Text = down_info.temp_batt_board_2.ToString();
            //tB_batt_tempe_3.Text = down_info.temp_eps_1.ToString();
            //tB_batt_tempe_4.Text = down_info.temp_eps_2.ToString();
            //tB_batt_tempe_5.Text = down_info.temp_eps_3.ToString();
            //tB_batt_tempe_6.Text = down_info.temp_eps_4.ToString();

            tB_batt_on_off.Text = down_info.on_off_status.ToString();

            #endregion

            tB_save_frame_cnt.Text = down_info.aindex.ToString();
            tB_flash_block.Text = down_info.mindex.ToString();

        }


    }
}
