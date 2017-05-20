﻿using System.Windows;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;


using CubeCOM;
using Dongzr.MidiLite;  //for MMtimers
using System.Text;
using System.Windows.Threading;

namespace CubeGCS_Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {



        #region 链路协议参数


        private cubeCOMM.down_obc_ST obc_info = new cubeCOMM.down_obc_ST();
        private cubeCOMM.down_adcs_ST adcs_info = new cubeCOMM.down_adcs_ST();

        private UInt32 rec_down_info_count = 0;

        private byte[] up_buf = new byte[200];


        private byte[] down_info_buf = new byte[512];
        byte down_info_buf_length = 0;

        private byte rec_state = 0;

        #endregion

        private IOFuctions io_func = new IOFuctions();

      

        #region 主窗口相关及控件初始化
        /// <summary>
        /// 主窗口初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            get_local_time();   //本机时间初始化

            cB_pid_intiz();
        }
        /// <summary>
        /// 主窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            io_func.CloseFile();
            local_time_timer.Stop();
        }


        /// <summary>
        /// 目的ID 下拉菜单初始化
        /// </summary>
        private void cB_pid_intiz()
        {

  
            string[] Pid_list = new string[3] { "请选择目的地", "星务计算机", "姿控计算机" };

  
            cB_pid_81.SelectedIndex = 1;

            cB_pid_81.ItemsSource = Pid_list;
            cB_pid_81.SelectedIndex = cB_pid_81.Items.Count > 0 ? 1 : -1;

        }


        #endregion


        #region 定时上传下行指令


        private UInt32 down_auto_cnt = 0;
        Thread down_auto_th;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// cB_down_auto_checked
        private void cB_down_auto_Checked(object sender, RoutedEventArgs e)
        {

            //接收消息
            down_auto_th = new Thread(down_auto_cmd);
            down_auto_th.IsBackground = true;
            down_auto_th.Priority = ThreadPriority.Highest;//线程优先级最高
                                                           //th.Start(tSocket);
            down_auto_th.Start();

        }


        private void down_auto_cmd()
        {
            while (true)
            {
                if (down_auto_cnt >= 600)
                {

                    //cube_com.generate_up_ctrl_cmd(ref up_buf[0], 1,
                    //                           cube_com.INS_DOWN_CMD_ON,
                    //                          0);
                    //client_up_Socket.Send(up_buf, 0, para_length, SocketFlags.None);
                    down_auto_cnt = 0;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_down_auto_UnChecked(object sender, RoutedEventArgs e)
        {
            down_auto_th.Abort();
        }
        #endregion


        #region  菜单及Button函数




        /// <summary>
        /// 发送上行控制指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_up_ctrl_cmd(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 发送上行注入指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_up_para_cmd(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 发送上行轨道及QR指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_up_orbit_cmd(object sender, EventArgs e)
        {
           
        }

        private void FileMode_Click(object sender, RoutedEventArgs e)
        {

            double seconds = 1442350630;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(                 //显示为本地时间
            1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(secs);

            MessageBox.Show(dt.ToString());
        }


        /// <summary>
        /// 打开网络按键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_net_open_Click(object sender, RoutedEventArgs e)
        {

            if (btn_net_open.IsPressed)
            {
                ShowMsg("网络已打开！");
                return;
            }
            tcp_down_client_create();
            tcp_up_client_create();


            io_func.CreateOBCFrameFile();
            io_func.CreateADCSFrameFile();

        }

        private void btn_send_ctrl_cmd(object sender, RoutedEventArgs e)
        {
            send_ctrl_cmd_cs();
        }
        private void btn_send_para_cmd(object sender, RoutedEventArgs e)
        {

            send_para_cmd_cs();
        }
        private void btn_send_orbit_cmd(object sender, RoutedEventArgs e)
        {
            send_orbit_cmd_cs();
        }

        private void btn_serial_open_Click(object sender, RoutedEventArgs e)
        {
            serial_create();

            io_func.CreateOBCFrameFile();
            io_func.CreateADCSFrameFile();
        }

        private void btn_serial_close_Click(object sender, RoutedEventArgs e)
        {
            SerialPort_Close();
        }

        private void btn_serial_send_Click(object sender, RoutedEventArgs e)
        {
            serial_send();
        }

        private void btn_serial_rec_clear_Click(object sender, RoutedEventArgs e)
        {
            tB_recbuf.Clear();
        }

        /// <summary>
        /// 生成并发送控制指令
        /// </summary>
        private void send_ctrl_cmd_cs()
        {

            UInt32 delay_time = 0;

            UInt16 cmd_cnt = 0;

            #region 数据生成



            if (hk_up_frm.mwa_open_checked || hk_up_frm.mwa_close_checked)    //动量轮A
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                        (hk_up_frm.mwa_open_checked) ? cubeCOMM.INS_MW_A_ON : cubeCOMM.INS_MW_A_OFF,
                                        delay_time
                                        );

                cmd_cnt++;
            }

            if (hk_up_frm.mwb_open_checked || hk_up_frm.mwb_close_checked)    //动量轮B
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.mwb_open_checked) ? cubeCOMM.INS_MW_B_ON : cubeCOMM.INS_MW_B_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }


            if (hk_up_frm.hk_reset_checked)                                     //星务重启
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      cubeCOMM.INS_OBC_RST,
                                      delay_time
                                      );

                cmd_cnt++;
            }

            if (hk_up_frm.hmra_open_checked || hk_up_frm.hmra_close_checked)                                     //磁强计A
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                       (hk_up_frm.hmra_open_checked) ? cubeCOMM.INS_SW_MAG_A_ON : cubeCOMM.INS_SW_MAG_A_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }

            if (hk_up_frm.hmrb_open_checked || hk_up_frm.hmrb_close_checked)                                     //磁强计B
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.hmrb_open_checked) ? cubeCOMM.INS_SW_MAG_B_ON : cubeCOMM.INS_SW_MAG_B_OFF,
                                      delay_time
                                      );
                cmd_cnt++;
            }

            if (hk_up_frm.fan_open_start_checked || hk_up_frm.fan_open_stop_checked)                                     //帆板开
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.fan_open_start_checked) ? cubeCOMM.INS_SLBRD_ON : cubeCOMM.INS_SLBRD_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }

            if (hk_up_frm.atenna_open_checked)                                     //天线开
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      cubeCOMM.INS_USB_ON,
                                      delay_time
                                      );

                cmd_cnt++;
            }


            if (hk_up_frm.batt_warm_open_checked || hk_up_frm.batt_warm_close_checked)                                     //电池加热
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.batt_warm_open_checked) ? cubeCOMM.INS_SW_BATT_WARM_ON : cubeCOMM.INS_SW_BATT_WARM_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }

            if (hk_up_frm.down_start_checked || hk_up_frm.down_stop_checked)                                     //星务下行
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.down_start_checked) ? cubeCOMM.INS_DOWN_CMD_ON : cubeCOMM.INS_DOWN_CMD_OFF,
                                      delay_time
                                      );
                cmd_cnt++;
            }

            if (hk_up_frm.atenna_pwr_on_checked || hk_up_frm.atenna_pwr_off_checked)                                     //天线电源开关
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                      (hk_up_frm.atenna_pwr_on_checked) ? cubeCOMM.INS_S2_ON : cubeCOMM.INS_S2_OFF,
                                      delay_time
                                      );
                cmd_cnt++;
            }


            if (hk_up_frm.adcs_open_checked || hk_up_frm.adcs_close_checked)    //姿控计算机开关
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                        (hk_up_frm.adcs_open_checked) ? cubeCOMM.INS_ADCS_ON : cubeCOMM.INS_ADCS_OFF,
                                        delay_time
                                        );

                cmd_cnt++;
            }

            if (hk_up_frm.redam_checked)//重阻尼
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_DET,
                     delay_time);
                cmd_cnt++;
            }

            if (hk_up_frm.always_dam_checked)//永久阻尼
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_STA,
                     delay_time);
                cmd_cnt++;
            }

            if (hk_up_frm.dam_mode_checked) //阻尼置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_DMP_FLAG,
                     delay_time);

                cmd_cnt++;
            }

            if (hk_up_frm.ctrl_mode_checked) //三轴稳定置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_CTL_FLAG,
                     delay_time);

                cmd_cnt++;
            }

            if (hk_up_frm.pitch_mode_checked) //俯仰控制置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_FLT_FLAG,
                     delay_time);

                cmd_cnt++;
            }

            if (hk_up_frm.zero_mode_checked)   //控制模式
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                                   cubeCOMM.INS_ZERO_MODE,
                                   delay_time
                                   );
                cmd_cnt++;
            }



            if (hk_up_frm.close_all_checked) //CLOSE ALL
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_CLOSE_ALL,
                     delay_time);

                cmd_cnt++;
            }




            if (hk_up_frm.error_checked)
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex,
                     cubeCOMM.INS_ERROR_ENABLE,
                    delay_time);

                cmd_cnt++;
            }



            #endregion


            if (cmd_cnt < 1)
            {
                MessageBox.Show("请选择一条指令！");
                return;
            }

            if (cmd_cnt>=2)
            {
                MessageBox.Show("每次只能执行一条指令，请重选！");
                return;
            }


            //串口打开则用串口
            if(In_Port.IsOpen)
            {
                In_Port.Write(up_buf, 0, cubeCOMM.ctrl_length);
                return;
            }

            if(client_up_Socket == null)
            {

                MessageBox.Show("上行未创建，请先打开网络！");
                return;
            }

            try
            {
                client_up_Socket.Send(up_buf, 0, cubeCOMM.ctrl_length, SocketFlags.None);

                clear_up_buf();
            }
            catch
            {
                MessageBox.Show("网络错误！");
            }


        }

        /// <summary>
        /// 生成并发送注入指令
        /// </summary>
        private void send_para_cmd_cs()
        {
            UInt32 delay_time = 0;

            UInt16 cmd_cnt = 0;

            if (hk_up_frm.para_D_checked)   //D参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_CTL_D_PRA,
                    delay_time,
                    hk_up_frm.para_D, 0);

                cmd_cnt++;
            }

            if (hk_up_frm.para_Z_checked)   //Z参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_ZJD_CTL,
                     delay_time,
                    hk_up_frm.para_Z, 0);

                cmd_cnt++;
            }

            if (hk_up_frm.para_P_checked)  //P参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_CTL_P_PRA,
                     delay_time,
                     hk_up_frm.para_P, 0);

                cmd_cnt++;
            }

            if (hk_up_frm.para_down_period_checked) //更新下行周期
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_COM_PERIOD,
                     delay_time,
                     hk_up_frm.para_down_period, 0);

                cmd_cnt++;
            }


            if (hk_up_frm.delay_hk_checked) //下行延时遥测
            {
                UInt32 para1 = 0, para2 = 0;


                para1 = hk_up_frm.delay_hk_select * Convert.ToUInt32(Math.Pow(2, 16)) + hk_up_frm.delay_hk_index;

                para2 = hk_up_frm.delay_hk_orbit_cnt;

                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_DOWN_TEL,
                     delay_time,
                     para1, para2);

                cmd_cnt++;
            }


            if (hk_up_frm.pianzhi_mode_checked)
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_PIANZHI_MODE,
                   delay_time,
                   hk_up_frm.bias_mode, 0);

                cmd_cnt++;
            }



            if (hk_up_frm.para_time_checked) //星上时间注入
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_ADCS_TIME_IN,
                     delay_time,
                     hk_up_frm.para_time, 0);
                cmd_cnt++;

            }


            if (hk_up_frm.camera_checked) //延时成像
            {
                cubeCOMM.generate_up_para_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, 0x71,//cube_com.INS_CAMERA,
                        delay_time,
                     hk_up_frm.camera_delay_time, 0);
                cmd_cnt++;

            }


            if (cmd_cnt < 1)
            {
                MessageBox.Show("请选择一条指令！");
                return;
            }

            if (cmd_cnt >= 2)
            {
                MessageBox.Show("每次只能执行一条指令，请重选！");
                return;
            }

            //串口打开则用串口
            if (In_Port.IsOpen)
            {
                In_Port.Write(up_buf, 0, cubeCOMM.para_length);
                return;
            }

            if (client_up_Socket == null)
            {

                MessageBox.Show("上行未创建，请先打开网络！");
                return;
            }

            try
            {
                client_up_Socket.Send(up_buf, 0, cubeCOMM.para_length, SocketFlags.None);

                clear_up_buf();
            }
            catch
            {
                MessageBox.Show("网络错误！");
            }


   

        }

        /// <summary>
        /// 生成并发送轨道参数指令
        /// </summary>
        private void send_orbit_cmd_cs()
        {
            UInt32 delay_time = 0;

            if (hk_up_frm.orbit_checked)
            {
                cubeCOMM.generate_up_orbit_cmd_cs(ref up_buf, (byte)cB_pid_81.SelectedIndex, cubeCOMM.INS_ORB_TLE_FLAG,
                     delay_time,
                     hk_up_frm.orbit);

            }

            //串口打开则用串口
            if (In_Port.IsOpen)
            {
                In_Port.Write(up_buf, 0, cubeCOMM.orbit_length);
                return;
            }

            if (client_up_Socket == null)
            {

                MessageBox.Show("上行未创建，请先打开网络！");
                return;
            }

            try
            {
                client_up_Socket.Send(up_buf, 0, cubeCOMM.orbit_length, SocketFlags.None);

                clear_up_buf();
            }
            catch
            {
                MessageBox.Show("网络错误！");
            }


        }

        private void clear_up_buf()
        {
            for (int kc = 0; kc < 200; kc++)
                up_buf[kc] = 0;
        }

        #endregion


        #region 定时器

        string local_time_now;//本机时间
        MmTimer local_time_timer = new MmTimer();

        private void get_local_time()
        {
            local_time_now = DateTime.Now.ToString("yyyy-MM-dd") +
                  '(' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ')';

            local_timer_start();
        }

        private void local_timer_start()
        {
            local_time_timer.Interval = 1000;
            local_time_timer.Mode = MmTimerMode.Periodic;
            local_time_timer.Tick += new EventHandler(local_timer_handler);
            local_time_timer.Start();
        }

        private void local_timer_handler(object sender, EventArgs e)
        {
            local_time_now = DateTime.Now.ToString("yyyy年MM月dd日") +
                   DateTime.Now.ToLongTimeString().ToString();

            down_auto_cnt += 1;
            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {
                    timerCnt_frm.setLocaltime(local_time_now);
                }));
            }).Start();
        }

        #endregion



        #region Socket Client功能函数

        private Dictionary<string, Socket> dic = new Dictionary<string, Socket>();

        private Socket hk_client;

        private Socket client_down_Socket, client_up_Socket;


        //EndPoint Remote;
        /// <summary>
        /// 创建TCP Client
        /// </summary>       
        public void tcp_down_client_create()
        {

            //ip地址
            IPAddress ip = IPAddress.Parse(iNet_frm.iNet_IP);

            //端口号
            IPEndPoint point = new IPEndPoint(ip, int.Parse(iNet_frm.iNet_DOWN_NO));

            client_down_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client_down_Socket.Connect(point); //配置服务器IP与端口  
                ShowMsg("连接下行服务器成功");

                //接收消息
                Thread th = new Thread(ReceiveMsg);
                th.IsBackground = true;
                th.Priority = ThreadPriority.Highest;//线程优先级最高
                                                     //th.Start(tSocket);
                th.Start();

            }
            catch
            {
                ShowMsg("连接下行服务器失败，请按回车键退出！");
                return;
            }
        }

        public void tcp_up_client_create()
        {

            //ip地址
            IPAddress ip = IPAddress.Parse(iNet_frm.iNet_IP);

            //端口号
            IPEndPoint point = new IPEndPoint(ip, int.Parse(iNet_frm.iNet_UP_NO));

            client_up_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client_up_Socket.Connect(point); //配置服务器IP与端口  
                ShowMsg("连接上行服务器成功");

            }
            catch
            {
                ShowMsg("连接上行服务器失败，请按回车键退出！");
                return;
            }
        }
        /// <summary>
        /// TCP接收进程
        /// </summary>
        private void ReceiveMsg()
        {
            byte[] buffer_rc = new byte[10240];

            while (true)
            {
                try
                {
                    Int32 n = client_down_Socket.Receive(buffer_rc);

                    if (n > 0)
                    {
                        // ShowMsg("rec num is " + n.ToString());
                        byte[] csp_buff = new byte[n];
                        for (int kc = 0; kc < n; kc++)
                            csp_buff[kc] = buffer_rc[kc];



                        //analysis_down_cmd_81(csp_buff);
                    }
                }

                catch (Exception ex)
                {
                    ShowMsg("Error+" + ex.Message);
                    // break;
                }
            }
        }

        /// <summary>
        /// 下行链路协议解析
        /// </summary>
        /// <param name="buffer"></param>
        private void analysis_rec_buf(byte[] buffer)
        {

            foreach (byte Buf in buffer)
            {
                switch (rec_state)
                {
                    case 0:
                        down_info_buf_length = 0;
                        down_info_buf[down_info_buf_length++] = Buf;

                        if ((Buf == 0x1A))
                        { 
                            rec_state = 1;
                        }

                        break;
                    #region 星务帧
                    case 1:
                        down_info_buf[down_info_buf_length++] = Buf;
                        if ((Buf == 0x50))
                        {
                            rec_state = 2;
                        }
                        else if (Buf == 0x51)
                        {                    
                            rec_state = 3;
                        }

                        //else if(Buf ==0x53)
                        //{

                        //}

                        //else if(Buf == 0x56)
                        //{
                        //    rec_state = 5;
                        //}

                        else
                        {
                            rec_state = 0;
                        }

                        break;
                    case 2:
                        {
                            down_info_buf[down_info_buf_length++] = Buf;

                            if (down_info_buf_length >= cubeCOMM.obc_length)
                            {
                                rec_down_info_count++;      //接收到的指令数加1
                                rec_state = 0;

                                cubeCOMM.get_info_from_obc_buf(down_info_buf, ref obc_info);

                                obc_displayAndsave();

                            }
                            break;
                        }

                    case 3:
                        {
                            down_info_buf[down_info_buf_length++] = Buf;
                            if(down_info_buf_length>= cubeCOMM.adcs_length)
                            {
                                rec_down_info_count++;      //接收到的指令数加1
                                rec_state = 0;
                                cubeCOMM.get_info_from_adcs_buf(down_info_buf, ref adcs_info);

                                adcs_displayAndsave();

                            }
                            break;
                        }

                    case 5:
                        {

                            break;
                        }

                    #endregion

                    default:
                        break;
                }
            }
        }


        private void obc_displayAndsave()
        {

            double seconds = obc_info.utc_time;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(                 //显示为本地时间
            1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(secs);


            timerCnt_frm.setCounter(rec_down_info_count, obc_info.down_count);
        
            timerCnt_frm.setCubetime(dt.ToString("yyyy年MM月dd日hh:mm:ss"));

            Temp_frm.displayTemperature(obc_info);


            sat_status.dis_cubesat_status(obc_info.on_off_status, obc_info.work_mode);
            hk_obc_frm.display_obc_info(obc_info);
                 
            io_func.WriteObcFrameFile(obc_info);

        }
        private void adcs_displayAndsave()
        {

            //tBk_rec_frame_cnt.Text = rec_down_info_count.ToString();
            //tBk_sat_down_frame_cnt.Text = down_adcs_81.response_count.ToString();
            hk_adcs_frm.display_adcs_info(adcs_info);
            //sat_status.set_sat_color(down_adcs_81.control_mode);
            io_func.WriteAdcsFrameFile(adcs_info);

        }



        private void ShowMsg(string str)
        {

            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    tB_recbuf.AppendText(str + "\r\n");
                }));
            }).Start();

        }

        #endregion

        #region 串口

        private SerialPort In_Port = new SerialPort();      ///接收数据串口
        private StringBuilder builder = new StringBuilder(); //格式化串口接收缓冲



        /// <summary>
        /// 创建串口
        /// </summary>
        private void serial_create()
        {
            if (In_Port.IsOpen)
            {
                MessageBox.Show("串口已打开！");
                return;
            }

            In_Port.PortName = gcSerial_frm.Port_ID;
            In_Port.BaudRate = 115200;

            In_Port.Parity = Parity.None;
            In_Port.StopBits = StopBits.One;
            In_Port.DataBits = 8;
            //In_Port.ReadTimeout = 200;
            //In_Port.WriteTimeout = 50;

            In_Port.ReadBufferSize = 4096;
            In_Port.DataReceived += new SerialDataReceivedEventHandler(In_Port_DataReceived);

            In_Port.Open();


        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        private void SerialPort_Close()
        {
            if (In_Port.IsOpen)  In_Port.Close();     
        }


        private delegate void UpdateUiTextDelegate(byte[] rec_buf);
        /// <summary>
        /// 串口中断响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void In_Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int bytes = In_Port.BytesToRead;
            if (bytes > 0)
            {
                byte[] buffer = new byte[bytes];    // 创建字节数组 
                In_Port.Read(buffer, 0, bytes);       // 读取缓冲区的数据到数组
                //ShowMsg("rec len is " + bytes.ToString());
                Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(rec_buff_proc), buffer);
            }
        }

        /// <summary>
        /// 串口接收数据显示到接收区
        /// </summary>
        /// <param name="buf"></param>
        private void rec_buff_proc(byte[] rec_buf)
        {

            analysis_rec_buf(rec_buf);
            if (gcSerial_frm.rec_show)
                rec_buff_display(rec_buf);

        }

        /// <summary>
        /// 串口接收数据显示到接收区
        /// </summary>
        /// <param name="buf"></param>
        private void rec_buff_display(byte[] rec_buf)
        {

            if (gcSerial_frm.rec_show_hex)
            {
                foreach (byte b in rec_buf)
                {
                    builder.Append(b.ToString("X2") + " ");
                }

            }
            else
            {
                builder.Append(Encoding.ASCII.GetString(rec_buf));
            }
            tB_recbuf.AppendText(builder.ToString());

        }

        private void serial_send()
        {
            //In_Port.Write("\r\n");
            string str = "";
            byte[] sendbuf= new byte[11];

            sendbuf[0] = 0x0B;
            sendbuf[1] = 0x01;
            sendbuf[2] = 0x03;
            sendbuf[3] = 0x00;
            sendbuf[4] = 0x03;
            sendbuf[5] = 0x04;
            sendbuf[6] = 0x05;
            sendbuf[7] = 0x00;
            sendbuf[8] = 0x14;
            sendbuf[9] = 0x00;
            sendbuf[10] = 0x00;


            str = System.Text.Encoding.ASCII.GetString(sendbuf);

            //In_Port.Write(sendbuf, 0, sendbuf.Length);
            In_Port.Write(str);
            //In_Port.Write("1234567890");
            In_Port.Write("\r");

        }

        #endregion



        #region socket server代码

        /// <summary>
        ///使用 IP4地址。
        /// </summary>
        /// <param name="txtIP"></param>
        /// <param name="txtPort"></param>
        public void socker_create()
        {
            //ip地址
            IPAddress ip = IPAddress.Parse(iNet_frm.iNet_IP);

            //端口号
            IPEndPoint point = new IPEndPoint(ip, int.Parse(iNet_frm.iNet_UP_NO));

            //创建监听用的Socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



            //创建好socket后，告诉socket绑定的IP地址和端口号 让socket监听point
            try
            {
                //socket监听哪个端口
                socket.Bind(point);

                //同一个时间点过来10个客户端，排队
                socket.Listen(10);
                ShowMsg("服务器开始监听");
                Thread thread = new Thread(AcceptInfo);
                thread.IsBackground = true;
                thread.Start(socket);
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }



        void AcceptInfo(object o)
        {
            Socket socket = o as Socket;

            while (true)
            {
                //通信用socket
                try
                {
                    //创建通信用的Socket
                    hk_client = socket.Accept();

                    string point = hk_client.RemoteEndPoint.ToString();

                    ShowMsg(point + "连接成功！");
                    dic.Add(point, hk_client);
                    //接收消息
                    Thread th = new Thread(serverReceiveMsg);
                    th.IsBackground = true;
                    th.Priority = ThreadPriority.Highest;//线程优先级最高
                    //th.Start(tSocket);
                    th.Start(hk_client);
                }

                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                    break;
                }
            }

        }

        private void serverReceiveMsg(object o)
        {
            Socket client = o as Socket;

            byte[] buffer = new byte[10240];

            while (true)
            {
                try
                {
                    int n = hk_client.Receive(buffer);

                    if (n > 0)
                    {
                        byte[] csp_buff = new byte[n];
                        for (int kc = 0; kc < n; kc++)
                            csp_buff[kc] = buffer[kc];

                        //ShowMsg_Hex(csp_buff, n);

                        analysis_rec_buf(csp_buff);
                    }
                }

                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                    break;
                }
            }
        }
        #endregion

    }
}
