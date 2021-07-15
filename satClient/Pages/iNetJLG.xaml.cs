using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Pages
{
    /// <summary>
    /// iNetJLG.xaml 的交互逻辑
    /// </summary>
    public partial class iNetJLG : UserControl
    {

        #region 参数
       
        public string iNet_IP
        {
            get { return tB_IP.Text; }
        }


        public string iNet_Remote_NO
        {
            get { return tB_remote_No.Text; }
        }

        public string iNet_local_NO
        {
            get { return tB_local_No.Text; }
        }


        public byte[] testbuf = new byte[216]
              {
            0x80 , 0x5A , 0x5A , 0x04 , 0x03 , 0x02 , 0x01 , 0xA0, 0xA0 , 0xA0, 0xA0 , 0x11 , 0x01, 0x11 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00, 0x00, 0x00, 0x82, 0x1A, 0x50, 0xCE, 0xE3, 0x1D, 0xB8, 0x00,
            0x42, 0x01, 0xE1, 0x05, 0x05, 0x18, 0x00, 0x01 , 0x00 , 0x00 , 0x00, 0xC4 , 0x00 , 0x10 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xD9 , 0x84 , 0x55 , 0x7C , 0xF1 , 0x11 , 0x00 , 0x00 , 0x60 , 0x01 , 0x04 , 0x00 , 0x10 , 0x10 , 0x1C , 0x00 , 0x1D , 0x00,
            0x23, 0x00, 0x21, 0x00, 0x22, 0x00, 0x22, 0x00 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xC5 , 0x09 , 0xCC , 0x09 , 0xC8 , 0x09 , 0x45 , 0x01 , 0xD0 , 0x09 , 0xC5 , 0x09 , 0xFD , 0x01 , 0xB2 , 0x1C , 0x50 , 0x01 , 0x02 , 0x00 , 0x01 , 0x00 , 0x02 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00 , 0x01 , 0x00 , 0x70, 0x00 , 0x23 , 0x00 , 0x21 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00 , 0x00 , 0x00, 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xB8 , 0x06 , 0x00 , 0x00 , 0x00 , 0x80 , 0x02 , 0xE0 , 0x61 , 0x00 , 0xC2 , 0x00 , 0xC0 , 0x49 , 0x02 , 0x00 , 0x0C , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
              };

        #endregion


        public iNetJLG()
        {
            InitializeComponent();
        }

        public void iNetInitz(string header = "南理工本地网络设置", string ip = "192.168.1.58", string remoteport = "18001",string localport = "9001")
        {
            gB_iNet.Header = header;
            tB_IP.Text = ip;
            tB_local_No.Text = localport;
            tB_remote_No.Text = remoteport;

        }


        public void setTextWidth(double width)
        {
            tB_recbuf.Width = (width - 250) *2/ 3;
            tB_sendbuf.Width = (width - 250) / 3;

            tBk_recCnt.Width = (width - 250) *2/ 3;
            tBk_sendCnt.Width = (width - 250) / 3;
        }


        #region 按键响应
        public delegate void OpenClickEventHandler(object sender, RoutedEventArgs e);
        public delegate void CloseClickEventHandler(object sender, RoutedEventArgs e);
        //public delegate void SendClickEventHandler();

        public event OpenClickEventHandler OpenHandler;
        public event CloseClickEventHandler CloseHandler;
        //public event SendClickEventHandler SendHandler;

        private void btn_Net_open_Click(object sender, RoutedEventArgs e)
        {
           
            localUdpClient_create(sender, e);
 

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
                    SendMsg(testbuf);
                else
                    SendMsg(Encoding.Default.GetBytes(tB_sendbuf.Text + "\r\n"));
            }
            catch (Exception ex)
            {
                ShowMsg("连接下行服务器失败:" + ex.Message);
                Trace.WriteLine("连接下行服务器失败:" + ex.Message);
                return;
            }

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



        #region UDP相关

        //Server端口号
        private Socket localClient_Socket;    
        private IPEndPoint server_ep;

        private byte[] localDataRecv = new byte[1024];  //遥测接收缓冲区/
        private UInt32 localRecvCount = 0;  //接收计数
        private UInt32 localSendCount = 0;  //发送计数 
                                            //委托
        public delegate void iNetSocketDataArrival(byte[] data);
        public iNetSocketDataArrival socketDataArrival;//= socketDataArrivalHandler;  //这里不用static会报错,因此和此函数相关的函数和变量都要设置程static


        private void localUdpClient_create(object sender, RoutedEventArgs e)
        {

            try
            {
                server_ep = new IPEndPoint(IPAddress.Parse(iNet_IP), int.Parse(iNet_Remote_NO));

                //IP地址
                IPAddress client_ip = IPAddress.Any;
                //端口号
                IPEndPoint client_ep = new IPEndPoint(client_ip, int.Parse(iNet_local_NO));

                localClient_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                localClient_Socket.Bind(client_ep);

                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(localTaskRecv), localClient_Socket);

         
                ShowMsg("连接本地服务器成功");
                Trace.WriteLine("连接本地服务器成功");

            }
            catch (Exception ex)
            {
                ShowMsg("连接服务器失败:" + ex.Message);
                Trace.WriteLine("连接服务器失败:" + ex.Message);
                return;
            }

        }

        private void localTaskRecv(IAsyncResult ar)
        {
            try
            {             
                EndPoint recv = (EndPoint)server_ep;
                Int32 iRecv = localClient_Socket.ReceiveFrom(localDataRecv, ref recv);
               
                if (iRecv > 0)
                {
                    byte[] recBuf = new byte[iRecv];  //遥测接收缓冲区
                    Array.Copy(localDataRecv, recBuf, iRecv);  //从非托管内存拷贝到托管内存
                
                    if (showrec)
                        ShowMsg(recBuf);

                    localRecvCount += (UInt32)iRecv;
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
                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(localTaskRecv), localClient_Socket);
            }
            catch (Exception ex)
            {
                ShowMsg("数据接收失败：" + ex.Message);
                Trace.WriteLine("数据接收失败：" + ex.Message);
            }

        }
        #endregion




        public void SendMsg(byte[] buffer)
        {
            try
            {
               
                byte[] cmdBuf = new byte[3] { 0x01, 0x42, 0x02 };
                localClient_Socket.SendTo(cmdBuf, 0, cmdBuf.Length, SocketFlags.None, server_ep);

                localSendCount += (UInt32)buffer.Length;
                new Thread(() =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        tBk_sendCnt.Text = "已发送字节数：" + localSendCount.ToString();
                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                ShowMsg("iNetJLG数据发送失败：" + ex.Message);
                Trace.WriteLine("iNetJLG数据发送失败：" + ex.Message);
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
                    localClient_Socket.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMsg("关闭socket失败:" + ex.Message);
                Trace.WriteLine("关闭socket失败:" + ex.Message);
            }
        }


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
