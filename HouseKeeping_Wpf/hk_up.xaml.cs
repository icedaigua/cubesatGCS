using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// hk_up.xaml 的交互逻辑
    /// </summary>
    public partial class hk_up : UserControl
    {

        #region 控制指令

        public bool
                 

                    mwa_open_checked = false,       //动量轮
                    mwa_close_checked = false,
                    mwb_open_checked = false,
                    mwb_close_checked = false,

            
             

                    hmra_open_checked = false,      //磁强计
                    hmra_close_checked = false,
                    hmrb_open_checked = false,
                    hmrb_close_checked = false,

                    fan_open_start_checked = false, //帆板
                    fan_open_stop_checked = false,

                    ais_open_checked = false,       //AIS
                    ais_close_checked = false,

                    batt_warm_open_checked = false,//电池阵加热
                    batt_warm_close_checked = false,

                    atenna_open_checked = false,         //天线展开
                    atenna_pwr_on_checked = false,  //天线电源
                    atenna_pwr_off_checked = false,

                    hk_reset_checked = false,       //星务重启

                    down_start_checked = false,     //开始下行
                    down_stop_checked = false,       //停止下行

                    dam_mode_checked = false,       //控制模式
                    ctrl_mode_checked = false,
                    pitch_mode_checked = false,
                    redam_checked = false,
                    always_dam_checked = false,

                    pianzhi_mode_checked = false,
                    zero_mode_checked = false,

                    close_all_checked = false,

                    bpsk_1200_checked = false,
                    bpsk_9600_checked = false,

       

                    adcs_open_checked = false,
                    adcs_close_checked = false,
                    down_fipex_checked = false,
                    error_checked = false

                    ;

        #endregion

        #region 参数注入

        public bool
                    para_time_checked = false,
                    para_P_checked = false,
                    para_Z_checked = false,
                    para_D_checked = false,
                    para_down_period_checked = false,
                    delay_hk_checked = false,
                    camera_checked = false
                    ;

        public UInt32 para_P = 0,
                     para_D = 0,
                     para_Z = 0,
                     para_down_period = 0,
                     delay_hk_orbit_cnt = 0,
                     delay_hk_select = 0,
                     delay_hk_index = 0,

                     bias_mode = 0,
            camera_delay_time = 0
                    ;

        public UInt32 para_time = 0;


        #endregion

        #region 轨道注入
        public bool orbit_modi = false;
        public double[] orbit = new double[8];


        public bool orbit_checked = false;

        #endregion


        #region 特殊指令

        public bool
                 rsh_checked = false;

        public string rsh_str = "", fipex_str = "";

        #endregion
        public hk_up()
        {
            InitializeComponent();

            cB_delay_hk_select_init();
        }


        private void cB_delay_hk_select_init()
        {

            List<String> NetNo_list = new List<String> { "SD", "RAM" };
            cB_delay_select.ItemsSource = NetNo_list;
            cB_delay_select.SelectedIndex = cB_delay_select.Items.Count > 0 ? 1 : -1;
        }

        #region 动量轮
        /// <summary>
        /// 动量轮A开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_open_Checked(object sender, RoutedEventArgs e)
        {
            mwa_open_checked = true;
            if (cB_mwa_close.IsChecked == true)
                cB_mwa_close.IsChecked = false;
        }

        /// <summary>
        /// 动量轮A开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_open_UnChecked(object sender, RoutedEventArgs e)
        {
            mwa_open_checked = false;
        }

        /// <summary>
        /// 动量轮A关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_close_Checked(object sender, RoutedEventArgs e)
        {
            mwa_close_checked = true;

            if (cB_mwa_open.IsChecked == true)
                cB_mwa_open.IsChecked = false;

        }

        /// <summary>
        /// 动量轮A关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_close_UnChecked(object sender, RoutedEventArgs e)
        {
            mwa_close_checked = false;
        }

        /// <summary>
        /// 动量轮B开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_open_Checked(object sender, RoutedEventArgs e)
        {
            mwb_open_checked = true;
            if (cB_mwb_close.IsChecked == true)
                cB_mwb_close.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_open_UnChecked(object sender, RoutedEventArgs e)
        {
            mwb_open_checked = false;
        }

        /// <summary>
        /// 动量轮B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_close_Checked(object sender, RoutedEventArgs e)
        {
            mwb_close_checked = true;

            if (cB_mwb_open.IsChecked == true)
                cB_mwb_open.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_close_UnChecked(object sender, RoutedEventArgs e)
        {
            mwb_close_checked = false;
        }

        #endregion


        #region 姿控计算机指令
        /// <summary>
        /// 动量轮d开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_open_Checked(object sender, RoutedEventArgs e)
        {
            adcs_open_checked = true;
            if (cB_adcs_close.IsChecked == true)
                cB_adcs_close.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_open_UnChecked(object sender, RoutedEventArgs e)
        {
            adcs_open_checked = false;
        }

        /// <summary>
        /// 动量轮B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_close_Checked(object sender, RoutedEventArgs e)
        {
            adcs_close_checked = true;

            if (cB_adcs_open.IsChecked == true)
                cB_adcs_open.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_close_UnChecked(object sender, RoutedEventArgs e)
        {
            adcs_close_checked = false;
        }
        #endregion


        #region 其他

        /// <summary>
        /// 动量轮B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_close_all_Checked(object sender, RoutedEventArgs e)
        {
            close_all_checked = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_close_all_UnChecked(object sender, RoutedEventArgs e)
        {
            close_all_checked = false;
        }
        #endregion

        #region 磁强计
        /// <summary>
        /// 磁强计A开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_open_Checked(object sender, RoutedEventArgs e)
        {
            hmra_open_checked = true;
            if (cB_hmra_close.IsChecked == true)
                cB_hmra_close.IsChecked = false;
        }

        /// <summary>
        /// 磁强计A开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_open_UnChecked(object sender, RoutedEventArgs e)
        {
            hmra_open_checked = false;
        }

        /// <summary>
        /// 磁强计A关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_close_Checked(object sender, RoutedEventArgs e)
        {
            hmra_close_checked = true;

            if (cB_hmra_open.IsChecked == true)
                cB_hmra_open.IsChecked = false;

        }

        /// <summary>
        /// 磁强计A关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_close_UnChecked(object sender, RoutedEventArgs e)
        {
            hmra_close_checked = false;
        }

        /// <summary>
        /// 磁强计B开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_open_Checked(object sender, RoutedEventArgs e)
        {
            hmrb_open_checked = true;
            if (cB_hmrb_close.IsChecked == true)
                cB_hmrb_close.IsChecked = false;

        }

        /// <summary>
        /// 磁强计B开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_open_UnChecked(object sender, RoutedEventArgs e)
        {
            hmrb_open_checked = false;
        }

        /// <summary>
        /// 磁强计B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_close_Checked(object sender, RoutedEventArgs e)
        {
            hmrb_close_checked = true;

            if (cB_hmrb_open.IsChecked == true)
                cB_hmrb_open.IsChecked = false;

        }

        /// <summary>
        /// 磁强计B关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_close_UnChecked(object sender, RoutedEventArgs e)
        {
            hmrb_close_checked = false;
        }
        #endregion


        #region 电池加热
        /// <summary>
        /// 电池加热开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_open_Checked(object sender, RoutedEventArgs e)
        {
            batt_warm_open_checked = true;
            if (cB_batt_warm_close.IsChecked == true)
                cB_batt_warm_close.IsChecked = false;

        }

        /// <summary>
        /// 电池加热开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_open_UnChecked(object sender, RoutedEventArgs e)
        {
            batt_warm_open_checked = false;
        }

        /// <summary>
        /// 电池加热关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_close_Checked(object sender, RoutedEventArgs e)
        {
            batt_warm_close_checked = true;

            if (cB_batt_warm_open.IsChecked == true)
                cB_batt_warm_open.IsChecked = false;

        }

        /// <summary>
        /// 电池加热关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_close_UnChecked(object sender, RoutedEventArgs e)
        {
            batt_warm_close_checked = false;
        }
        #endregion

        #region 天线电源
        /// <summary>
        /// 天线电源开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_open_Checked(object sender, RoutedEventArgs e)
        {
            atenna_pwr_on_checked = true;
            if (cB_atenna_pwr_close.IsChecked == true)
                cB_atenna_pwr_close.IsChecked = false;

        }

        /// <summary>
        /// 天线电源开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_open_UnChecked(object sender, RoutedEventArgs e)
        {
            atenna_pwr_on_checked = false;
        }

        /// <summary>
        /// 天线电源关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_close_Checked(object sender, RoutedEventArgs e)
        {
            atenna_pwr_off_checked = true;

            if (cB_atenna_pwr_open.IsChecked == true)
                cB_atenna_pwr_open.IsChecked = false;

        }

        /// <summary>
        /// 天线电源关取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_close_UnChecked(object sender, RoutedEventArgs e)
        {
            atenna_pwr_off_checked = false;
        }
        #endregion

        #region 帆板展开
        /// <summary>
        /// 帆板展开启动选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_start_Checked(object sender, RoutedEventArgs e)
        {
            fan_open_start_checked = true;
            if (cB_fan_open_stop.IsChecked == true)
                cB_fan_open_stop.IsChecked = false;
        }

        /// <summary>
        /// 帆板展开启动取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_start_UnChecked(object sender, RoutedEventArgs e)
        {
            fan_open_start_checked = false;
        }

        /// <summary>
        /// 帆板展开停止选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_stop_Checked(object sender, RoutedEventArgs e)
        {
            fan_open_stop_checked = true;
            if (cB_fan_open_start.IsChecked == true)
                cB_fan_open_start.IsChecked = false;
        }

        /// <summary>
        /// 帆板展开停止选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_stop_UnChecked(object sender, RoutedEventArgs e)
        {
            fan_open_stop_checked = false;
        }
        #endregion

        #region 天线展开
        /// <summary>
        /// 天线展开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_open_Checked(object sender, RoutedEventArgs e)
        {
            atenna_open_checked = true;
        }

        /// <summary>
        /// 天线展开取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_open_UnChecked(object sender, RoutedEventArgs e)
        {
            atenna_open_checked = false;
        }
        #endregion

        #region 星务重启
        /// <summary>
        /// 星务重启选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hk_reset_Checked(object sender, RoutedEventArgs e)
        {
            hk_reset_checked = true;
        }

        /// <summary>
        /// 星务重启取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hk_reset_UnChecked(object sender, RoutedEventArgs e)
        {
            hk_reset_checked = false;
        }
        #endregion



        #region 使能纠错
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_error_Checked(object sender, RoutedEventArgs e)
        {
            error_checked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_error_UnChecked(object sender, RoutedEventArgs e)
        {
            error_checked = false;
        }
        #endregion

        #region 下行控制
        /// <summary>
        /// 开始下行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_start_down_Checked(object sender, RoutedEventArgs e)
        {
            down_start_checked = true;
            if (cB_stop_down.IsChecked == true)
                cB_stop_down.IsChecked = false;
        }

        /// <summary>
        /// 开始下行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_start_down_UnChecked(object sender, RoutedEventArgs e)
        {
            down_start_checked = false;
        }

        /// <summary>
        /// 停止下行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_stop_down_Checked(object sender, RoutedEventArgs e)
        {
            down_stop_checked = true;
            if (cB_start_down.IsChecked == true)
                cB_start_down.IsChecked = false;
        }

        /// <summary>
        /// 停止下行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_stop_down_UnChecked(object sender, RoutedEventArgs e)
        {
            down_stop_checked = false;
        }
        #endregion

       

        #region 姿控模式
        /// <summary>
        /// 阻尼模式选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_dam_mode_Checked(object sender, RoutedEventArgs e)
        {
            dam_mode_checked = true;
        }

        /// <summary>
        /// 阻尼模式取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_dam_mode_UnChecked(object sender, RoutedEventArgs e)
        {
            dam_mode_checked = false;
        }
        /// <summary>
        /// 俯仰控制模式选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_pitch_mode_Checked(object sender, RoutedEventArgs e)
        {
            pitch_mode_checked = true;
        }

        /// <summary>
        /// 俯仰控制模式取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_pitch_mode_UnChecked(object sender, RoutedEventArgs e)
        {
            pitch_mode_checked = false;
        }

        /// <summary>
        /// 三轴稳定模式选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_ctrl_mode_Checked(object sender, RoutedEventArgs e)
        {
            ctrl_mode_checked = true;
        }

        /// <summary>
        /// 三轴稳定模式取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_ctrl_mode_UnChecked(object sender, RoutedEventArgs e)
        {
            ctrl_mode_checked = false;
        }

        /// <summary>
        /// 重阻尼模式选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_reDam_Checked(object sender, RoutedEventArgs e)
        {
            redam_checked = true;
        }

        /// <summary>
        /// 重阻尼模式取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_reDam_UnChecked(object sender, RoutedEventArgs e)
        {
            redam_checked = false;
        }

        /// <summary>
        /// 永久阻尼模式选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_always_dam_Checked(object sender, RoutedEventArgs e)
        {
            always_dam_checked = true;
        }

        /// <summary>
        /// 永久阻尼模式取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_always_dam_UnChecked(object sender, RoutedEventArgs e)
        {
            always_dam_checked = false;
        }


        /// <summary>
        /// 偏置动量模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_pianzhi_mode_Checked(object sender, RoutedEventArgs e)
        {
            pianzhi_mode_checked = true;
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_pianzhi_mode_UnChecked(object sender, RoutedEventArgs e)
        {
            pianzhi_mode_checked = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_bias_mode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_bias_mode.Text);
                bias_mode = Convert.ToUInt32(tB_bias_mode.Text);
            }
        }

        #endregion

        #region 上行参数
        /// <summary>
        /// P参数上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_P_para_Checked(object sender, RoutedEventArgs e)
        {
            para_P_checked = true;
            para_P = Convert.ToUInt32((Convert.ToDouble(tB_P_para.Text) * 1e7));
        }

        /// <summary>
        /// P参数上行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_P_para_UnChecked(object sender, RoutedEventArgs e)
        {
            para_P_checked = false;
        }


        /// <summary>
        /// P参数输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_P_para_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_P_para.Text);
                para_P = Convert.ToUInt32((Convert.ToDouble(tB_P_para.Text) * 1e7));
            }
        }

        /// <summary>
        /// Z参数上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_Z_para_Checked(object sender, RoutedEventArgs e)
        {
            para_Z_checked = true;
            para_Z = Convert.ToUInt32((Convert.ToDouble(tB_Z_para.Text) * 1e7));
        }

        /// <summary>
        /// Z参数上行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_Z_para_UnChecked(object sender, RoutedEventArgs e)
        {
            para_Z_checked = false;
        }

        /// <summary>
        /// Z参数输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_Z_para_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_Z_para.Text);
                para_Z = Convert.ToUInt32((Convert.ToDouble(tB_Z_para.Text) * 1e7));
            }
        }

        /// <summary>
        /// D参数上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_D_para_Checked(object sender, RoutedEventArgs e)
        {
            para_D_checked = true;
            para_D = Convert.ToUInt32((Convert.ToDouble(tB_D_para.Text) * 1e7));
        }

        /// <summary>
        /// D参数上行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_D_para_UnChecked(object sender, RoutedEventArgs e)
        {
            para_D_checked = false;
        }

        /// <summary>
        /// D参数输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_D_para_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_D_para.Text);
                para_D = Convert.ToUInt32((Convert.ToDouble(tB_D_para.Text) * 1e7));
            }
        }

        /// <summary>
        /// 星上时间上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_time_para_Checked(object sender, RoutedEventArgs e)
        {
            para_time_checked = true;
            para_time = Convert.ToUInt32(tB_time_para.Text);
        }

        /// <summary>
        /// 星上时间上行取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_time_para_UnChecked(object sender, RoutedEventArgs e)
        {
            para_time_checked = false;
        }

        /// <summary>
        /// 星上时间输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_time_para_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                //MessageBox.Show(tB_time_para.Text);
                tB_time_para.Text = xDateSeconds();//DateTime.Now.ToLongTimeString();
                                                   //MessageBox.Show(tB_time_para.Text);
                para_time = Convert.ToUInt32(tB_time_para.Text);
            }
        }

        /// <summary>
        /// 下行周期上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_down_period_para_Checked(object sender, RoutedEventArgs e)
        {
            para_down_period_checked = true;
            para_down_period = Convert.ToUInt32(tB_down_period.Text);
        }

        /// <summary>
        /// 下行周期取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_down_period_para_UnChecked(object sender, RoutedEventArgs e)
        {
            para_down_period_checked = false;
        }

        /// <summary>
        /// 下行周期输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_down_period_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_down_period.Text);
                para_down_period = Convert.ToUInt32(tB_down_period.Text);
            }
        }

        /// <summary>
        /// 延时遥测上行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_delay_hk_Checked(object sender, RoutedEventArgs e)
        {
            delay_hk_checked = true;

            delay_hk_index = Convert.ToUInt32(tB_delay_index.Text);

            delay_hk_orbit_cnt = Convert.ToUInt32(tB_delay_hk.Text);//(Convert.ToUInt32(xDateSeconds()) -
                                                                    //     Convert.ToUInt32(tB_delay_hk.Text)
                                                                    //   );

            delay_hk_select = Convert.ToUInt32(cB_delay_select.SelectedIndex);

        }

        /// <summary>
        /// 延时遥测取消选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_delay_hk_UnChecked(object sender, RoutedEventArgs e)
        {
            delay_hk_checked = false;
        }

        /// <summary>
        /// 延时遥测输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_delay_hk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_delay_hk.Text);

                delay_hk_orbit_cnt = Convert.ToUInt32(tB_delay_hk.Text);
                //delay_hk_orbit_cnt = (Convert.ToUInt32(xDateSeconds()) -
                //                        Convert.ToUInt32(tB_delay_hk.Text)
                //                        );//DateTime.Now.ToLongTimeString();

                MessageBox.Show(delay_hk_orbit_cnt.ToString());


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_delay_index_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {

                MessageBox.Show(tB_delay_index.Text);
                delay_hk_index = Convert.ToUInt32(tB_delay_index.Text);
            }
        }
        #endregion

        #region RSH
        /// <summary>
        /// RSH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_rsh_Checked(object sender, RoutedEventArgs e)
        {
            rsh_checked = true;

            rsh_str = tB_rsh.Text;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_rsh_UnChecked(object sender, RoutedEventArgs e)
        {
            rsh_checked = false;
        }

        private void tB_rsh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                rsh_str = tB_rsh.Text;
        }

        #endregion

        #region 延时成像
        /// <summary>
        /// RSH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_camera_Checked(object sender, RoutedEventArgs e)
        {
            camera_checked = true;

            camera_delay_time = Convert.ToUInt32(tB_camera.Text);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_camera_UnChecked(object sender, RoutedEventArgs e)
        {
            camera_checked = false;
        }

        private void tB_camera_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                camera_delay_time = Convert.ToUInt32(tB_camera.Text);
        }

        #endregion

        #region  轨道参数
        private void cB_orbit_para_Checked(object sender, RoutedEventArgs e)
        {
            orbit_checked = true;


            string PATH = Directory.GetCurrentDirectory();
            StreamReader Orbit_File;                       ///实时数据存储文件  
            Orbit_File = new StreamReader(PATH + "\\orbit.txt");

            tB_TLEJdsatepoch.Text = Orbit_File.ReadLine();
            tB_TLEBstar.Text = Orbit_File.ReadLine();
            tB_TLEcco.Text = Orbit_File.ReadLine();
            tB_TLEInclo.Text = Orbit_File.ReadLine();
            tB_TLEArgpo.Text = Orbit_File.ReadLine();
            tB_TLEMo.Text = Orbit_File.ReadLine();
            tB_TLENodeo.Text = Orbit_File.ReadLine();
            tB_TLENo.Text = Orbit_File.ReadLine();

            orbit[0] = Convert.ToDouble(tB_TLEBstar.Text);   //大气系数
            orbit[1] = Convert.ToDouble(tB_TLEcco.Text);      //偏心率
            orbit[2] = Convert.ToDouble(tB_TLEInclo.Text);      //倾角
            orbit[3] = Convert.ToDouble(tB_TLEArgpo.Text);      //近地点幅角
            orbit[4] = Convert.ToDouble(tB_TLEJdsatepoch.Text);   //儒略时间
            orbit[5] = Convert.ToDouble(tB_TLEMo.Text);    //平近点角
            orbit[6] = Convert.ToDouble(tB_TLENo.Text);    //平均运动
            orbit[7] = Convert.ToDouble(tB_TLENodeo.Text); //升交点赤经

            Orbit_File.Close();


        }

        private void cB_orbit_para_UnChecked(object sender, RoutedEventArgs e)
        {
            orbit_checked = false;
        }

        private void tB_TLEJdsatepoch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[4] = Convert.ToDouble(tB_TLEJdsatepoch.Text);   //儒略时间

        }

        private void tB_TLEBstar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[0] = Convert.ToDouble(tB_TLEBstar.Text);   //大气系数
        }

        private void tB_TLEcco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[1] = Convert.ToDouble(tB_TLEcco.Text);      //偏心率
        }

        private void tB_TLEInclo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[2] = Convert.ToDouble(tB_TLEInclo.Text);      //倾角
        }

        private void tB_TLEArgpo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[3] = Convert.ToDouble(tB_TLEArgpo.Text);      //近地点幅角
        }

        private void tB_TLEMo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[5] = Convert.ToDouble(tB_TLEMo.Text);    //平近点角
        }

        private void tB_TLENodeo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[7] = Convert.ToDouble(tB_TLENodeo.Text); //升交点赤经
        }

        private void tB_TLENo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[6] = Convert.ToDouble(tB_TLENo.Text);    //平均运动
        }
        #endregion


        private string xDateSeconds()
        {
            long xdateseconds = 0;
            DateTime xdatenow = DateTime.UtcNow;     //当前UTC时间

            long xminute = 60;      //一分种60秒
            long xhour = 3600;
            long xday = 86400;
            long byear = 1970;//从1970-1-1 0：00：00开始到现在所过的秒
            long[] xmonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            long[] xyear = { 365, 366 };
            long num = 0;
            xdateseconds += xdatenow.Second;    //算秒
            xdateseconds += xdatenow.Minute * xminute;      //算分
            xdateseconds += xdatenow.Hour * xhour;      //算时
            xdateseconds += (xdatenow.Day - 1) * xday;        //算天
            //算月(月换成天算)
            if (DateTime.IsLeapYear(xdatenow.Year))
            {
                xdateseconds += (xmonth[xdatenow.Month - 1] + 1) * xday;
            }
            else
            {
                xdateseconds += (xmonth[xdatenow.Month - 1]) * xday;
            }
            //算年（年换成天算）
            long lyear = xdatenow.Year - byear;
            for (int i = 0; i < lyear; i++)
            {
                if (DateTime.IsLeapYear((int)byear + i))
                {
                    num++;
                }
            }
            xdateseconds += ((lyear - num) * xyear[0] + num * xyear[1]) * xday;
            return xdateseconds.ToString();
        }
    }
}

