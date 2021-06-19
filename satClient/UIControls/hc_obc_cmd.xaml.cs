using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UIControls
{
    /// <summary>
    /// hc_obc_cmd.xaml 的交互逻辑
    /// </summary>
    public partial class hc_obc_cmd : UserControl
    {
        public hc_obc_cmd()
        {
            InitializeComponent();

            cB_delay_hk_select_init();
        }

        private void cB_delay_hk_select_init()
        {

            List<String> NetNo_list = new List<String> { "SD", "RAM", "FLASH" };
            cB_delay_select.ItemsSource = NetNo_list;
            cB_delay_select.SelectedIndex = cB_delay_select.Items.Count > 0 ? 1 : -1;

            List<String> mode_list = new List<String> { "LOWPOWER", "NORMAL", "SLEEP" };
            cbB_obc_workmode.ItemsSource = mode_list;
            cbB_obc_workmode.SelectedIndex = cbB_obc_workmode.Items.Count > 0 ? 1 : -1;
        }

        #region 控制指令



        public bool fan_open_start_checked { get { return cB_fan_open_start.IsChecked == true; } } //帆板
        public bool fan_open_stop_checked { get { return cB_fan_open_stop.IsChecked == true; } }



        public bool batt_warm_open_checked { get { return cB_batt_warm_open.IsChecked == true; } }//电池阵加热
        public bool batt_warm_close_checked { get { return cB_batt_warm_close.IsChecked == true; } }

        public bool atenna_open_checked { get { return cB_atenna_open.IsChecked == true; } }        //天线展开
        public bool atenna_pwr_on_checked { get { return cB_atenna_pwr_open.IsChecked == true; } } //天线电源
        public bool atenna_pwr_off_checked { get { return cB_atenna_pwr_close.IsChecked == true; } }

        public bool hk_reset_checked { get { return cB_hk_reset.IsChecked == true; } }        //星务重启

        public bool down_start_checked { get { return cB_start_down.IsChecked == true; } }     //开始下行
        public bool down_stop_checked { get { return cB_stop_down.IsChecked == true; } }       //停止下行





        public bool adcs_open_checked { get { return cB_adcs_open.IsChecked == true; } }
        public bool adcs_close_checked { get { return cB_adcs_close.IsChecked == true; } }

        public bool error_checked { get { return cB_error.IsChecked == true; } }

        public bool obc_eps_open_checked { get { return cB_obc_eps_open.IsChecked == true; } }
        public bool obc_eps_close_checked { get { return cB_obc_eps_close.IsChecked == true; } }



        #endregion

        #region 参数注入


        public bool para_time_checked { get { return cB_time_para.IsChecked == true; } }

        public bool para_down_period_checked { get { return cB_down_period_para.IsChecked == true; } }
        public bool delay_hk_checked { get { return cB_delay_hk.IsChecked == true; } }


        public UInt32 para_down_period { get { return Convert.ToUInt32(tB_down_period.Text); } }
        public UInt32 delay_hk_orbit_cnt { get { return Convert.ToUInt32(tB_delay_hk.Text); } }
        public UInt32 delay_hk_select { get { return Convert.ToUInt32(cB_delay_select.SelectedIndex); } }
        public UInt32 delay_hk_index { get { return Convert.ToUInt32(tB_delay_index.Text); } }


        public UInt32 para_time { get { return Convert.ToUInt32(tB_time_para.Text); } }

        public bool obc_mode_checked { get { return cB_obc_workmode.IsChecked == true; } }
        public UInt32 obc_work_mode { get { return Convert.ToUInt32(cbB_obc_workmode.SelectedIndex); } }

        #endregion






        #region 控制指令生成
        /// <summary>
        /// 生成控制指令
        /// </summary>
        public void createCtrlCmd(byte[] up_buf, byte selectIndex, UInt32 delay_time, ref byte cmd_cnt)
        {




            //if (hk_reset_checked)                                     //星务重启
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          cubeCOMM.INS_OBC_RST,
            //                          delay_time
            //                          );

            //    cmd_cnt++;
            //}



            //if (fan_open_start_checked || fan_open_stop_checked)                                     //帆板开
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          (fan_open_start_checked) ? cubeCOMM.INS_SLBRD_ON : cubeCOMM.INS_SLBRD_OFF,
            //                          delay_time
            //                          );

            //    cmd_cnt++;
            //}

            //if (atenna_open_checked)                                     //天线开
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          cubeCOMM.INS_USB_ON,
            //                          delay_time
            //                          );

            //    cmd_cnt++;
            //}


            //if (batt_warm_open_checked || batt_warm_close_checked)                                     //电池加热
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          (batt_warm_open_checked) ? cubeCOMM.INS_SW_BATT_WARM_ON : cubeCOMM.INS_SW_BATT_WARM_OFF,
            //                          delay_time
            //                          );

            //    cmd_cnt++;
            //}

            //if (down_start_checked || down_stop_checked)                                     //星务下行
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          (down_start_checked) ? cubeCOMM.INS_DOWN_CMD_ON : cubeCOMM.INS_DOWN_CMD_OFF,
            //                          delay_time
            //                          );
            //    cmd_cnt++;
            //}

            //if (atenna_pwr_on_checked || atenna_pwr_off_checked)                                     //天线电源开关
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                          (atenna_pwr_on_checked) ? cubeCOMM.INS_S2_ON : cubeCOMM.INS_S2_OFF,
            //                          delay_time
            //                          );
            //    cmd_cnt++;
            //}


            //if (adcs_open_checked || adcs_close_checked)    //姿控计算机开关
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //                            (adcs_open_checked) ? cubeCOMM.INS_ADCS_ON : cubeCOMM.INS_ADCS_OFF,
            //                            delay_time
            //                            );

            //    cmd_cnt++;
            //}




            //if (obc_eps_open_checked || obc_eps_close_checked) //电源控制
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //        (obc_eps_open_checked) ? cubeCOMM.INS_OBC_EPS_ON : cubeCOMM.INS_OBC_EPS_OFF,
            //         delay_time);

            //    cmd_cnt++;
            //}


            //if (error_checked)
            //{
            //    cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
            //         cubeCOMM.INS_ERROR_ENABLE,
            //        delay_time);

            //    cmd_cnt++;
            //}


        }


        #endregion

        #region 参数指令生成
        /// <summary>
        /// 生成注入指令
        /// </summary>
        public void createParametersCmd(byte[] up_buf, byte selectIndex, UInt32 delay_time, ref byte cmd_cnt)
        {




            //if (para_down_period_checked) //更新下行周期
            //{
            //    cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_COM_PERIOD,
            //         delay_time,
            //         para_down_period, 0);

            //    cmd_cnt++;
            //}


            //if (delay_hk_checked) //下行延时遥测
            //{
            //    UInt32 para1 = 0, para2 = 0;

            //    para1 = delay_hk_index * Convert.ToUInt32(Math.Pow(2, 16)) + delay_hk_select;

            //    para2 = delay_hk_orbit_cnt;

            //    cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_HK_GET,
            //         delay_time,
            //         para1, para2);

            //    cmd_cnt++;
            //}


            //if (para_time_checked) //星上时间注入
            //{
            //    cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_TIME_IN,
            //         delay_time,
            //         para_time, 0);
            //    cmd_cnt++;

            //}

            //if (obc_mode_checked) //星上时间注入
            //{
            //    cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_OBC_WORKMODE,
            //         delay_time,
            //         obc_work_mode, 0);
            //    cmd_cnt++;

            //}

        }
        #endregion




        #region 姿控计算机指令
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_open_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_adcs_close.IsChecked == true)
                cB_adcs_close.IsChecked = false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_close_Checked(object sender, RoutedEventArgs e)
        {


            if (cB_adcs_open.IsChecked == true)
                cB_adcs_open.IsChecked = false;

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
            if (cB_batt_warm_close.IsChecked == true)
                cB_batt_warm_close.IsChecked = false;

        }

        /// <summary>
        /// 电池加热关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_batt_warm_open.IsChecked == true)
                cB_batt_warm_open.IsChecked = false;

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

            if (cB_atenna_pwr_close.IsChecked == true)
                cB_atenna_pwr_close.IsChecked = false;

        }


        /// <summary>
        /// 天线电源关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_atenna_pwr_open.IsChecked == true)
                cB_atenna_pwr_open.IsChecked = false;

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

            if (cB_fan_open_stop.IsChecked == true)
                cB_fan_open_stop.IsChecked = false;
        }

        /// <summary>
        /// 帆板展开停止选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_stop_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_fan_open_start.IsChecked == true)
                cB_fan_open_start.IsChecked = false;
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

            if (cB_stop_down.IsChecked == true)
                cB_stop_down.IsChecked = false;
        }

        /// <summary>
        /// 停止下行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_stop_down_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_start_down.IsChecked == true)
                cB_start_down.IsChecked = false;
        }

        #endregion


        #region 上行参数

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
            }
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

                MessageBox.Show(delay_hk_orbit_cnt.ToString());
            }
        }


        #endregion


        #region 电源控制模式
        private void cB_obc_eps_open_Checked(object sender, RoutedEventArgs e)
        {
            if (cB_obc_eps_close.IsChecked == true)
                cB_obc_eps_close.IsChecked = false;
        }

        private void cB_obc_eps_close_Checked(object sender, RoutedEventArgs e)
        {
            if (cB_obc_eps_open.IsChecked == true)
                cB_obc_eps_open.IsChecked = false;
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
