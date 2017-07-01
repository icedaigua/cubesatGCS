using System;
using System.Windows.Controls;

using CubeCOM;
using System.Collections.Generic;
using System.Diagnostics;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// hk_obc_81.xaml 的交互逻辑
    /// </summary>
    public partial class hk_obc : UserControl
    {

        private List<String> wordmode = new List<String> { "0:低功耗模式","1:正常模式","2:休眠模式"},
                             
                            reset_cause = new List<string>
                             {
                                 "0:CPU_RESET_NONE",
                                 "1:CPU_RESET_USER",
                                 "2:CPU_RESET_STACK_OVERFLOW",
                                 "3:CPU_RESET_HardFault",
                                 "4:CPU_RESET_MemManage",
                                 "5:CPU_RESET_BusFault",
                                 "6:CPU_RESET_UsageFault",
                                 "7;CPU_RESET_DebugMon",
                                 "8:CPU_RESET_NMI",
                                 "9:CPU_RESET_NRST"
                             },
                            sd_card_error = new List<string>
                            {
                                "FR_OK",              
                                "FR_DISK_ERR",            
                                "FR_INT_ERR",             
                                "FR_NOT_READY",           
                                "FR_NO_FILE",             
                                "FR_NO_PATH",             
                                "FR_INVALID_NAME",        
                                "FR_DENIED",              
                                "FR_EXIST",               
                                "FR_INVALID_OBJECT",      
                                "FR_WRITE_PROTECTED",     
                                "FR_INVALID_DRIVE",       
                                "FR_NOT_ENABLED",         
                                "FR_NO_FILESYSTEM",       
                                "FR_MKFS_ABORTED",        
                                "FR_TIMEOUT",             
                                "FR_LOCKED",              
                                "FR_NOT_ENOUGH_CORE",     
                                "FR_TOO_MANY_OPEN_FILES", 
                                "FR_INVALID_PARAMETER"                                         
                            };

        public hk_obc()
        {
            InitializeComponent();
        }

        public void display_obc_info(cubeCOMM.down_obc_ST down_info)
        {

            try
            {
                #region CPU   
                tB_sat_id.Text = "NJUST";
                tB_reboot_count.Text = down_info.reboot_count.ToString();

                tB_rec_cmd_count.Text = down_info.rec_cmd_count.ToString();


                tB_rec_cmd_ID.Text = getCmdID(down_info.rec_cmd_ID);
                tB_rec_cmd_status.Text = down_info.cmd_proc_status.ToString();

                tB_down_count.Text = down_info.down_count.ToString();


                double seconds = down_info.last_reset_time;

                double secs = Convert.ToDouble(seconds);

                DateTime dt = new DateTime(
                1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

                tB_last_reset_time.Text = dt.ToString();


                tB_work_mode.Text = wordmode[down_info.work_mode];

                seconds = down_info.utc_time;
                secs = Convert.ToDouble(seconds);

                dt = new DateTime(
                1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secs);

                tB_utc_time.Text = dt.ToString();

                tB_reset_cause.Text = reset_cause[down_info.reset_cause];

                tB_tmep_hk.Text = ((down_info.temp_hk * 2030 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2");
                #endregion


                #region 文件系统

                tB_flash_index.Text = down_info.flash_index.ToString();
                tB_flash_block.Text = down_info.flash_block.ToString();


                tB_sram_index.Text = down_info.sram_index.ToString();
                tB_sram_block.Text = down_info.sram_block.ToString();

                tB_file_sd_time.Text = down_info.file_sd_time_latest.ToString();
                tB_sd_status.Text = sd_card_error[down_info.sd_card_status];
                tB_sd_saved_cnt.Text = down_info.sd_saved_cnt.ToString();


                tB_camera_time.Text = down_info.camera_time_latest.ToString();
                tB_camera_saved_cnt.Text = down_info.camera_saved_cnt.ToString();

                #endregion
                tB_obc_control_eps.Text = (((down_info.on_off_status & 0x80) > 0) ? "1:开" : "0:关").ToString();

         
            }
            catch(Exception e)
            {
                Trace.TraceError("星务下行数据处理错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("星务显示错误" + e.Message);
            }
        }


        private string getCmdID(byte ID)
        {
            switch (ID)
            {

                case cubeCOMM.INS_APP_HK_GET:
                    return "0x" + ID.ToString("X") + ":" + "下行载荷数据";
                case cubeCOMM.INS_HK_GET:
                    return "0x" + ID.ToString("X") + ":" + "下行延时遥测";


                case cubeCOMM.INS_DOWN_CMD_ON:
                    return "0x" + ID.ToString("X") + ":" + "实时遥测";
                case cubeCOMM.INS_APP_STR_DOWN:
                    return "0x" + ID.ToString("X") + ":" + "下行载荷数据";


                case cubeCOMM.INS_OBC_RST:
                    return "0x" + ID.ToString("X") + ":" + "星务重启";
                case cubeCOMM.INS_DOWN_CMD_OFF:
                    return "0x" + ID.ToString("X") + ":" + "空指令";

                case cubeCOMM.INS_OBC_EPS_ON:
                    return "0x" + ID.ToString("X") + ":" + "EPS自动控制开";
                case cubeCOMM.INS_OBC_EPS_OFF:
                    return "0x" + ID.ToString("X") + ":" + "EPS自动控制关";
                case cubeCOMM.INS_OBC_WORKMODE:
                    return "0x" + ID.ToString("X") + ":" + "星务模式设置";


                //开关指令

                case cubeCOMM.INS_ADCS_ON:
                    return "0x" + ID.ToString("X") + ":" + "ADCS开机";
                case cubeCOMM.INS_ADCS_OFF:
                    return "0x" + ID.ToString("X") + ":" + "ADCS关机";

                case cubeCOMM.INS_SLBRD_ON:
                    return "0x" + ID.ToString("X") + ":" + "启动展帆板"; 
                case cubeCOMM.INS_SLBRD_OFF:
                    return "0x" + ID.ToString("X") + ":" + "关闭展帆板";
                case cubeCOMM.INS_USB_ON:
                    return "0x" + ID.ToString("X") + ":" + "启动展天线";





                case cubeCOMM.INS_S2_ON:
                    return "0x" + ID.ToString("X") + ":" + "天线电源开";
                case cubeCOMM.INS_S2_OFF:
                    return "0x" + ID.ToString("X") + ":" + "天线电源关";


                case cubeCOMM.INS_SW_BATT_WARM_ON:
                    return "0x" + ID.ToString("X") + ":" + "电源加热开";  //
                case cubeCOMM.INS_SW_BATT_WARM_OFF:
                    return "0x" + ID.ToString("X") + ":" + "电源加热关";  //


                case cubeCOMM.INS_COM_PERIOD:
                    return "0x" + ID.ToString("X") + ":" + "下行周期更新";//


                case cubeCOMM.INS_TIME_IN:
                    return "0x"+ID.ToString("X") + ":" + "时间注入"; //
                default:
                    return "0x" + ID.ToString("X") + ":" + "无此指令"; //


            }
        }


    }
}
