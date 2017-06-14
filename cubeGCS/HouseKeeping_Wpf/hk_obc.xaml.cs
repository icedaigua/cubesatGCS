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
            tB_sat_id.Text = "NJUST";

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
            
            tB_reset_cause.Text = down_info.reset_cause.ToString();

            tB_tmep_hk.Text = ((down_info.temp_hk * 2370 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2");
            //System.Diagnostics.Trace.WriteLine("ADCS" + down_info.temp_hk.ToString());

            #endregion

        

            tB_save_frame_cnt.Text = down_info.aindex.ToString();
            tB_flash_block.Text = down_info.mindex.ToString();
            tB_file_sd_time.Text = down_info.file_sd_time_latest.ToString();

        }


    }
}
