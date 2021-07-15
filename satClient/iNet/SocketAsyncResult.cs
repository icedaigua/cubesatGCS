using System;
using System.Threading;

namespace iNet
{
    /// <summary>
    /// Socket异步操作状态
    /// </summary>
    public class SocketAsyncResult : IAsyncResult
    {
        /// <summary>
        /// 获取用户自定义对象，它限定或包含关于异步操作的信息
        /// </summary>
        public object AsyncState { get; private set; }

        /// <summary>
        /// 获取用于等待异步操作完成的System.Threading.WaitHandle
        /// </summary>
        public WaitHandle AsyncWaitHandle { get; private set; }

        /// <summary>
        /// 获取一个值，指示异步操作是否同步完成
        /// </summary>
        public bool CompletedSynchronously { get; internal set; }

        /// <summary>
        /// 获取一个值，指示异步操作是否已经完成
        /// </summary>
        public bool IsCompleted { get; internal set; }

        /// <summary>
        /// 实例化Socket异步操作状态
        /// </summary>
        /// <param name="state"></param>
        public SocketAsyncResult(object state)
        {
            AsyncState = state;
            AsyncWaitHandle = new AutoResetEvent(false);
            CompletedSynchronously = false;
        }
    }
}
