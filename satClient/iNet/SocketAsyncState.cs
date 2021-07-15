namespace iNet
{
    internal class SocketAsyncState
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
        /// 是否异步
        /// </summary>
        public bool IsAsync { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Completed { get; set; }
    }
}
