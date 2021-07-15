using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace iNet
{
    public class UdpClientOwn
    {

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


        public void localUdpClient_create(string iNet_IP,string iNet_Remote_NO,string iNet_local_NO)
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


                //ShowMsg("连接本地服务器成功");
                Trace.WriteLine("连接本地服务器成功");

            }
            catch (Exception ex)
            {
                //ShowMsg("连接服务器失败:" + ex.Message);
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

                    //if (showrec)
                    //    ShowMsg(recBuf);

                    localRecvCount += (UInt32)iRecv;
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
                localClient_Socket.BeginReceive(localDataRecv, 0, localDataRecv.Length, SocketFlags.None, new AsyncCallback(localTaskRecv), localClient_Socket);
            }
            catch (Exception ex)
            {
                //ShowMsg("数据接收失败：" + ex.Message);
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
                //new Thread(() =>
                //{
                //    this.Dispatcher.Invoke(new Action(() =>
                //    {
                //        tBk_sendCnt.Text = "已发送字节数：" + localSendCount.ToString();
                //    }));
                //}).Start();
            }
            catch (Exception ex)
            {
                //ShowMsg("iNetJLG数据发送失败：" + ex.Message);
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
                //ShowMsg("关闭socket失败:" + ex.Message);
                Trace.WriteLine("关闭socket失败:" + ex.Message);
            }
        }
    }
}
