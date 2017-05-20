using System;
using System.IO;
using CubeCOM;

namespace CubeGCS_Wpf
{
    public class IOFuctions
    {
        private DirectoryInfo File_PATH;

        private StreamWriter File_obcFrame;                       ///实时数据存储文件  
        private StreamWriter File_adcsFrame;


        private StreamWriter File_uv_Frame;

        private StreamWriter File_camera_Frame;


        private StreamWriter File_wav_Frame;

        //private StreamWriter wav_old_frame_sw;
        private FileStream wav_old_frame;
        private BinaryWriter wav_old_br;


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


        public void CreateUVFrameFile()
        {
            File_uv_Frame = new StreamWriter(File_PATH + "\\UV_Frame_HK_Dat.dat");


        }

        public void CreateCamaraFrameFile()
        {
            File_camera_Frame = new StreamWriter(File_PATH + "\\Camara_Frame_HK_Dat.dat");
        }

        public void CreateWAVFrameFile()
        {
            File_wav_Frame = new StreamWriter(File_PATH + "\\WAV_Frame_HK_Dat.dat");


        }

        public void Create_Old_wav_File()
        {

            wav_old_frame = new FileStream(File_PATH + "\\song.mid", FileMode.Create);
            wav_old_br = new BinaryWriter(wav_old_frame);
            //wav_old_frame_sw = new StreamWriter(File_PATH + "\\song.mid");
        }


        public void Write_music_File(byte[] music_buf,UInt16 length)
        {
           // string str = "";
           // str = BitConverter.A(music_buf);
           for(int kc=0;kc<length;kc++)
                wav_old_br.Write(music_buf[kc]);
        }

        public void WriteWAVFile(byte[] buffer,UInt32 length)
        {
            byte len = (byte)(length / 2);


            for(int kc = 0;kc< len; kc++)
            {
                File_wav_Frame.Write(buffer[2*kc]);
                File_wav_Frame.Write('\t');

                File_wav_Frame.Write('\t');


                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)buffer[2 * kc+1] };
                string strCharacter = asciiEncoding.GetString(byteArray);
               

                File_wav_Frame.WriteLine(strCharacter);
            }
                
        }


        private void WriteObcFrameFirstLine()
        {
            string str = "";


            str =
            #region CPU
                     "Rows" + '\t'
                    + "卫星ID" + '\t'
                    + "Reboot Count" + '\t'
                    + "Up Cnt" + '\t'
                    + "Down Cnt" + '\t'
                    + "Reboot Time" + '\t'
                    + "WorkMode" + '\t'
                    + "UTC Time" + '\t'
                    //       + "Sensor" + '\t'
                    + "tempe_hk" + '\t'

            #endregion


            #region 电源
                    + "电池温度1" + '\t'
                    + "电池温度2" + '\t'
                    + "电源板温度1" + '\t'
                    + "电源板温度2" + '\t'
                    + "电源板温度3" + '\t'
                    + "电源板温度4" + '\t'


                    + "充电电流1" + '\t'
                    + "充电电流2" + '\t'
                    + "充电电流3" + '\t'
                    + "充电电流4" + '\t'
                    + "充电电流5" + '\t'
                    + "充电电流6" + '\t'

                    + "充电电压1" + '\t'
                    + "充电电压2" + '\t'
                    + "充电电压3" + '\t'
                    + "充电电压4" + '\t'
                    + "充电电压5" + '\t'
                    + "充电电压6" + '\t'

                    + "母线电流" + '\t'
                    + "母线电压" + '\t'
                    + "UV电流" + '\t'
                    + "5V_3.3V电流1" + '\t'
                    + "5V_3.3V电流2" + '\t'
                    + "5V_3.3V电流3" + '\t'
                    + "5V_3.3V电流4" + '\t'
                    + "5V_3.3V电流5" + '\t'
                    + "5V_3.3V电流6" + '\t'

                    + "母线电流1" + '\t'
                    + "母线电流2" + '\t'
                    + "母线电流3" + '\t'
                    + "母线电流4" + '\t'
                    + "母线电流5" + '\t'
                    + "开关状态" + '\t'

            #endregion


                    + "flash num" + '\t'
                    + "frame index" + '\t'
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
                    + down_info.rec_cmd_count.ToString() + '\t'
                    + down_info.down_count.ToString() + '\t'
                    + down_info.last_reset_time.ToString() + '\t'
                    + down_info.work_mode.ToString() + '\t'
                    + down_info.utc_time.ToString() + '\t'
                    //    + down_info.status_sensor_on_off.ToString() + '\t'

                    + ((down_info.temp_hk * 2370 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2") + '\t'
            #endregion

      
            #region 电源
                    //+ down_info.temp_batt_board_1.ToString() + '\t'
                    //+ down_info.temp_batt_board_2.ToString() + '\t'
                    //+ down_info.temp_eps_1.ToString() + '\t'
                    //+ down_info.temp_eps_2.ToString() + '\t'
                    //+ down_info.temp_eps_3.ToString() + '\t'
                    //+ down_info.temp_eps_4.ToString() + '\t'


                    //+ down_info.sun_c_1.ToString() + '\t'
                    //+ down_info.sun_c_2.ToString() + '\t'
                    //+ down_info.sun_c_3.ToString() + '\t'
                    //+ down_info.sun_c_4.ToString() + '\t'
                    //+ down_info.sun_c_5.ToString() + '\t'
                    //+ down_info.sun_c_6.ToString() + '\t'

                    //+ down_info.sun_v_1.ToString() + '\t'
                    //+ down_info.sun_v_2.ToString() + '\t'
                    //+ down_info.sun_v_3.ToString() + '\t'
                    //+ down_info.sun_v_4.ToString() + '\t'
                    //+ down_info.sun_v_5.ToString() + '\t'
                    //+ down_info.sun_v_6.ToString() + '\t'


                    + down_info.out_BusC.ToString() + '\t'
                    + down_info.out_BusV.ToString() + '\t'
                    + down_info.UV_board_C.ToString() + '\t'



                    //+ down_info.Vol_5_C_1.ToString() + '\t'
                    //+ down_info.Vol_5_C_2.ToString() + '\t'
                    //+ down_info.Vol_5_C_3.ToString() + '\t'
                    //+ down_info.Vol_5_C_4.ToString() + '\t'
                    //+ down_info.Vol_5_C_5.ToString() + '\t'
                    //+ down_info.Vol_5_C_6.ToString() + '\t'
                    //+ down_info.Bus_c_1.ToString() + '\t'
                    //+ down_info.Bus_c_2.ToString() + '\t'
                    //+ down_info.Bus_c_3.ToString() + '\t'
                    //+ down_info.Bus_c_4.ToString() + '\t'
                    //+ down_info.Bus_c_5.ToString() + '\t'
                    + down_info.on_off_status.ToString() + '\t'

            #endregion

       
                    //+ down_info.index.ToString() + '\t'
                    //+ down_info.num.ToString() + '\t'

                    ;


            File_obcFrame.WriteLine(str);
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
                    + "动量轮A" + '\t'
                    
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
                //+ (down_info.reboot_count-28045).ToString() + '\t'
                //+ down_info.rec_cmd_count.ToString() + '\t'
                //+ down_info.response_count.ToString() + '\t'

                //+ down_info.last_reset_time.ToString() + '\t'
                ////          + down_info.status_sensor_on_off.ToString() + '\t'
                //+ down_info.utc_time.ToString() + '\t'
                //+ down_info.control_mode.ToString() + '\t'

                //+ down_info.dam_count.ToString() + '\t'
                //+ down_info.pitch_count.ToString() + '\t'
                //+ down_info.ctrl_count.ToString() + '\t'
               // + down_info.zero_count.ToString() + '\t'
            #endregion

            #region 传感器

                //+ (down_info.hmr_x * 10.0).ToString("F1") + '\t'
                //+ (down_info.hmr_y * 10.0).ToString("F1") + '\t'
                //+ (down_info.hmr_z * 10.0).ToString("F1") + '\t'
                //+ (down_info.mag_wgs84_x * 10.0).ToString("F1") + '\t'
                //+ (down_info.mag_wgs84_y * 10.0).ToString("F1") + '\t'
                //+ (down_info.mag_wgs84_z * 10.0).ToString("F1") + '\t'

                //+ down_info.momentum_vel_a.ToString() + '\t'


                //+ (down_info.bar_compute_x / 1000.0).ToString("F3") + '\t'
                //+ (down_info.bar_compute_y / 1000.0).ToString("F3") + '\t'
                //+ (down_info.bar_compute_z / 1000.0).ToString("F3") + '\t'


            #endregion

            #region 滤波器

                //+ (down_info.pitch_mearment / 100.0).ToString("F3") + '\t'
                //+ (down_info.pitch_filter / 100.0).ToString("F3") + '\t'
                //+ (down_info.pitch_rate / 1000.0).ToString("F3") + '\t'


         
            #endregion


            #region 轨道

                //+ down_info.orbit_posi_x.ToString() + '\t'
                //+ down_info.orbit_posi_y.ToString() + '\t'
                //+ down_info.orbit_posi_z.ToString() + '\t'

                //+ down_info.orbit_velo_x.ToString() + '\t'
                //+ down_info.orbit_velo_y.ToString() + '\t'
                //+ down_info.orbit_velo_z.ToString() + '\t'
            #endregion



            #region 温度

                + ((down_info.temp_cpu / 16 * 2450 / 4096.0 - 760.0) / 2.5 + 25).ToString("F2") + '\t'
                //+ (down_info.gyro_temp * 0.07386 + 31.0).ToString("F2") + '\t'

                //+ ((down_info.adc_1 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_2 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_3 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_4 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_5 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_6 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_7 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_8 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_9 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'
                //+ ((down_info.adc_10 / 4096.0 * 5.0 - 0.273) * 1000.0).ToString("F2") + '\t'

            #endregion

                + '\t';
            File_adcsFrame.WriteLine(str);

        }


        public void WriteCamaraFrameFile(byte[] camara_buf, int length)
        {

            string s = "";

            for (int kc = 0; kc < length; kc++)
            {
                s += camara_buf[kc].ToString("X2") + " ";
            }
            File_camera_Frame.WriteLine(s);

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

            if (File_camera_Frame != null)
                File_camera_Frame.Close();

            if (File_uv_Frame != null)
                File_uv_Frame.Close();

            if (File_wav_Frame != null)
                File_wav_Frame.Close();

            //if (wav_old_frame_sw != null)
            //    wav_old_frame_sw.Close();
        }
    }
}

