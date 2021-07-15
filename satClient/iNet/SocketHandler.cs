using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace iNet
{ 
    /// <summary>
    /// Socket处理程序
    /// </summary>
    public class SocketHandler : ISocketHandler
    {
        /// <summary>
        /// 异步处理关系集合
        /// </summary>
        private Dictionary<IAsyncResult, SocketHandlerState> StateSet;
        /// <summary>
        /// 发送队列
        /// </summary>
        private List<SocketHandlerState> SendQueue;

        /// <summary>
        /// 实例化SocketHandler
        /// </summary>
        public SocketHandler()
        {
            StateSet = new Dictionary<IAsyncResult, SocketHandlerState>();
            SendQueue = new List<SocketHandlerState>();
        }

        /// <summary>
        /// 开始接收
        /// </summary>
        /// <param name="stream">Socket网络流</param>
        /// <param name="callback">回调函数</param>
        /// <param name="state">自定义状态</param>
        /// <returns>异步结果</returns>
        public IAsyncResult BeginReceive(Stream stream, AsyncCallback callback, object state)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream can not read");
            if (callback == null)
                throw new ArgumentNullException("callback");

            SocketAsyncResult result = new SocketAsyncResult(state);
            SocketHandlerState shs = new SocketHandlerState();  //初始化SocketHandlerState
            shs.Data = new byte[1024];
            shs.Stream = stream;
            shs.Completed = true;
            shs.AsyncResult = result;
            shs.AsyncCallBack = callback;

            //开始异步接收数据，包含接收的数据长度
            try
            {
                stream.BeginRead(shs.Data, 0, 1024, EndRead, shs);
            }
            catch
            {
                result.CompletedSynchronously = true;
                shs.Data = new byte[0];
                shs.DataLength = 0;
                shs.Completed = false;
                lock (StateSet)
                    StateSet.Add(result, shs);
                ((AutoResetEvent)result.AsyncWaitHandle).WaitOne();
                callback(result);
            }

            return result;
        }

        /// <summary>
        /// 异步读取结束
        /// </summary>
        /// <param name="iar">异步结果</param>
        private void EndRead(IAsyncResult iar)
        {
            SocketHandlerState state = (SocketHandlerState)iar.AsyncState;
            try
            {
                state.DataLength = state.Stream.EndRead(iar);
            }
            catch
            {
                state.DataLength = 0;
            }

            if (state.DataLength == 0)  //表示Socket断开连接
            {
                lock (StateSet)
                    StateSet.Add(state.AsyncResult, state);
                state.Data = new byte[0];  //设定接收的数据为空
                state.DataLength = 0;  //设定接收的数据长度为0
                ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
                state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数
                return;
            }

            if (state.Completed)  //如果已经完成状态，开始下次接收
            {
                lock (StateSet)
                    StateSet.Add(state.AsyncResult, state);
                ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
                state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数

                try
                {
                    state.Stream.BeginRead(state.Data, 0, 1024, EndRead, state);  //继续接收数据
                }
                catch  //出现Socket异常
                {
                    lock (StateSet)
                        StateSet.Add(state.AsyncResult, state);
                    state.Data = new byte[0];  //设定接收的数据为空
                    state.DataLength = 0;  //设定接收的数据长度为0
                    ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
                    state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数
                }
            }
        }

        /// <summary>
        /// 结束接收
        /// </summary>
        /// <param name="iar">异步结果</param>
        /// <returns>接收的数据</returns>
        public byte[] EndReceive(IAsyncResult iar)
        {
            SocketHandlerState state = null;
            lock (StateSet)
            {
                if (!StateSet.ContainsKey(iar))  //判断异步操作状态是否属于当前处理程序
                    throw new ArgumentException("aysncresult not contain");
                state = StateSet[iar];
                StateSet.Remove(iar);

                byte[] recv = new byte[state.DataLength];
                Array.Copy(state.Data, recv, state.DataLength);

                return recv;
            }
        }

        /// <summary>
        /// 开始发送
        /// </summary>
        /// <param name="data">发送的数据内容</param>
        /// <param name="offset">数据偏移</param>
        /// <param name="count">数据长度</param>
        /// <param name="stream">Socket网络流</param>
        /// <param name="callback">回调函数</param>
        /// <param name="state">自定义状态</param>
        /// <returns>异步结果</returns>
        public IAsyncResult BeginSend(byte[] data, int offset, int count, Stream stream, AsyncCallback callback, object state)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > data.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count <= 0 || count > data.Length - offset || count > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("count");
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("stream can not write");
            if (callback == null)
                throw new ArgumentNullException("callback");

            SocketAsyncResult result = new SocketAsyncResult(state);
            SocketHandlerState shs = new SocketHandlerState();  //初始化SocketHandlerState
            shs.Data = data;
            shs.DataLength = 0;
            shs.Stream = stream;
            shs.Completed = true;
            shs.AsyncResult = result;
            shs.AsyncCallBack = callback;

            lock (SendQueue)  //锁定SendQueue,避免多线程同时发送数据
            {
                SendQueue.Add(shs);  //添加状态
                if (SendQueue.Count > 1)  //表示尚有数据未发送完成
                    return result;
            }

            try
            {
                stream.BeginWrite(shs.Data, 0, shs.Data.Length, EndWrite, shs);  //开始异步发送数据
            }
            catch
            {
                result.CompletedSynchronously = true;
                shs.Completed = false;
                lock (StateSet)
                    StateSet.Add(result, shs);
                ((AutoResetEvent)result.AsyncWaitHandle).Set();
                callback(result);
            }

            return result;
        }

        /// <summary>
        /// 异步写入结束
        /// </summary>
        /// <param name="iar">异步结果</param>
        private void EndWrite(IAsyncResult iar)
        {
            SocketHandlerState state = (SocketHandlerState)iar.AsyncState;
            
            lock (StateSet)  //锁定StateSet
                StateSet.Add(state.AsyncResult, state);

            try
            {
                state.Stream.EndWrite(iar);
            }
            catch  //出现Socket异常
            {
                state.Completed = false;  //发送失败
                ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
                state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数
                return;
            }

            state.Completed = true;  //发送成功
            ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
            state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数

            lock (SendQueue)  //锁定SendQueue
            {
                SocketHandlerState prepare = null;
                SendQueue.Remove(state);  //移除当前发送完成的数据
                if (SendQueue.Count > 0)  //如果SendQueue还有数据待发送，则继续发送
                    prepare = SendQueue[0];
                if (prepare != null)
                {
                    try
                    {
                        prepare.Stream.BeginWrite(prepare.Data, 0, prepare.Data.Length, EndWrite, prepare).AsyncWaitHandle.WaitOne();  //开始异步发送数据
                    }
                    catch
                    {
                        state.Completed = false;  //发送失败
                        ((AutoResetEvent)state.AsyncResult.AsyncWaitHandle).Set();  //允许等待线程继续
                        state.AsyncCallBack(state.AsyncResult);  //执行异步回调函数
                    }
                }
            }
        }

        /// <summary>
        /// 结束发送
        /// </summary>
        /// <param name="iar">异步结果</param>
        /// <returns>是否发送成功</returns>
        public bool EndSend(IAsyncResult iar)
        {
            SocketHandlerState state = null;
            lock (StateSet)
            {
                if (!StateSet.ContainsKey(iar))  //判断异步操作状态是否属于当前处理程序
                    throw new ArgumentException("aysncresult not contain");
                state = StateSet[iar];
                StateSet.Remove(iar);
            }

            return state.Completed;
        }
    }

    /// <summary>
    /// Socket处理状态
    /// </summary>
    internal class SocketHandlerState
    {
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Socket网络流
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// 异步结果
        /// </summary>
        public IAsyncResult AsyncResult { get; set; }

        /// <summary>
        /// 异步回调函数
        /// </summary>
        public AsyncCallback AsyncCallBack { get; set; }
    }
}
