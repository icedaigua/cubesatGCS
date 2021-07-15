using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace iNet
{
    /// <summary>
    /// TCP客户端
    /// </summary>
    public class TCPClient : SocketBase
    {
        public bool IsUseAuthenticate { get; set; }

        /// <summary>
        /// 实例化TCP客户端
        /// </summary>
        public TCPClient()
            : base(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), new SocketHandler())
        {
        }

        #region 连接
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="endpoint">服务器IP地址</param>
        public void Connect(IPEndPoint endpoint)
        {
            if (IsConnected)  //判断是否已经连接
                throw new InvalidOperationException("connected server");
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            lock (this)  //锁定自己，避免多线程同时操作
            {
                SocketAsyncState state = new SocketAsyncState();
                Socket.BeginConnect(endpoint, EndConnect, state).AsyncWaitHandle.WaitOne();  //Socket异步连接
                while (!state.Completed)  //等待异步处理全部完成
                    System.Threading.Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 异步连接服务器
        /// </summary>
        /// <param name="endpoint">服务器IP地址</param>
        public void ConnectAsync(IPEndPoint endpoint)
        {
            if (IsConnected)  //判断是否已经连接
                throw new InvalidOperationException("connected server");
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            lock (this)  //锁定自己，避免多线程同时操作
            {
                SocketAsyncState state = new SocketAsyncState();
                state.IsAsync = true;  //设置状态为异步
                Socket.BeginConnect(endpoint, EndConnect, state);  //Socket异步连接
            }
        }

        /// <summary>
        /// 异步连接完成
        /// </summary>
        /// <param name="iar">异步结果</param>
        private void EndConnect(IAsyncResult iar)
        {
            SocketAsyncState state = (SocketAsyncState)iar.AsyncState;
            try
            {
                Socket.EndConnect(iar);
            }
            catch  //出现异常
            {
                state.Completed = true;  //连接失败
                if (state.IsAsync && ConnectCompleted != null)  //判断是否异步，如果异步则触发异步完成事件
                    ConnectCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Connect));
                return;
            }

            Stream = new NetworkStream(Socket);  //连接成功，创建Socket网络流
            if (IsUseAuthenticate)  //
            {
                NegotiateStream negotiate = new NegotiateStream(Stream);
                negotiate.AuthenticateAsClient();
                while (!negotiate.IsMutuallyAuthenticated)
                    System.Threading.Thread.Sleep(10);
            }

            state.Completed = true;  //连接完成
            if (state.IsAsync && ConnectCompleted != null)
                ConnectCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Connect));

            Hander.BeginReceive(Stream, EndReceive, state);  //开始接收数据
        }
        #endregion

        /// <summary>
        /// 连接完成时触发的事件
        /// </summary>
        public event EventHandler<SocketEventArgs> ConnectCompleted;




    }
}
