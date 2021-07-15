using System;
using System.Net;

namespace iNet
{
    /// <summary>
    /// 基于TCP的Socket接口
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// 获取是否已经连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 异步断开连接
        /// </summary>
        void DisconnectAsync();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据内容</param>
        void Send(byte[] data);

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data">异步发送的数据内容</param>
        void SendAsync(byte[] data);

        /// <summary>
        /// 获取远程终结点地址。
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// 获取本地终结点地址。
        /// </summary>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// 断开连接完成时触发的事件
        /// </summary>
        event EventHandler<SocketEventArgs> DisconnectCompleted;

        /// <summary>
        /// 接收完成时触发的事件
        /// </summary>
        event EventHandler<SocketEventArgs> ReceiveCompleted;

        /// <summary>
        /// 发送完成时触发的事件
        /// </summary>
        event EventHandler<SocketEventArgs> SendCompleted;
    }
}
