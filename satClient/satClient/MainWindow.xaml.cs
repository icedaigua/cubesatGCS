using DataIO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniHelper;

namespace satClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 初始化
        public MainWindow()
        {
            InitializeComponent();

            LoggingInitz();
            UI_Initz();

            localClient_frm_init();     //本地网络设置初始化
            iNetClientInitz();


            hkView_Initz();
            myTimer_Initz();
        }

        /// <summary>
        /// 主窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            localClient_Close();
            iNetClient_frm_close(sender,null);

            myTimer_close();


        }

        /// <summary>
        /// 日志文件系统初始化
        /// </summary>
        private void LoggingInitz()
        {
            Trace.Listeners.Clear();  //清除系统监听器 (就是输出到Console的那个)

            Trace.Listeners.Add(new Logging.Logging(Directory.GetCurrentDirectory() + "\\Logging\\"));
        }

        private void UI_Initz()
        {

            double workHeight = SystemParameters.WorkArea.Height;
            double workWidth = SystemParameters.WorkArea.Width;

            this.Width = workWidth;
            this.Height = workHeight;

        }

        #endregion

        #region 按键响应
        private void FileMode_Click(object sender, RoutedEventArgs e)
        {

        }



        #endregion

        #region 本地Socket Client功能函数

        #region 参数

        private IOFuctions localClientIO = new IOFuctions();

        private bool localIsRunning = false;

        private BlockingQueue<byte[]> localRecvQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private BlockingQueue<byte[]> localSendQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private AutoResetEvent localWaitHandler = new AutoResetEvent(false);  //事件信号，线程同步：处理线程通知发送线程；发送线程处于阻塞状态；默认非终止，阻塞状态

        #endregion

        private void localClient_frm_init()
        {
            iNet_local_frm.OpenHandler += new iNet.iNetJLG.OpenClickEventHandler(localClient_create);
            iNet_local_frm.socketDataArrival = localTaskRecv;


            iNet_local_frm.setTextWidth(this.Width);

            iNet_local_frm.iNetInitz("本地网络设置", "192.168.1.58", "18001", "9001");

        }
        /// <summary>
        /// 创建TCP Client
        /// </summary>       
        private void localClient_create(object sender, RoutedEventArgs e)
        {

            localIsRunning = true;

            localClientIO.CreateFrameFile();


            Task.Factory.StartNew(localTaskProc);  //启动处理任务     
            iNet_local_frm.ShowMsg("数据处理任务启动成功");
            Trace.WriteLine("数据处理任务启动成功");
   
        }

        private void localClient_Close()
        {
            localIsRunning = false;

            localClientIO.CloseFile();

        }

        private void localTaskRecv(byte[] buffer)
        {
            try
            {
                Int32 iRecv = buffer.Length;

                if (iRecv > 0)
                {
                    iNet_local_frm.ShowMsg("接收线程运行中:" + iRecv.ToString());
                    byte[] recBuf = new byte[iRecv];    //遥测接收缓冲区
                    Array.Copy(buffer, recBuf, iRecv);  //从非托管内存拷贝到托管内存

                    localRecvQueue.Enqueue(recBuf);  //遥测接收数据入队

                    iNet_local_frm.ShowMsg(recBuf);
                    localClientIO.WriteFrameFile(recBuf);
                }

            }
            catch (Exception ex)
            {
                iNet_local_frm.ShowMsg("数据接收处理任务失败：" + ex.Message);
                Trace.WriteLine("数据接收处理任务失败：" + ex.Message);
            }

        }

        /// <summary>
        /// 执行处理任务，解析数据
        /// </summary>
        public void localTaskProc()
        {
            try
            {
                while (localIsRunning)
                {
                    if (!localRecvQueue.IsEmpty())  //遥测接收数据队列中存在待处理的数据
                    {
                        byte[] data = localRecvQueue.Dequeue();  //取出遥测接收数据队列中的数据
                        if (data != null)  //使用BlockingQueue可以不用判定，即此部可省略
                        {

                            //localJson.decodeBuf(data);  //数据解析处理

                            //byte[] localsendBuff = SeqToServer.generateSequence(data);

                            //localSendQueue.Enqueue(localsendBuff);

                            //this.Dispatcher.Invoke(new Action(() =>
                            //{
                            //    hk_view.hkInfo_AddNewView(localJson.dicForShow);
                            //}));

                            localWaitHandler.Set();  //将事件状态设为终止状态，允许等待线程继续执行
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);  //休息10ms
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("localSend Task Error:" + ex.Message);
            }
        }


        #endregion



        #region iNetClient

        private bool remoteIsRunning = false;

        private BlockingQueue<byte[]> remoteRecvQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private BlockingQueue<byte[]> remoteSendQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private AutoResetEvent remoteWaitHandler = new AutoResetEvent(false);  //事件信号，线程同步：处理线程通知发送线程；发送线程处于阻塞状态；默认非终止，阻塞状态

        private void iNetClientInitz()
        {
            iClientUDP_frm.iNetInitz("本地网络设置", "192.168.1.100", "2012");
            iClientUDP_frm.setTextWidth(this.Width);

            iClientTCP_frm.setTextWidth(this.Width);
            iClientTCP_frm.iNetInitz("远程网络设置", "3s.dkys.org", "10293");

            iNetClient_frm_init();
        }



        private void iNetClient_frm_init()
        {
            iClientTCP_frm.OpenHandler += new iNet.iNetClient.OpenClickEventHandler(iNetClient_frm_create);
            iClientTCP_frm.CloseHandler += new iNet.iNetClient.CloseClickEventHandler(iNetClient_frm_close);
            //iClientTCP_frm.SendHandler += new iNet.iNetSimple.SendClickEventHandler(remoteSendMsgClick);

            //iClientTCP_frm.socketDataArrival +=iNetClientTaskRecv;

            //iClientUDP_frm..OpenHandler += new iNet.iNetClient.OpenClickEventHandler(iNetClient_frm_create);
            iClientUDP_frm.socketDataArrival = iNetClientTaskRecv;
        }

        private void iNetClient_frm_close(object sender, RoutedEventArgs e)
        {
            remoteIsRunning = false;
        }

        private void iNetClient_frm_create(object sender, RoutedEventArgs e)
        {

            remoteIsRunning = true;

            Task.Factory.StartNew(iNetClientTaskProc);  //启动处理任务     
            iClientTCP_frm.ShowMsg("数据处理任务启动成功");
            Trace.WriteLine("数据处理任务启动成功");

            Task.Factory.StartNew(iNetClientTaskSend);  //启动发送任务
            iClientTCP_frm.ShowMsg("数据发送任务启动成功");
            Trace.WriteLine("数据发送任务启动成功");



        }

        private void iNetClientTaskRecv(byte[] buffer)
        {
            try
            {

                Int32 iRecv = buffer.Length;//remoteClient_Socket.Receive(remoteDataRecv);
                if (iRecv > 0)
                {
                    byte[] recBuf = new byte[iRecv];  //遥测接收缓冲区
                    Array.Copy(buffer, recBuf, iRecv);  //从非托管内存拷贝到托管内存
                    remoteRecvQueue.Enqueue(recBuf);  //遥测接收数据入队
                    iClientTCP_frm.ShowMsg("rec num is " + Encoding.ASCII.GetString(recBuf));
                }

            }
            catch (Exception ex)
            {
                iClientTCP_frm.ShowMsg("数据接收失败：" + ex.Message);
                Trace.WriteLine("数据接收失败：" + ex.Message);
            }

        }

        /// <summary>
        /// 执行处理任务，解析数据
        /// </summary>
        public void iNetClientTaskProc()
        {
            while (remoteIsRunning)
            {
                if (!remoteRecvQueue.IsEmpty())  //遥测接收数据队列中存在待处理的数据
                {
                    byte[] data = remoteRecvQueue.Dequeue();  //取出遥测接收数据队列中的数据
                    if (data != null)  //使用BlockingQueue可以不用判定，即此部可省略
                    {

                        //DataProc(data);  //数据解析处理

                        remoteWaitHandler.Set();  //将事件状态设为终止状态，允许等待线程继续执行
                    }
                }
                else
                {
                    Thread.Sleep(10);  //休息10ms
                }
            }
        }

        /// <summary>
        /// 执行发送任务,发送的是localSendQueue中的数据
        /// </summary>
        public void iNetClientTaskSend()
        {
            try
            {
                while (remoteIsRunning)  //控制任务运行的信号
                {
                    //localWaitHandler.WaitOne();  //阻塞当前线程，直到WaitHandler收到信号

                    if (!localSendQueue.IsEmpty())  //遥测接收数据队列中存在待处理的数据
                    {
                        byte[] data = localSendQueue.Dequeue();  //取出遥测接收数据队列中的数据
                        if (data != null)  //使用BlockingQueue可以不用判定，即此部可省略
                        {
                            //remoteSendMsg(data);
                            if (!iClientTCP_frm.SendMsg(data))  //发送失败，将数据重新放入发送队列中
                                localSendQueue.Enqueue(data);
                        }
                    }
                    else
                    {
                        iClientTCP_frm.SendMsg(iClientTCP_frm.testbuf3);
                    }

                    Thread.Sleep(1000);  //休息10ms
                }
            }
            catch (Exception ex)
            {
                iClientTCP_frm.ShowMsg("数据发送失败：" + ex.Message);
            }
        }


        #endregion

        private void hkView_Initz()
        {
            hk_view.hkInfo_Initz();
        }


        #region hk_view

        #endregion

        #region Timer


        private byte[] testbuf3 = new byte[184]
{
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


        private System.Timers.Timer myTimer = new System.Timers.Timer();
        private void myTimer_Initz()
        {
            //设置Timer，开始执行 
            myTimer.Interval = 1000;  //周期 毫秒

            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);

            myTimer.Enabled = true;
        }
        private void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    hk_view.hkInfo_AddNewView(testbuf3);
                }));
            }).Start();
            //Thread.Sleep(3000);
           

        }

        private void myTimer_close()
        {
            myTimer.Enabled = false;
        }
        #endregion
    }
}
