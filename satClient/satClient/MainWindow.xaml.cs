using DataIO;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniHelper;
using satMsg;
using DataProcess;

namespace satClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 参数
        iNet.TCPClient satTcpClient;  //连接调制解调器服务器端的TCPclient
        private BlockingQueue<byte[]> RecvQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private ExcelHelper excelApp;   //保存数据至excel
        private string appCurrPath;     //当前运行路径
        private string satName = "田园一号";


        private Task mainTask;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private ManualResetEvent resetEvent = new ManualResetEvent(true); //用于Task暂停和恢复
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            LoggingInitz();
            UI_Initz();
            Messenger.Default.Register<string>(this, "MainNet", HandleMainNet);
            //hkView_Initz();
            createIOFolder();
            //satTimerInitz();
            TaskInitz();
        }

        #region Window相关
        private void Window_Closed(object sender, EventArgs e)
        {

            excelApp.Dispose();
            
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

        /// <summary>
        /// 创建保存数据文件
        /// </summary>
        public void createIOFolder()
        {
            appCurrPath = Directory.GetCurrentDirectory()+ "\\遥测数据存储\\"+satName;
            if (!System.IO.Directory.Exists(appCurrPath))//不存在就创建目录
            {
                Directory.CreateDirectory(appCurrPath);
            }
           excelApp = new ExcelHelper(appCurrPath);
        }

        #endregion

        #region MVVM Method

        public void HandleMainNet(string msg)
        {
            string[] info = msg.Split(',');
            switch(info[0])
            {
                case "Open":
                    //Messenger.Default.Send<string>(info[1] + info[2], "INET");
                    tcpConnect(info);
                    break;
                case "Close":
                    satTcpClient.Dispose();
                    break;
                case "Send":
                    tcpSendData(info[1]);
                    break;
                default:
                    break;
            }
            
        }


        private void tcpSendData(string msg)
        {
            try
            {
                //byte[] bytes = Encoding.ASCII.GetBytes(msg);
                satTcpClient.SendAsync(Encoding.ASCII.GetBytes(msg));
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<string>("网络连接启动错误:" + ex.Message, "INET");
                Trace.WriteLine("网络连接启动错误:" + ex.Message);
                satTcpClient.Dispose();
                return;
            }
        }
        private void tcpConnect(string[] msg)
        {
            IPEndPoint endPoint;
            //ip地址
            try
            {
                string iNet_IP = msg[1];
                string iNet_NO = msg[2];
                string PATTERN_IP = "(\\d*\\.){3}\\d*";
                IPHostEntry ipHost;
                IPAddress ip;
            
                Match match = Regex.Match(iNet_IP, PATTERN_IP); //判断是否IP地址
                if (match.Value == "")
                {
                    ipHost = Dns.GetHostEntry(iNet_IP);
                    //端口号
                    endPoint = new IPEndPoint(ipHost.AddressList[0], int.Parse(iNet_NO)); 
                }
                else
                {
                    //IP地址
                    ip = IPAddress.Parse(iNet_IP);
                    //端口号
                    endPoint = new IPEndPoint(ip, int.Parse(iNet_NO));
                }

                satTcpClient = new iNet.TCPClient();
                satTcpClient.Connect(endPoint);

                if(satTcpClient.IsConnected)
                {
                    Messenger.Default.Send<string>("网络连接成功","INET");
                }
                else
                {
                    Messenger.Default.Send<string>("网络连接失败，服务器不存在", "INET");
                    return;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<string>("网络连接启动错误:" + ex.Message, "INET");
                Trace.WriteLine("网络连接启动错误:" + ex.Message);
                return;
            }     
            satTcpClient.ReceiveCompleted += SatTcpClient_ReceiveCompleted;

        }

        private void SatTcpClient_ReceiveCompleted(object sender, iNet.SocketEventArgs e)
        {
            localRecvQueue.Enqueue(e.Data);
            StringBuilder builder = new StringBuilder(); //格式化接收
            foreach (byte b in e.Data)
            {
                builder.Append(b.ToString("X2") + " ");
            }
            builder.Append('\n');
            //string asciiString = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Messenger.Default.Send<string>(builder.ToString(), "INET");
            //satTcpClient.SendAsync(e.Data);

        }
        #endregion



        #region 本地Socket Client功能函数

        #region 参数

        //private IOFuctions localClientIO = new IOFuctions();

        private bool localIsRunning = false;

        private BlockingQueue<byte[]> localRecvQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private BlockingQueue<byte[]> localSendQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private AutoResetEvent localWaitHandler = new AutoResetEvent(false);  //事件信号，线程同步：处理线程通知发送线程；发送线程处于阻塞状态；默认非终止，阻塞状态

        #endregion

        private void localClient_frm_init()
        {
            iNet_local_frm.OpenHandler += new Pages.iNetClient.OpenClickEventHandler(localClient_create);
            iNet_local_frm.socketDataArrival = localTaskRecv;


            iNet_local_frm.setTextWidth(this.Width);

            iNet_local_frm.iNetInitz("本地网络设置", "192.168.1.114", "9889");

        }
        /// <summary>
        /// 创建TCP Client
        /// </summary>       
        private void localClient_create(object sender, RoutedEventArgs e)
        {

            localIsRunning = true;

            //localClientIO.CreateFrameFile();


            Task.Factory.StartNew(localTaskProc);  //启动处理任务     
            iNet_local_frm.ShowMsg("数据处理任务启动成功");
            Trace.WriteLine("数据处理任务启动成功");
   
        }

        private void localClient_Close()
        {
            localIsRunning = false;

            //localClientIO.CloseFile();

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
                    //localClientIO.WriteFrameFile(recBuf);
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

        private BlockingQueue<byte[]> remoteSendQueue = new BlockingQueue<byte[]>(10);  //遥测接收数据队列
        private AutoResetEvent remoteWaitHandler = new AutoResetEvent(false);  //事件信号，线程同步：处理线程通知发送线程；发送线程处于阻塞状态；默认非终止，阻塞状态

        private void iNetClientInitz()
        {
            //iClientUDP_frm.iNetInitz("本地网络设置", "192.168.1.100", "2012");
            //iClientUDP_frm.setTextWidth(this.Width);

            //iClientTCP_frm.setTextWidth(this.Width);
            //iClientTCP_frm.iNetInitz("远程网络设置", "3s.dkys.org", "10293");

            iNetClient_frm_init();
        }



        private void iNetClient_frm_init()
        {
            //iClientTCP_frm.OpenHandler += new Pages.iNetClient.OpenClickEventHandler(iNetClient_frm_create);
            //iClientTCP_frm.CloseHandler += new Pages.iNetClient.CloseClickEventHandler(iNetClient_frm_close);
            //iClientTCP_frm.SendHandler += new iNet.iNetSimple.SendClickEventHandler(remoteSendMsgClick);

            //iClientTCP_frm.socketDataArrival +=iNetClientTaskRecv;

            //iClientUDP_frm..OpenHandler += new iNet.iNetClient.OpenClickEventHandler(iNetClient_frm_create);
            //iClientUDP_frm.socketDataArrival = iNetClientTaskRecv;
        }

        private void iNetClient_frm_close(object sender, RoutedEventArgs e)
        {
            remoteIsRunning = false;
        }

        private void iNetClient_frm_create(object sender, RoutedEventArgs e)
        {

            remoteIsRunning = true;

            Task.Factory.StartNew(iNetClientTaskProc);  //启动处理任务     
            //iClientTCP_frm.ShowMsg("数据处理任务启动成功");
            Trace.WriteLine("数据处理任务启动成功");

            Task.Factory.StartNew(iNetClientTaskSend);  //启动发送任务
           // iClientTCP_frm.ShowMsg("数据发送任务启动成功");
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
    //                remoteRecvQueue.Enqueue(recBuf);  //遥测接收数据入队
                    //iClientTCP_frm.ShowMsg("rec num is " + Encoding.ASCII.GetString(recBuf));
                }

            }
            catch (Exception ex)
            {
                //iClientTCP_frm.ShowMsg("数据接收失败：" + ex.Message);
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
                //if (!remoteRecvQueue.IsEmpty())  //遥测接收数据队列中存在待处理的数据
                //{
                //    byte[] data = remoteRecvQueue.Dequeue();  //取出遥测接收数据队列中的数据
                //    if (data != null)  //使用BlockingQueue可以不用判定，即此部可省略
                //    {

                //        //DataProc(data);  //数据解析处理

                //        remoteWaitHandler.Set();  //将事件状态设为终止状态，允许等待线程继续执行
                //    }
                //}
                //else
                //{
                //    Thread.Sleep(10);  //休息10ms
                //}
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
                            //if (!iClientTCP_frm.SendMsg(data))  //发送失败，将数据重新放入发送队列中
                            //    localSendQueue.Enqueue(data);
                        }
                    }
                    else
                    {
                        //iClientTCP_frm.SendMsg(iClientTCP_frm.testbuf3);
                    }

                    Thread.Sleep(1000);  //休息10ms
                }
            }
            catch
            {
                //iClientTCP_frm.ShowMsg("数据发送失败：" + ex.Message);
            }
        }


        #endregion


        #region Task相关


        private void TaskInitz()
        {
           
        }
        /// <summary>
        /// 执行处理任务，解析数据
        /// </summary>
        private int TaskProc(CancellationToken token)
        {
            while(true)
            {
                if (token.IsCancellationRequested)
                {
                    return -1;
                }

                // 初始化为true时执行WaitOne不阻塞
                resetEvent.WaitOne();
                //if (localRecvQueue.IsEmpty()) return;
                //localRecvQueue.Dequeue();

                //excelTest();
            }
        }
   
        #endregion
        
        
        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            mainTask = new Task<int>(() => TaskProc(tokenSource.Token), tokenSource.Token);
            mainTask.Start();
        }

        private void btn_test2_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = excelApp.ExcelToDataTable("姿控");
            //resetEvent.Reset();
        }

        private void btn_test3_Click(object sender, RoutedEventArgs e)
        {
            //resetEvent.Set();
            //excelTest();

            //tokenSource.Cancel();

            DataTable dt = new DataTable("fffff");

            string str =  dt.TableName;

        }

        private void excelTest()
        {
            //创建一个表
            System.Data.DataTable dt = new System.Data.DataTable("Product");
            System.Data.DataColumn dc = null;

            //添加列，赋值
            dc = dt.Columns.Add("id", Type.GetType("System.Int32"));
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            dc.AllowDBNull = false;
            dt.Columns.Add("pname", Type.GetType("System.String"));
            dt.Columns.Add("price", Type.GetType("System.Double"));

            System.Data.DataRow dr = dt.NewRow();
            dr["pname"] = "red apple";
            dr["price"] = 9.9;

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["pname"] = "black apple";
            dr["price"] = 19.9;  
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["pname"] = "gold apple";
            dr["price"] = 29.9;
            dt.Rows.Add(dr);

            excelApp.DataTableToExcel(dt, "姿控");
        }

        private void btn_test4_Click(object sender, RoutedEventArgs e)
        {

            //TTCHeader tth = new TTCHeader();

            //tth.uiData0 = 0x5581;

            //tth.uiData1 = 0x00000080;

            //MessageBox.Show("ver = :"+tth.getVERSION().ToString("X2"));
            //MessageBox.Show("scid = :" + tth.getSCID().ToString("X2"));
            //MessageBox.Show("vcid = :" + tth.getVCID().ToString("X2"));

            frameTest();

        }
        

        SATFRAMEOBC fobc ;
        SATFRAMEADCS fadcs;

        RecvMsgParse recmsgParse;

        private void frameTest()
        {
             ushort obcapid = 0x0000, adcsapid = 0x0C00;
             byte scid = 0x54;
             fobc = new SATFRAMEOBC(scid, obcapid);
             fadcs = new SATFRAMEADCS(scid, adcsapid);

            // TTCHeader tth = new TTCHeader();
            //tth.uiData0 = 0x5581;
            //tth.uiData1 = 0x00000080;

            //MessageBox.Show("scid = :" + fobc.tth.getSCID().ToString("X2"));
            //MessageBox.Show("obcapid = :" + fobc.epdu.getAPID().ToString("X2"));
            //MessageBox.Show("adcsapid = :" + fadcs.epdu.getAPID().ToString("X2"));

            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.tth).ToString(""));
            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.mpdu).ToString(""));
            //MessageBox.Show("length = :" + System.Runtime.InteropServices.Marshal.SizeOf(fobc.epdu).ToString(""));

            recmsgParse = new RecvMsgParse(Directory.GetCurrentDirectory() + "\\settings\\tianyuan-1.json");

            recmsgParse.getHouseKeepingPackage(createOBCFrame());
            //createOBCFrame();
            createADCSFrame();

        }
        public ushort recCNT = 2;
        public ushort downCNT = 3;
        private byte[] createOBCFrame()
        {

            fobc.pl.soft_id = 0x55;                            //1
            fobc.pl.reboot_count = 0x01;                     //2

            fobc.pl.rec_cmd_count = recCNT;                    //2
            fobc.pl.down_count = downCNT;                       //2

            fobc.pl.last_reset_time = 0;                  //4
            fobc.pl.work_mode = 0x10;                          //1

            fobc.pl.utc_time = UniFunction.xDateSeconds(DateTime.UtcNow);                         //4
            fobc.pl.temp_hk = -27;                           //2

            fobc.pl.on_off_status = 0xAA55AA55;                            //4
            fobc.pl.batt_TEMP1 = 27;                               //2


            
            byte[] bval =  UniSerialize.StructToByte((PlatForm)fobc.pl);

            string str = Encoding.ASCII.GetString(bval);
            //string str = "";
            //foreach(byte b in bval)
            //{
            //    str += "0x" + b.ToString("X2") + "\t";
            //}
            //str += "\n";
            //Trace.WriteLine("", "");
            //Trace.WriteLine("", str);
            //Trace.WriteLine("", "");
            return bval;
        }

        private byte[] createADCSFrame()
        {

            fadcs.adcs.velox = (float)1.2;                            //1
            fadcs.adcs.posix = (float)123.45;                     //2

            return UniSerialize.StructToByte((SATFRAMEADCS)fadcs);
        }
    }
}
