using System;
using System.IO;
using CubeCOM;
using System.Diagnostics;

namespace CubeGCS_Wpf
{
    public class IOFuctions
    {
        private DirectoryInfo File_PATH;

        private StreamWriter File_obcFrame;                       ///实时数据存储文件  
        private StreamWriter File_adcsFrame;

        private UInt64 obcFrame_save_cnt = 0;
        private UInt64 adcsFrame_save_cnt = 0;


        /// <summary>
        /// 创建保存数据文件
        /// </summary>
        public void CreateOBCFrameFile()
        {
            string PATH = Directory.GetCurrentDirectory();
            string time_now = DateTime.Now.ToString("yyyy-MM-dd") +
                    '(' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ')';
            File_PATH = new DirectoryInfo(PATH + "\\遥测数据存储\\HK_DAT'" + time_now);
            File_PATH.Create();


            File_obcFrame = new StreamWriter(File_PATH + "\\OBCFrame_HK_Dat.dat");

            WriteObcFrameFirstLine();
        }

        public void CreateADCSFrameFile()
        {
            File_adcsFrame = new StreamWriter(File_PATH + "\\ADCSFrame_HK_Dat.dat");

            WriteAdcsFrameFirstLine();
        }





        private void WriteObcFrameFirstLine()
        {
            string str = "";


            str =
            #region CPU
                     "Rows" + '\t'
                    + "SatID" + '\t'
                    + "RebootCount" + '\t'
                    + "ResetCause" + '\t'
                    + "UpCmdCnt" + '\t'
                    + "DownCnt" + '\t'
                    + "RebootTime" + '\t'
                    + "WorkMode" + '\t'
                    + "UTCTime" + '\t'
                    + "tempe_hk" + '\t'
                    + "sensorOnOff" + '\t'

                    + "flash block" + '\t'
                    + "frame index" + '\t'

                    + "sram block" + '\t'
                    + "sram index" + '\t'

                    + "sdtime" + '\t'
                    + "sdstatus" + '\t'
                    + "sdsavecnt" + '\t'

                    + "cameratime" + '\t'
                    + "cameracnt" + '\t'

            #endregion


            #region 电源
                    + "temp_batt_board1" + '\t'
                    + "temp_batt_board2" + '\t'
                    + "temp_eps_1" + '\t'
                    + "temp_eps_2" + '\t'
                    + "temp_eps_3" + '\t'
                    + "temp_eps_4" + '\t'


                    + "sun_c_1" + '\t'
                    + "sun_c_2" + '\t'
                    + "sun_c_3" + '\t'
                    + "sun_c_4" + '\t'
                    + "sun_c_5" + '\t'
                    + "sun_c_6" + '\t'

                    + "sun_v_1" + '\t'
                    + "sun_v_2" + '\t'
                    + "sun_v_3" + '\t'
                    + "sun_v_4" + '\t'
                    + "sun_v_5" + '\t'
                    + "sun_v_6" + '\t'

                    + "out_BusC" + '\t'
                    + "out_BusV" + '\t'
                    + "UV_board_C" + '\t'
                    + "Vol_5_C_1" + '\t'
                    + "Vol_5_C_2" + '\t'
                    + "Vol_5_C_3" + '\t'
                    + "Vol_5_C_4" + '\t'
                    + "Vol_5_C_5" + '\t'
                    + "Vol_5_C_6" + '\t'

                    + "Bus_c_1" + '\t'
                    + "Bus_c_2" + '\t'
                    + "Bus_c_3" + '\t'
                    + "Bus_c_4" + '\t'
                    + "Bus_c_5" + '\t'
                    + "eps_switch" + '\t'


            #endregion
            ;


            File_obcFrame.WriteLine(str);


        }


        /// <summary>
        /// 数据流写入文件
        /// </summary>
        /// <param name="buffer">数据流</param>
        /// <param name="bytes">数据流长度</param>
        public void WriteObcFrameFile(cubeCOMM.down_obc_ST down_info)
        {
            string str = "";

            obcFrame_save_cnt++;

            str =

            #region CPU
                    obcFrame_save_cnt.ToString() + '\t'
                    + down_info.sat_id.ToString() + '\t'
                    + down_info.reboot_count.ToString() + '\t'
                    + down_info.reset_cause.ToString() + '\t'
                    + down_info.rec_cmd_count.ToString() + '\t'
                    + down_info.down_count.ToString() + '\t'
                    + down_info.last_reset_time.ToString() + '\t'
                    + down_info.work_mode.ToString() + '\t'
                    + down_info.utc_time.ToString() + '\t'

                    + ((down_info.temp_hk * 2030 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2") + '\t'
                    + down_info.on_off_status.ToString() + '\t'


                    + down_info.flash_block.ToString()+ '\t'
                    + down_info.flash_index.ToString() + '\t'

                    + down_info.sram_block.ToString() + '\t'
                    + down_info.sram_index.ToString() + '\t'


                    + down_info.file_sd_time_latest.ToString() + '\t'
                    + down_info.sd_card_status.ToString() + '\t'
                    + down_info.sd_saved_cnt.ToString() + '\t'

                    + down_info.camera_time_latest.ToString() + '\t'
                    + down_info.camera_saved_cnt.ToString() + '\t'
            #endregion


            #region 电源
                + down_info.temp_batt_board[0].ToString() + '\t'
                + down_info.temp_batt_board[1].ToString() + '\t'
                + down_info.temp_eps[0].ToString() + '\t'
                + down_info.temp_eps[1].ToString() + '\t'
                + down_info.temp_eps[2].ToString() + '\t'
                + down_info.temp_eps[3].ToString() + '\t'


                + down_info.sun_c[0].ToString() + '\t'
                + down_info.sun_c[1].ToString() + '\t'
                + down_info.sun_c[2].ToString() + '\t'
                + down_info.sun_c[3].ToString() + '\t'
                + down_info.sun_c[4].ToString() + '\t'
                + down_info.sun_c[5].ToString() + '\t'

                + down_info.sun_v[0].ToString() + '\t'
                + down_info.sun_v[1].ToString() + '\t'
                + down_info.sun_v[2].ToString() + '\t'
                + down_info.sun_v[3].ToString() + '\t'
                + down_info.sun_v[4].ToString() + '\t'
                + down_info.sun_v[5].ToString() + '\t'


                + down_info.out_BusC.ToString() + '\t'
                + down_info.out_BusV.ToString() + '\t'
                + down_info.UV_board_C.ToString() + '\t'



                + down_info.Vol_5_C[0].ToString() + '\t'
                + down_info.Vol_5_C[1].ToString() + '\t'
                + down_info.Vol_5_C[2].ToString() + '\t'
                + down_info.Vol_5_C[3].ToString() + '\t'
                + down_info.Vol_5_C[4].ToString() + '\t'
                + down_info.Vol_5_C[5].ToString() + '\t'
                + down_info.Bus_c[0].ToString() + '\t'
                + down_info.Bus_c[1].ToString() + '\t'
                + down_info.Bus_c[2].ToString() + '\t'
                + down_info.Bus_c[3].ToString() + '\t'
                + down_info.Bus_c[4].ToString() + '\t'
                + down_info.eps_switch_status.ToString() + '\t'

            #endregion

            ;

            try
            {
                File_obcFrame.WriteLine(str);
            }
            catch (Exception e)
            {
                Trace.TraceError("OBC文件IO错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("OBC文件IO错误:" + e.Message);
                File_obcFrame.Close();
            }
    
        }



        public void WriteAdcsFrameFirstLine()
        {
            string str = "";


            str =
            #region CPU
                    "Row" + '\t'
                    + "reboot cnt" + '\t'
                    + "rec_cmd_count" + '\t'
                    + "response_count" + '\t'

                    + "last_reset_time" + '\t'
                    //                   + "status_sensor_on_off" + '\t'
                    + "utc time" + '\t'
                    + "control mode" + '\t'


                    + "dam_count" + '\t'
                    + "pitch_count" + '\t'
                    + "ctrl_count" + '\t'
                 //   + "zero_count" + '\t'
            #endregion

            #region 传感器
                    + "hmr_x" + '\t'
                    + "hmr_y" + '\t'
                    + "hmr_z" + '\t'
                    + "hmr_x_wgs84" + '\t'
                    + "hmr_y_wgs84" + '\t'
                    + "hmr_z_wgs84" + '\t'
                    + "Moment" + '\t'
                    
                    + "Bar compute X" + '\t'
                    + "Bar compute Y" + '\t'
                    + "Bar compute Z" + '\t'

            #endregion

            #region 滤波器

                    + "pitch_mearment" + '\t'
                    + "pitch_filter" + '\t'
                    + "pitch_rate" + '\t'

                

            #endregion

            #region 轨道

                    + "Orbit Posi X" + '\t'
                    + "Orbit Posi Y" + '\t'
                    + "Orbit Posi Z" + '\t'
                    + "Orbit Velo X" + '\t'
                    + "Orbit Velo Y" + '\t'
                    + "Orbit Velo Z" + '\t'

            #endregion

            #region 温度
                    + "temp cpu" + '\t'
               
                    + "Tempe 1" + '\t'
                    + "Tempe 2" + '\t'
                    + "Tempe 3" + '\t'
                    + "Tempe 4" + '\t'
                    + "Tempe 5" + '\t'
                    + "Tempe 6" + '\t'
                    + "Tempe 7" + '\t'
                    + "Tempe 8" + '\t'
                    + "Tempe 9" + '\t'
                    + "Tempe 10" + '\t'
            #endregion

                    + '\t';

            File_adcsFrame.WriteLine(str);
        }

        public void WriteAdcsFrameFile(cubeCOMM.down_adcs_ST down_info)
        {
            string str = "";

            adcsFrame_save_cnt++;

            str =
            #region CPU
                adcsFrame_save_cnt.ToString() + '\t'
                + (down_info.rst_cnt).ToString() + '\t'
                + down_info.rcv_cnt.ToString() + '\t'
                + down_info.ack_cnt.ToString() + '\t'

                + down_info.rst_time.ToString() + '\t'
                //          + down_info.status_sensor_on_off.ToString() + '\t'
                + down_info.utc_time.ToString() + '\t'
                + down_info.adcs_ctrl_mode.ToString() + '\t'

                + down_info.downAdcscntDmp.ToString() + '\t'
                + down_info.downAdcscntPitcom.ToString() + '\t'
                + down_info.downAdcscntAttSta.ToString() + '\t'
                //+ down_info.zero_count.ToString() + '\t'
            #endregion

            #region 传感器

                + (down_info.downAdcsMagnetometer[0] * 10.0).ToString("F1") + '\t'
                + (down_info.downAdcsMagnetometer[1] * 10.0).ToString("F1") + '\t'
                + (down_info.downAdcsMagnetometer[2] * 10.0).ToString("F1") + '\t'
                + (down_info.downAdcsMagInO[0] * 10.0).ToString("F1") + '\t'
                + (down_info.downAdcsMagInO[1] * 10.0).ToString("F1") + '\t'
                + (down_info.downAdcsMagInO[2] * 10.0).ToString("F1") + '\t'

                + down_info.downAdcsWheelSpeed_Meas.ToString() + '\t'


                + (down_info.downAdcsMTQOut[0] / 1000.0).ToString("F3") + '\t'
                + (down_info.downAdcsMTQOut[1] / 1000.0).ToString("F3") + '\t'
                + (down_info.downAdcsMTQOut[2] / 1000.0).ToString("F3") + '\t'


            #endregion

            #region 滤波器

                + (down_info.downAdcsPitAngle / 100.0).ToString("F3") + '\t'
                + (down_info.downAdcsPitFltState[0] / 100.0).ToString("F3") + '\t'
                + (down_info.downAdcsPitFltState[1] / 1000.0).ToString("F3") + '\t'



            #endregion


            #region 轨道

                + down_info.downAdcsOrbPos[0].ToString() + '\t'
                + down_info.downAdcsOrbPos[1].ToString() + '\t'
                + down_info.downAdcsOrbPos[2].ToString() + '\t'

                + down_info.downAdcsOrbVel[0].ToString() + '\t'
                + down_info.downAdcsOrbVel[1].ToString() + '\t'
                + down_info.downAdcsOrbVel[2].ToString() + '\t'
            #endregion



            #region 温度

                + ((down_info.temp_cpu / 16 * 2450 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2") + '\t'
                //+ (down_info.gyro_temp * 0.07386 + 31.0).ToString("F2") + '\t'

                + ((down_info.adc[0]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[1]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[2]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[3]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[4]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[5]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[6]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[7]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[8]/ 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                + ((down_info.adc[9] / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'

            #endregion

                + '\t';

            try
            {
                File_adcsFrame.WriteLine(str);
            }
            catch (Exception e)
            {
                Trace.TraceError("ADCS文件IO错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("ADCS文件IO错误:" + e.Message);
                File_adcsFrame.Close();
            }

        }




        /// <summary>
        /// 关闭保存数据文件
        /// </summary>
        public void CloseFile()
        {
            if (File_obcFrame != null)
                File_obcFrame.Close();

            if (File_adcsFrame != null)
                File_adcsFrame.Close();

        }
    }
}
