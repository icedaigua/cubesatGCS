using System;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace iNet
{
    public class TcpClientOwn
    {

        #region Socket Client功能函数

        #region 参数
        private Socket localClient_Socket;

        private byte[] localDataRecv = new byte[1024];  //遥测接收缓冲区/

        private String PATTERN_IP = "(\\d*\\.){3}\\d*";


        private UInt32 localRecvCount = 0;  //接收计数
        //private UInt32 localSendCount = 0;  //发送计数 


        private IPHostEntry ipHost;
        private IPAddress ip;
        
        //端口号
        private IPEndPoint Point;

        //private string socket_Type = "TCP";

        //委托
        public delegate void iNetSocketDataArrival(byte[] data);
        public iNetSocketDataArrival socketDataArrival;//= socketDataArrivalHandler;  //这里不用static会报错,因此和此函数相关的函数和变量都要设置程static


        #endregion


        public TcpClientOwn(string iNet_IP, string iNet_NO)
        {
            localTcpClient_create(iNet_IP, iNet_NO);
        }
        /// <summary>
        /// 创建TCP Client
        /// </summary>       
        private void localTcpClient_create(string iNet_IP,string iNet_NO)
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
                //ShowMsg("网络连接启动错误:" + ex.Message);
                Trace.WriteLine("网络连接启动错误:" + ex.Message);
            }


        }


        ///
        /// 创建套接字+异步连接函数 
        private bool socket_create_connect()
        {
            try
            {
                //if (socket_Type == "TCP")
                //{
                    localClient_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    localClient_Socket.Connect(Point);
               // }
                //else
                //{
                //    localClient_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //    localClient_Socket.Bind(Point);
                //}

                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), localClient_Socket);

                //ShowMsg("连接本地服务器成功");
                Trace.WriteLine("连接本地服务器成功");

                return true;
            }
            catch (Exception ex)
            {
                //ShowMsg("连接" + socket_Type + "服务器失败:" + ex.Message);
                Trace.WriteLine("连接服务器失败:" + ex.Message + ex.StackTrace);
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

                    //if (showrec)
                    //    ShowMsg(recBuf);

                    localRecvCount += (UInt32)BytesRead;
                    //new Thread(() =>
                    //{
                    //    this.Dispatcher.Invoke(new Action(() =>
                    //    {
                    //        tBk_recCnt.Text = "已接收字节数：" + localRecvCount.ToString();
                    //    }));
                    //}).Start();

                    if (socketDataArrival != null)
                    {
                        socketDataArrival(recBuf);
                    }
                }

                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), localClient_Socket);
            }
            catch (Exception ex)
            {
                //ShowMsg("socket OnReceiveCallback错误:" + ex.Message);
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
                //ShowMsg("TCP socket开始重连");
                Trace.WriteLine("TCP socket开始重连");
                //关闭socket
                localClient_Socket.Shutdown(SocketShutdown.Both);
                localClient_Socket.Disconnect(true);
                localClient_Socket.Close();
            }
            catch (Exception ex)
            {
                //ShowMsg("关闭TCP失败:" + ex.Message);
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

            //if (socket_Type == "TCP")
            {
                if (checkSocketState())
                {
                    return SendData(sendMessage);
                }
                return false;
            }
            //else
            //{
            //    try
            //    {
            //        int n = localClient_Socket.SendTo(sendMessage, Point);
            //        if (n > 1)
            //            return true;

            //        return false;
            //    }
            //    catch (Exception ex)
            //    {
            //        //ShowMsg("UDP发送失败:" + ex.Message);
            //        Trace.WriteLine("UDP发送失败:" + ex.Message);

            //        return false;
            //    }

            //}

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
                //ShowMsg("TCP发送失败:" + ex.Message);
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
                    //ShowMsg("TCP socket失去连接,即将重连");
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
                //ShowMsg("TCP checkSocketState 连接失败:" + ex.Message);
                Trace.WriteLine("TCP checkSocketState 连接失败:" + ex.Message);
                return false;
            }
        }


        private bool IsSocketConnected(Socket s)
        {

            //return !((s.Poll(-1, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
            return s.Connected;

        }


        public void closeClient()
        {
            try
            {

                if ((localClient_Socket != null) && localClient_Socket.Connected)
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
                //ShowMsg("关闭socket失败:" + ex.Message);
                Trace.WriteLine("关闭socket失败:" + ex.Message);
            }
        }


        #endregion

    }
}
