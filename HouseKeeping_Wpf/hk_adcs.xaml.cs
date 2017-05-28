using System;
using System.Windows.Controls;
using CubeCOM;


namespace HouseKeeping_Wpf
{
    /// <summary>
    /// hk_adcs_81.xaml 的交互逻辑
    /// </summary>
    public partial class hk_adcs : UserControl
    {
        public hk_adcs()
        {
            InitializeComponent();
        }

        public void display_adcs_info(cubeCOMM.down_adcs_ST down_info)
        {

            #region CPU
            //tB_sat_id.Text = "NJUST-2";

            tB_reboot_count.Text = (down_info.rst_cnt).ToString();
            tB_rec_cmd_count.Text = down_info.rcv_cnt.ToString();
            tB_down_count.Text = down_info.ack_cnt.ToString();

            #region 重启时间

            double seconds = down_info.rst_time;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(
            1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

            tB_last_reset_time.Text = dt.ToString();

            #endregion

            //tB_sensor_onoff.Text = down_info.status_sensor_on_off.ToString();


            #region UTC
            seconds = down_info.utc_time;
            secs = Convert.ToDouble(seconds);

            dt = new DateTime(
            1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

            tB_utc_time.Text = dt.ToString();
            #endregion


            //tB_control_mode.Text = down_info.control_mode.ToString();

            //tB_dam_cnt.Text = down_info.dam_count.ToString();
            //tB_pitch_cnt.Text = down_info.pitch_count.ToString();
            //tB_ctrl_cnt.Text = down_info.ctrl_count.ToString();


            #endregion

            #region 传感器

         

            //tB_hmr_x.Text = (down_info.hmr_x * 10.0).ToString("F1");
            //tB_hmr_y.Text = (down_info.hmr_y * 10.0).ToString("F1");
            //tB_hmr_z.Text = (down_info.hmr_z * 10.0).ToString("F1");

            //tB_momentum_a_vel.Text = down_info.momentum_vel_a.ToString();

            //tB_bar_1.Text = (down_info.bar_compute_x / 1000.0).ToString("F3");
            //tB_bar_2.Text = (down_info.bar_compute_y / 1000.0).ToString("F3");
            //tB_bar_3.Text = (down_info.bar_compute_z / 1000.0).ToString("F3");

            //tB_mag_wgs84_x.Text = (down_info.mag_wgs84_x * 10.0).ToString("F1");
            //tB_mag_wgs84_y.Text = (down_info.mag_wgs84_y * 10.0).ToString("F1");
            //tB_mag_wgs84_z.Text = (down_info.mag_wgs84_z * 10.0).ToString("F1");

            #endregion

            #region 轨道
            //tB_orbit0.Text = down_info.orbit_posi_x.ToString();
            //tB_orbit1.Text = down_info.orbit_posi_y.ToString();
            //tB_orbit2.Text = down_info.orbit_posi_z.ToString();

            //tB_orbit3.Text = down_info.orbit_velo_x.ToString();
            //tB_orbit4.Text = down_info.orbit_velo_y.ToString();
            //tB_orbit5.Text = down_info.orbit_velo_z.ToString();


            //tB_pitch_mesm.Text = (down_info.pitch_mearment / 100.0).ToString("F3");
            //tB_pitch.Text = (down_info.pitch_filter / 100.0).ToString("F3");
            //tB_pitch_rate.Text = (down_info.pitch_rate / 1000.0).ToString("F3");

            #endregion

            #region 温度

            tB_tempe_cpu.Text = ((down_info.temp_cpu / 16 * 2450 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2");
            //System.Diagnostics.Trace.WriteLine("ADCS" + down_info.temp_cpu.ToString());



            tB_tempe_1.Text =   (down_info.adc[0]   ).ToString();
            tB_tempe_2.Text =   (down_info.adc[1]  ).ToString();
            tB_tempe_3.Text =   (down_info.adc[2]  ).ToString();
            tB_tempe_4.Text =   (down_info.adc[3]  ).ToString();
            tB_tempe_5.Text =   (down_info.adc[4]  ).ToString();
            tB_tempe_6.Text =   (down_info.adc[5]  ).ToString();
            tB_tempe_7.Text =   (down_info.adc[6]  ).ToString();
            tB_tempe_8.Text =   (down_info.adc[7]  ).ToString();
            tB_tempe_9.Text =   (down_info.adc[8]  ).ToString();
            tB_tempe_10.Text =  (down_info.adc[9]  ).ToString();
     
            //tB_tempe_2.Text = ((down_info.adc_2 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_3.Text = ((down_info.adc_3 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_4.Text = ((down_info.adc_4 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_5.Text = ((down_info.adc_5 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_6.Text = ((down_info.adc_6 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_7.Text = ((down_info.adc_7 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_8.Text = ((down_info.adc_8 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_9.Text = ((down_info.adc_9 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2");
            //tB_tempe_3.Text = "未使用";
            //tB_tempe_4.Text = "未使用";
            //tB_tempe_5.Text = "未使用";
            //tB_tempe_6.Text = "未使用";
            //tB_tempe_7.Text = "未使用";
            //tB_tempe_8.Text = "未使用";
            //tB_tempe_9.Text = "未使用";


            #endregion
        }
    }
}
