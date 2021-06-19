using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
//using UniHelper;


namespace iNet
{
    /// <summary>
    /// iNetClient.xaml 的交互逻辑
    /// </summary>
    public partial class iNetClient : UserControl
    {

        #region 参数
        public string iNet_Type
        {
            get { return cB_socket.Text; }
        }
        public string iNet_IP
        {
            get { return tB_IP.Text; }
        }


        public string iNet_NO
        {
            get { return tB_No.Text; }
        }



        public byte[] testbuf2 = new byte[232] {
            0x80 , 0x5A , 0x5A , 0x04 , 0x03 , 0x02 , 0x01 , 0xA0, 0xA0 , 0xA0, 0xA0 , 0x11 , 0x01, 0x11 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00, 0x00, 0x00, 0x82, 0x1A, 0x50, 0xCE, 0xE3, 0x1D, 0xC8, 0x00,
            0xEB , 0x90 , 0xDC , 0x32 , 0x01 , 0x35 , 0x01 , 0xC5, 0x06 , 0xCD, 0x06 , 0x75 , 0x05, 0x15 , 0x07 , 0xE1 , 0x07, 0xE1 , 0x07 , 0xE1 , 0x04 , 0x6D, 0x0B, 0x09, 0x03, 0x25, 0x09, 0x65, 0x0B, 0xB9, 0x05, 0xE1,
            0x04 , 0x71 , 0x07 , 0xC1 , 0x03 , 0xDD , 0x00 , 0x05, 0x03 , 0x51, 0x09 , 0xC5 , 0x0B, 0x9D , 0x0A , 0x41 , 0x03, 0xA1 , 0x01 , 0x21 , 0x07 , 0xD5, 0x04, 0xD5, 0x03, 0x85, 0x00, 0x05, 0x01, 0x0D, 0x07, 0x99,
            0x00 , 0x11 , 0x07 , 0x65 , 0x01 , 0x11 , 0x01 , 0x05, 0x01 , 0x05, 0x01 , 0x05 , 0x01, 0x0D , 0x01 , 0x0D , 0x01, 0x0D , 0x01 , 0x0D , 0x01 , 0x0D, 0x07, 0xE1, 0x0E, 0x89, 0x07, 0xE5, 0x07, 0xE1, 0x07, 0xE1,
            0x07 , 0xE1 , 0x00 , 0x11 , 0x01 , 0x0D , 0x11 , 0x55, 0x63 , 0x01, 0x01 , 0x05 , 0x0B, 0x5B , 0xD9 , 0xC2 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x54, 0x9F, 0x40, 0x00, 0x00, 0x00, 0x00, 0xFC, 0xBA, 0x17, 0x41,
            0x3B , 0x8B , 0xDE , 0xA9 , 0xD8 , 0x9A , 0xB4 , 0x40, 0xA5 , 0x82, 0x8A , 0xAA , 0x0F, 0xFF , 0xAF , 0x40 , 0x9E, 0xD0 , 0xEB , 0x4F , 0xE2 , 0x93, 0x27, 0x40, 0xFA, 0xB3, 0x1F, 0x29, 0x22, 0x43, 0xED, 0x3F,
            0x7F , 0xC2 , 0xD9 , 0xAD , 0x65 , 0x32 , 0xF3 , 0xBF, 0x03 , 0xEE, 0x79 , 0xFE , 0xB4, 0xA1 , 0x1E , 0x40 , 0x3A, 0x65 , 0xDB , 0xC6 , 0xA6 , 0x89, 0x81, 0xBF, 0x00, 0x65, 0x92, 0xE7, 0xC7, 0x13, 0x08, 0x40,
            0xF1 , 0x4F , 0x1D , 0x7B , 0x9C , 0x18 , 0x00 , 0x40};


        public byte[] testbuf = new byte[224] {
            0x80 , 0x5A , 0x5A , 0x04 , 0x03 , 0x02 , 0x01 , 0xA0, 0xA0 , 0xA0, 0xA0 , 0x11 , 0x01, 0x11 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00, 0x00, 0x00, 0x82, 0x1A, 0x50, 0xCE, 0xE3, 0x1D, 0xB8, 0x00,
            0xEB , 0x90 , 0xDC , 0x32 , 0x01 , 0x35 , 0x07 , 0xE1 , 0x07, 0xE1 , 0x07 , 0xE1 , 0x04 , 0x6D, 0x0B, 0x09, 0x03, 0x25, 0x09, 0x65, 0x0B, 0xB9, 0x05, 0xE1,
            0x04 , 0x71 , 0x07 , 0xC1 , 0x03 , 0xDD , 0x00 , 0x05, 0x03 , 0x51, 0x09 , 0xC5 , 0x0B, 0x9D , 0x0A , 0x41 , 0x03, 0xA1 , 0x01 , 0x21 , 0x07 , 0xD5, 0x04, 0xD5, 0x03, 0x85, 0x00, 0x05, 0x01, 0x0D, 0x07, 0x99,
            0x00 , 0x11 , 0x07 , 0x65 , 0x01 , 0x11 , 0x01 , 0x05, 0x01 , 0x05, 0x01 , 0x05 , 0x01, 0x0D , 0x01 , 0x0D , 0x01, 0x0D , 0x01 , 0x0D , 0x01 , 0x0D, 0x07, 0xE1, 0x0E, 0x89, 0x07, 0xE5, 0x07, 0xE1, 0x07, 0xE1,
            0x07 , 0xE1 , 0x00 , 0x11 , 0x01 , 0x0D , 0x11 , 0x55, 0x63 , 0x01, 0x01 , 0x05 , 0x0B, 0x5B , 0xD9 , 0xC2 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x54, 0x9F, 0x40, 0x00, 0x00, 0x00, 0x00, 0xFC, 0xBA, 0x17, 0x41,
            0x3B , 0x8B , 0xDE , 0xA9 , 0xD8 , 0x9A , 0xB4 , 0x40, 0xA5 , 0x82, 0x8A , 0xAA , 0x0F, 0xFF , 0xAF , 0x40 , 0x9E, 0xD0 , 0xEB , 0x4F , 0xE2 , 0x93, 0x27, 0x40, 0xFA, 0xB3, 0x1F, 0x29, 0x22, 0x43, 0xED, 0x3F,
            0x7F , 0xC2 , 0xD9 , 0xAD , 0x65 , 0x32 , 0xF3 , 0xBF, 0x03 , 0xEE, 0x79 , 0xFE , 0xB4, 0xA1 , 0x1E , 0x40 , 0x3A, 0x65 , 0xDB , 0xC6 , 0xA6 , 0x89, 0x81, 0xBF, 0x00, 0x65, 0x92, 0xE7, 0xC7, 0x13, 0x08, 0x40,
            0xF1 , 0x4F , 0x1D , 0x7B , 0x9C , 0x18 , 0x00 , 0x40};

        public byte[] testbuf3 = new byte[216]
{
            0x80 , 0x5B , 0x5B , 0x04 , 0x03 , 0x02 , 0x01 , 0xA0, 0xA0 , 0xA0, 0xA0 , 0x11 , 0x01, 0x11 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00, 0x00, 0x00, 0x82, 0x1A, 0x50, 0xCE, 0xE3, 0x1D, 0xB8, 0x00,
            0x42, 0x01, 0xE1,
            0x05, 0x05, 0x18, 0x00,
    0x01 , 0x00 , 0x00 , 0x00, 0xC4 , 0x00 , 0x10 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xD9 , 0x84 , 0x55 , 0x7C , 0xF1 ,
            0x11 , 0x00 , 0x00 , 0x60 , 0x01 , 0x04 , 0x00 , 0x10 , 0x10 , 0x1C , 0x00 , 0x1D , 0x00,
            0x23, 0x00, 0x21, 0x00, 0x22, 0x00, 0x22, 0x00 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xC5 , 0x09 , 0xCC , 0x09 , 0xC8 , 0x09 , 0x45 , 0x01 , 0xD0 , 0x09 , 0xC5 ,
            0x09 , 0xFD , 0x01 , 0xB2 , 0x1C , 0x50 , 0x01 , 0x02 , 0x00 , 0x01 , 0x00 , 0x02 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00 , 0x01 , 0x00 , 0x70, 0x00 , 0x23 , 0x00 , 0x21 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 ,
            0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xB8 , 0x06 , 0x00 , 0x00 , 0x00 , 0x80 , 0x02 , 0xE0 , 0x61 , 0x00 ,
            0xC2 , 0x00 , 0xC0 , 0x49 , 0x02 , 0x00 , 0x0C , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
};

        #endregion


        #region 窗体初始化
        public iNetClient()
        {
            InitializeComponent();

            cB_socket_Initz();
        }

        private void cB_socket_Initz()
        {
            string[] plist = new string[2] { "UDP", "TCP" };
            cB_socket.SelectedIndex = 1;
            cB_socket.ItemsSource = plist;
            cB_socket.SelectedIndex = cB_socket.Items.Count > 0 ? 1 : -1;
        }


        public void iNetInitz(string header = "网络设置", string ip = "192.168.1.1", string port = "8001")
        {
            gB_iNet.Header = header;
            tB_IP.Text = ip;
            tB_No.Text = port;

        }


        public void setTextWidth(double width)
        {
            tB_recbuf.Width = (width - 250) * 2 / 3;
            tB_sendbuf.Width = (width - 250) / 3;

            tBk_recCnt.Width = (width - 250) * 2 / 3;
            tBk_sendCnt.Width = (width - 250) / 3;
        }

        #endregion


        #region 按键响应
        public delegate void OpenClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CloseClickEventHandler(object sender, RoutedEventArgs e);
        //public delegate void SendClickEventHandler();

        public event OpenClickEventHandler OpenHandler;
        public event CloseClickEventHandler CloseHandler;
        //public event SendClickEventHandler SendHandler;

        private void btn_Net_open_Click(object sender, RoutedEventArgs e)
        {
            ShowMsg("当前模式:" + cB_socket.Text);

            cB_socket.IsEnabled = false;
            socket_Type = cB_socket.Text;

            localTcpClient_create(sender, e);

            if (OpenHandler != null)
                OpenHandler(sender, e);
        }

        private void btn_Net_close_Click(object sender, RoutedEventArgs e)
        {
            closeClient();

            if (CloseHandler != null)
                CloseHandler(sender, e);
        }

        private void btn_net_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tB_sendbuf.Text == "")
                {
                    SendMsg(Encoding.Default.GetBytes("AAA "));
                    SendMsg(testbuf);
                    SendMsg(Encoding.Default.GetBytes("\r\n"));

                }
                else
                    SendMsg(Encoding.Default.GetBytes(tB_sendbuf.Text + "\r\n"));
            }
            catch (Exception ex)
            {
                ShowMsg("连接下行服务器失败:" + ex.Message);
                Trace.WriteLine("连接下行服务器失败:" + ex.Message);
                return;
            }
            //SendHandler();
        }

        private void btn_clear_rec_Click(object sender, RoutedEventArgs e)
        {
            builder.Clear();
            tB_recbuf.Clear();
        }

        private bool showrec = false;
        private void btn_show_rec_Click(object sender, RoutedEventArgs e)
        {

            if (showrec)
            {
                btn_rec_show.Content = "显示接收";
            }
            else
            {
                btn_rec_show.Content = "不显示接收";
            }

            showrec = !showrec;

        }

        #endregion






        #region Socket Client功能函数

        #region 参数
        private Socket localClient_Socket;

        private byte[] localDataRecv = new byte[1024];  //遥测接收缓冲区/

        private String PATTERN_IP = "(\\d*\\.){3}\\d*";


        private UInt32 localRecvCount = 0;  //接收计数
        private UInt32 localSendCount = 0;  //发送计数 


        private IPHostEntry ipHost;
        private IPAddress ip;
        //端口号
        private IPEndPoint Point;

        private string socket_Type = "TCP";

        //委托
        public delegate void iNetSocketDataArrival(byte[] data);
        public iNetSocketDataArrival socketDataArrival;//= socketDataArrivalHandler;  //这里不用static会报错,因此和此函数相关的函数和变量都要设置程static




        #endregion


        /// <summary>
        /// 创建TCP Client
        /// </summary>       
        private void localTcpClient_create(object sender, RoutedEventArgs e)
        {

            //ip地址
            try
            {
                Match match = Regex.Match(iNet_IP, PATTERN_IP); //判断是否IP地址
                if (match.Value == "")
                {
                    ipHost = Dns.GetHostEntry(iNet_IP);
                    //端口号
                    Point = new IPEndPoint(ipHost.AddressList[0], int.Parse(iNet_NO)); //new IPEndPoint(ip, int.Parse(iNet_NO));
                }
                else
                {
                    //IP地址
                    ip = IPAddress.Parse(iNet_IP);
                    //端口号
                    Point = new IPEndPoint(ip, int.Parse(iNet_NO));
                }
                socket_create_connect();
                return;
            }
            catch (Exception ex)
            {
                ShowMsg("网络连接启动错误:" + ex.Message);
                Trace.WriteLine("网络连接启动错误:" + ex.Message);
            }


        }


        ///
        /// 创建套接字+异步连接函数 
        private bool socket_create_connect()
        {
            try
            {
                if (socket_Type == "TCP")
                {
                    localClient_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    localClient_Socket.Connect(Point);
                }
                else
                {
                    localClient_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    localClient_Socket.Bind(Point);
                }

                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), localClient_Socket);

                ShowMsg("连接本地服务器成功");
                Trace.WriteLine("连接本地服务器成功");

                return true;
            }
            catch (Exception ex)
            {

                ShowMsg("连接" + socket_Type + "服务器失败:" + ex.Message);
                Trace.WriteLine("连接" + socket_Type + "服务器失败:" + ex.Message + ex.StackTrace);
                return false;
            }

        }

        private void OnReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket peerSock = (Socket)ar.AsyncState;
                int BytesRead = peerSock.EndReceive(ar);

                if (BytesRead > 0)
                {
                    byte[] recBuf = new byte[BytesRead];
                    Array.ConstrainedCopy(localDataRecv, 0, recBuf, 0, BytesRead);

                    if (showrec)
                        ShowMsg(recBuf);

                    localRecvCount += (UInt32)BytesRead;
                    new Thread(() =>
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            tBk_recCnt.Text = "已接收字节数：" + localRecvCount.ToString();
                        }));
                    }).Start();

                    if (socketDataArrival != null)
                    {
                        socketDataArrival(recBuf);
                    }
                }

                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), localClient_Socket);
            }
            catch (Exception ex)
            {
                ShowMsg("socket OnReceiveCallback错误:" + ex.Message);
                Trace.WriteLine("socket OnReceiveCallback错误:" + ex.Message);
                Reconnect();
                return;
            }
        }


        ///
        /// 断线重连函数
        /// 
        private bool Reconnect()
        {

            try
            {
                ShowMsg("TCP socket开始重连");
                Trace.WriteLine("TCP socket开始重连");
                //关闭socket
                localClient_Socket.Shutdown(SocketShutdown.Both);
                localClient_Socket.Disconnect(true);
                localClient_Socket.Close();
            }
            catch (Exception ex)
            {
                ShowMsg("关闭TCP失败:" + ex.Message);
                Trace.WriteLine("关闭TCP失败:" + ex.Message);
            }

            //创建socket
            return socket_create_connect();
        }

        ///
        /// 同步send函数
        ///
        private bool socket_send(byte[] sendMessage)
        {

            if (socket_Type == "TCP")
            {
                if (checkSocketState())
                {
                    return SendData(sendMessage);
                }
                return false;
            }
            else
            {
                try
                {
                    int n = localClient_Socket.SendTo(sendMessage, Point);
                    if (n > 1)
                        return true;

                    return false;
                }
                catch (Exception ex)
                {
                    ShowMsg("UDP发送失败:" + ex.Message);
                    Trace.WriteLine("UDP发送失败:" + ex.Message);

                    return false;
                }

            }

        }

        ///
        /// 同步发送
        private bool SendData(byte[] dataStr)
        {
            bool result = false;
            if (dataStr.Length < 0)
                return result;
            try
            {
                int n = localClient_Socket.Send(dataStr);

                if (n < 1)
                    result = false;
                else
                    result = true;
            }
            catch (Exception ex)
            {
                ShowMsg("TCP发送失败:" + ex.Message);
                Trace.WriteLine("TCP发送失败:" + ex.Message);
                result = false;
            }
            return result;
        }


        /// 检测socket的状态
        /// 
        private bool checkSocketState()
        {
            try
            {
                if (localClient_Socket == null)
                {
                    return socket_create_connect();
                }
                else if (!IsSocketConnected(localClient_Socket))
                {
                    ShowMsg("TCP socket失去连接,即将重连");
                    Trace.WriteLine("TCP socket失去连接,即将重连");
                    return Reconnect();
                }
                else
                {
                    return true;
                }

            }
            catch (SocketException ex)
            {
                ShowMsg("TCP checkSocketState 连接失败:" + ex.Message);
                Trace.WriteLine("TCP checkSocketState 连接失败:" + ex.Message);
                return false;
            }
        }


        private bool IsSocketConnected(Socket s)
        {

            //return !((s.Poll(-1, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
            return s.Connected;

        }

        public bool SendMsg(byte[] buffer)
        {
            try
            {
                if (socket_send(buffer))
                {
                    localSendCount += (UInt32)buffer.Length;
                    new Thread(() =>
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            tBk_sendCnt.Text = "已发送字节数：" + localSendCount.ToString();
                        }));
                    }).Start();

                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                ShowMsg("iNetClient数据发送失败：" + ex.Message);
                Trace.WriteLine("iNetClient数据发送失败：" + ex.Message);
                return false;
            }
        }

        public void closeClient()
        {
            try
            {

                if ((localClient_Socket != null) && (localClient_Socket.Connected))
                {
                    //关闭socket
                    localClient_Socket.Shutdown(SocketShutdown.Both);

                    localClient_Socket.Disconnect(true);
                    //IsconnectSuccess = false;

                    localClient_Socket.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("关闭socket失败:" + ex.Message);
                Trace.WriteLine("关闭socket失败:" + ex.Message);
            }
        }


        #endregion




        #region 文本显示
        public void ShowMsg(string str)
        {
            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {

                    tB_recbuf.AppendText(str + "\r\n");
                    tB_recbuf.ScrollToEnd();

                }));
            }).Start();

        }


        private StringBuilder builder = new StringBuilder(); //格式化串口接收缓冲
        public void ShowMsg(byte[] buf)
        {
            try
            {
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {

                        foreach (byte b in buf)
                        {
                            builder.Append(b.ToString("X2") + " ");

                        }
                        builder.Append('\n');
                        tB_recbuf.AppendText(builder.ToString());

                        tB_recbuf.ScrollToEnd();

                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("接收缓冲区错误:" + ex.Message);
                tB_recbuf.Clear();
            }
        }

        #endregion

    }
}