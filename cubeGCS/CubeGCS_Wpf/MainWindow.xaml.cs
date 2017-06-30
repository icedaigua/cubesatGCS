using System.Windows;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;


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
            camera_frm.SerialPort_Close();
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
            send_ctrl_cmd_cs();
        }

        /// <summary>
        /// 发送上行注入指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_up_para_cmd(object sender, EventArgs e)
        {
            send_para_cmd_cs();
        }

        /// <summary>
        /// 发送上行轨道及QR指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_up_orbit_cmd(object sender, EventArgs e)
        {
            send_orbit_cmd_cs();
        }

        private void FileMode_Click(object sender, RoutedEventArgs e)
        {

            double seconds = 1442350630;

            double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(                 //显示为本地时间
            1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(secs);

            MessageBox.Show(dt.ToString());
        }



        private void btn_down_img_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btn_img_trans_Click(object sender, RoutedEventArgs e)
        {
            camera_frm.image_trans();
        }

        private void btn_send_cameraCmd_Click(object sender, RoutedEventArgs e)
        {
            camera_frm.send_camera_cmd(up_buf);

            GCS_send_cmd(up_buf, 0, cubeCOMM.para_length, 1);
        }

        private void btn_img_proc_Click(object sender, RoutedEventArgs e)
        {
            camera_frm.image_proc();
           

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
            builder.Clear();
            tB_recbuf.Clear();
        }

        /// <summary>
        /// 生成并发送控制指令
        /// </summary>
        private void send_ctrl_cmd_cs()
        {

            UInt32 delay_time =Convert.ToUInt32( tB_delay_time.Text);

            byte cmd_cnt = 0;

            hk_up_frm.createCtrlCmd(up_buf, (byte)cB_pid_81.SelectedIndex, delay_time, ref cmd_cnt);


            GCS_send_cmd(up_buf, 0, cubeCOMM.ctrl_length,cmd_cnt);

            tB_delay_time.Text = 0.ToString();

        }

        /// <summary>
        /// 生成并发送注入指令
        /// </summary>
        private void send_para_cmd_cs()
        {
            UInt32 delay_time = Convert.ToUInt32(tB_delay_time.Text);

            byte cmd_cnt = 0;

            hk_up_frm.createParametersCmd(up_buf, (byte)cB_pid_81.SelectedIndex, delay_time, ref cmd_cnt);

            GCS_send_cmd(up_buf, 0, cubeCOMM.para_length, cmd_cnt);

            tB_delay_time.Text = 0.ToString();

        }

        /// <summary>
        /// 生成并发送轨道参数指令
        /// </summary>
        private void send_orbit_cmd_cs()
        {
            UInt32 delay_time = Convert.ToUInt32(tB_delay_time.Text);
            byte cmd_cnt = 0;

            hk_up_frm.createOrbitCmd(up_buf, (byte)cB_pid_81.SelectedIndex, delay_time, ref cmd_cnt);

            GCS_send_cmd(up_buf, 0, cubeCOMM.orbit_length, cmd_cnt);

            tB_delay_time.Text = 0.ToString();

        }



        private void GCS_send_cmd(byte[] cmd,int addr,int length,UInt16 CMDcnt)
        {
            if (CMDcnt < 1)
            {
                MessageBox.Show("请选择一条指令！");
                return;
            }

            if (CMDcnt >= 2)
            {
                MessageBox.Show("每次只能执行一条指令，请重选！");
                return;
            }


            //串口打开则用串口
            if (In_Port.IsOpen)
            {
                In_Port.Write(cmd, addr, length);
                return;
            }

            if (client_up_Socket == null)
            {

                MessageBox.Show("上行未创建，请先打开网络！");
                return;
            }

            try
            {
                client_up_Socket.Send(cmd, addr, length, SocketFlags.None);

                //clear_up_buf();
            }
            catch
            {
                MessageBox.Show("网络错误！");
            }
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

                        Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(rec_buff_proc), csp_buff);

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

        #region 下行解码
        private void analysis_rec_buf(byte[] buffer)
        {
            //new Thread(() => {
            //this.Dispatcher.Invoke(new Action(() => {

             foreach (byte Buf in buffer)
            {
                if (rec_state <= cubeCOMM.FRAME_START)
                {
                    searchHearder(Buf);
                    continue;
                }
                switch (rec_state)
                {
                    case cubeCOMM.FRAME_OBC:
                        ParseOBC(Buf);
                        break;
                    case cubeCOMM.FRAME_ADCS:
                        ParseADCS(Buf);
                        break;
                    case cubeCOMM.FRAME_RESPONSE:
                        ParseResponse(Buf);
                        break;
                    case cubeCOMM.FRAME_CAMERA:
                        ParseCamera(Buf);
                        break;
                    case cubeCOMM.FRAME_NULL:
                    break;
                    default:
                        break;
                }

            }

            //}));
            //}).Start();
        }

        private byte searchHearder(byte Buf)
        {
            switch (rec_state)
            {
                case cubeCOMM.FRAME_NULL:
                    down_info_buf_length = 0;
                    down_info_buf[down_info_buf_length++] = Buf;

                    if ((Buf == 0x1A))
                    {
                        rec_state = cubeCOMM.FRAME_START;
                    }
                    break;

                case 1:
                    down_info_buf[down_info_buf_length++] = Buf;
                    if ((Buf == 0x50))
                    {
                        rec_state = cubeCOMM.FRAME_OBC;
                    }
                    else if (Buf == 0x51)
                    {
                        rec_state = cubeCOMM.FRAME_ADCS;
                    }

                    else if (Buf == 0x53)
                    {
                        rec_state = cubeCOMM.FRAME_RESPONSE;

                    }
                    else if (Buf == 0x56)
                    {
                        rec_state = cubeCOMM.FRAME_CAMERA;

                    }
                    else
                    {
                        rec_state = cubeCOMM.FRAME_NULL;
                    }
                    break;
                default:
                    {
                        rec_state = cubeCOMM.FRAME_NULL;
                        break;
                    }

            }
            return rec_state;
        }

        private void ParseOBC(byte Buf)
        {
            down_info_buf[down_info_buf_length++] = Buf;

            if (down_info_buf_length >= cubeCOMM.obc_length)
            {
                rec_down_info_count++;      //接收到的指令数加1

                cubeCOMM.get_info_from_obc_buf(down_info_buf, ref obc_info);
                rec_state = 0;
                obc_displayAndsave();
            }
        }

        private void ParseResponse(byte Buf)
        {
            down_info_buf[down_info_buf_length++] = Buf;
            if (down_info_buf_length >= 5)
            {
                //byte[] new_rec = new byte[5];
                //down_info_buf.CopyTo(new_rec, 5);
                //rec_buff_display(new_rec);
                rec_state = 0;
            }
        }

        private void ParseADCS(byte Buf)
        {
            down_info_buf[down_info_buf_length++] = Buf;
            if (down_info_buf_length >= cubeCOMM.adcs_length)
            {
                rec_down_info_count++;      //接收到的指令数加1

                cubeCOMM.get_info_from_adcs_buf(down_info_buf, ref adcs_info);
                rec_state = 0;
                adcs_displayAndsave();

            }
        }

        private void ParseCamera(byte Buf)
        {
            down_info_buf[down_info_buf_length++] = Buf;

            if (down_info_buf_length >= down_info_buf[2]+5)
            {
                rec_down_info_count++;      //接收到的指令数加1

                camera_frm.CameraProcess(down_info_buf);
                rec_state = 0;
                //obc_displayAndsave();
            }
        }

        private void obc_displayAndsave()
        {

                //double seconds = obc_info.utc_time;

                //double secs = Convert.ToDouble(seconds);

            DateTime dt = new DateTime(                 //显示为本地时间
            1970, 1, 1, 0, 0, 0, DateTimeKind.Local).
            AddSeconds(Convert.ToDouble(obc_info.utc_time));


            timerCnt_frm.setCounter(rec_down_info_count, obc_info.down_count);

            timerCnt_frm.setCubetime(dt.ToString("yyyy年MM月dd日hh:mm:ss"));

            Temp_frm.displayTemperature(obc_info);


            sat_status.dis_cubesat_status(obc_info.on_off_status, obc_info.work_mode);
            hk_obc_frm.display_obc_info(obc_info);
            hk_eps_frm.display_eps_info(obc_info);

            io_func.WriteObcFrameFile(obc_info);

        }
        private void adcs_displayAndsave()
        {

            //tBk_rec_frame_cnt.Text = rec_down_info_count.ToString();
            //tBk_sat_down_frame_cnt.Text = down_adcs_81.response_count.ToString();
            hk_adcs_frm.display_adcs_info(adcs_info);
            sat_status.set_sat_color(adcs_info.adcs_ctrl_mode);
            io_func.WriteAdcsFrameFile(adcs_info);

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
            new Thread(() => {
            this.Dispatcher.Invoke(new Action(() => {
                builder.Clear();
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
            builder.Append('\n');
            tB_recbuf.Text += builder.ToString();

            tB_recbuf.ScrollToEnd();
            }));
            }).Start();
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

        private Socket hk_client;
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

        private void btn_test_click(object sender, RoutedEventArgs e)
        {

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

        #region TEST
        public void test()
        {
         
        }
        #endregion

    }
}
