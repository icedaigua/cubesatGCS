using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace iNet
{
    /// <summary>
    /// Socket基类
    /// </summary>
    public class SocketBase : ISocket, IDisposable
    {
        /// <summary>
        /// Socket
        /// </summary>
        protected Socket Socket { get; set; }
        /// <summary>
        /// Stream
        /// </summary>
        protected Stream Stream { get; set; }

        /// <summary>
        /// Handler
        /// </summary>
        public ISocketHandler Hander { get; set; }

        /// <summary>
        /// 获取远程终结点地址。
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            //get
            //{
            //    if (IsConnected)
            //        return (IPEndPoint)Socket.RemoteEndPoint;
            //    return null;
            //}
            get
            {
                return (IPEndPoint)Socket.RemoteEndPoint;
            }
        }

        /// <summary>
        /// 获取本地终结点地址。
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            //get
            //{
            //    if (IsConnected)
            //        return (IPEndPoint)Socket.LocalEndPoint;
            //    return null;
            //}
            get
            {
                return (IPEndPoint)Socket.LocalEndPoint;
            }
        }

        /// <summary>
        /// 实例化SocketBase
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="handler">Hander</param>
        public SocketBase(Socket socket, ISocketHandler handler)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");
            if (handler == null)
                throw new ArgumentNullException("handler");

            Socket = socket;
            Socket.NoDelay = true;
            Hander = handler;
        }

        /// <summary>
        /// 获取是否已经连接
        /// </summary>
        public bool IsConnected { get { return Socket.Connected; } }

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected)  //判断是否已经连接
                throw new SocketException(10057);

            lock (this)
            {
                try
                {
                    Socket.BeginDisconnect(false, EndDisconnect, true).AsyncWaitHandle.WaitOne();  //Socket同步断开并等待完成
                }
                catch
                { 
                }
            }
        }

        /// <summary>
        /// 异步断开连接
        /// </summary>
        public void DisconnectAsync()
        {
            if (!IsConnected)  //判断是否已经连接
                throw new SocketException(10057);
            
            lock (this)
            {
                Socket.BeginDisconnect(false, EndDisconnect, false );  //Socket异步断开
            }
        }

        private void EndDisconnect(IAsyncResult iar)
        {
            try
            {
                Socket.EndDisconnect(iar);
                Socket.Close();
            }
            catch
            {
            }
            
            bool sync = Convert.ToBoolean(iar.AsyncState);  //是否同步
            if (!sync && DisconnectCompleted != null)
                DisconnectCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Disconnect));
        }

        /// <summary>
        /// 为收发异常准备的断开连接触发事件
        /// </summary>
        /// <param name="raise"></param>
        private void Disconnected(bool raise)
        {
            if (raise && DisconnectCompleted != null)
                DisconnectCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Disconnect));
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据内容</param>
        public void Send(byte[] data)
        {
            if (!IsConnected)  //判断是否已经连接
                throw new SocketException(10057);
            if (data == null)  //发送数据为null
                throw new ArgumentNullException("data null");
            if (data.Length == 0)  //发送数据长度为0
                throw new ArgumentException("data length = 0");

            SocketAsyncState state = new SocketAsyncState();  //设置异步状态
            state.Data = data;
            state.IsAsync = false;

            try
            {
                Hander.BeginSend(data, 0, data.Length, Stream, EndSend, state).AsyncWaitHandle.WaitOne();  //开始发送数据
            }
            catch
            {
                Disconnected(true);  //发送异常，则断开Socket连接
            }
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data">异步发送的数据内容</param>
        public void SendAsync(byte[] data)
        {
            if (!IsConnected)  //判断是否已经连接
                throw new SocketException(10057);
            if (data == null)  //发送数据为null
                throw new ArgumentNullException("data null");
            if (data.Length == 0)  //发送数据长度为0
                throw new ArgumentException("data length = 0");

            SocketAsyncState state = new SocketAsyncState();  //设置异步状态
            state.Data = data;
            state.IsAsync = true;

            try
            {
                Hander.BeginSend(data, 0, data.Length, Stream, EndSend, state);  //开始发送数据
            }
            catch
            {
                Disconnected(true);  //发送异常，则断开Socket连接
            }
        }

        private void EndSend(IAsyncResult iar)
        {
            SocketAsyncState state = (SocketAsyncState)iar.AsyncState;
            state.Completed = Hander.EndSend(iar);  //是否完成
            if (!state.Completed)  //如果没有完成，则断开Socket连接
                Disconnected(true);

            if (state.IsAsync && SendCompleted != null)  //发送完成事件
                SendCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Send) { Data = state.Data });
        }
        #endregion

        #region 接收数据
        protected void EndReceive(IAsyncResult iar)
        {
            SocketAsyncState state = (SocketAsyncState)iar.AsyncState;
            byte[] data = Hander.EndReceive(iar);  //接收的数据
            if (data.Length == 0)  //如果接收数据长度为0，则断开连接
            {
                Disconnected(true);
                return;
            }

            Hander.BeginReceive(Stream, EndReceive, state);  //再次开始接收数据
            if (ReceiveCompleted != null)  //接收完成事件
                ReceiveCompleted(this, new SocketEventArgs(this, SocketAsyncOperation.Receive) { Data = data });
        }
        #endregion

        /// <summary>
        /// 断开连接完成时触发的事件
        /// </summary>
        public event EventHandler<SocketEventArgs> DisconnectCompleted;

        /// <summary>
        /// 接收完成时触发的事件
        /// </summary>
        public event EventHandler<SocketEventArgs> ReceiveCompleted;

        /// <summary>
        /// 发送完成时触发的事件
        /// </summary>
        public event EventHandler<SocketEventArgs> SendCompleted;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (IsConnected)
                    Socket.Disconnect(false);
                Socket.Close();
            }
        }

        
    }
}
