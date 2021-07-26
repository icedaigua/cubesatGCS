using DataIO;
using DataProcess;
using GalaSoft.MvvmLight.Messaging;
using satClient.Model;
using satMsg;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniHelper;

using satCompent.View;
using System.Windows.Controls;
using System.Data;

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

        private iNetView pginet = null;  //inet
        private ResultView pgResult = null;  //参数
        private DataGridView pggrid = null;  //参数
        private CurrView pgcurr = null;
        private CurveView pgcurve = null;
        private OBCDataGridView pgobcgrid = null;
        private ADCSDataGridView pgadcsgrid = null;


        #endregion


        public MainWindow()
        {
            InitializeComponent();
            LoggingInitz();
            UI_Initz();
            Messenger.Default.Register<string>(this, "MainNet", HandleMainNet);
            Messenger.Default.Register<string>(this, "Main", HandleMain);
            Messenger.Default.Register<object>(this, "Navi", HandleNavi);
         
            createIOFolder();
            //satTimerInitz();
            TaskInitz();
        }

        #region Window相关

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

            this.WindowState = WindowState.Maximized;
           // double workHeight = SystemParameters.WorkArea.Height;
           // double workWidth = SystemParameters.WorkArea.Width;

           // this.Width = workWidth;
           // this.Height = workHeight;
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


        private void HandleMain(string info)
        {
            if ("Loaded".Equals(info))
            {
                pginet = new iNetView();
                pgResult = new ResultView();
                pggrid = new DataGridView();
                pgcurr = new CurrView();//new CurrView();
                pgcurve = new CurveView(0,0);
                pgobcgrid = new OBCDataGridView();
                pgadcsgrid = new ADCSDataGridView();
            }
            else if ("Closed".Equals(info))
            {
                excelApp.Dispose();
            }
            else if ("Set".Equals(info))
            {
               
            }
            else
            {
            }
        }

        private void HandleNavi(object obj)
        {
            NavigationModel navigation = (NavigationModel)obj;

 
            if (navigation.Parent == 0)  //第1级导航菜单
            {
               
            }
            else if (navigation.Parent == 1)  //第2级导航菜单：遥测
            {
                switch (navigation.Id)
                {
                    case 11:
                        this.content.Content = new Frame() { Content = pgResult };
                        break;
                    case 12:
                        this.content.Content = new Frame() { Content = pgcurr };
                        break;
                    case 13:
                
                        break;
                    default:
                        break;
                }
            }
            else if (navigation.Parent == 2)  //第2级导航菜单：遥控
            {
                switch (navigation.Id)
                {
                    case 21:
                        this.content.Content = new Frame() { Content = pgcurr };
                        break;
                    case 22:
                        this.content.Content = new Frame() { Content = pgcurve };
                        break;
                    default:
                        break;
                }
            }
            else if (navigation.Parent == 3)  //第2级导航菜单：自定义
            {
                switch (navigation.Id)
                {
                    case 121:
                        //this.content.Content = new Frame() { Content = pgCmd };
                        break;
                    default:
                        break;
                }
            }
            else if (navigation.Parent == 4)  //第2级导航菜单：原始
            {
                switch (navigation.Id)
                {
                    case 41:
                        this.content.Content = new Frame() { Content = pggrid };
                        break;
                    case 42:
                        this.content.Content = new Frame() { Content = pgobcgrid };
                        //this.content.Content = pgobcgrid;
                        break;
                    case 43:
                        this.content.Content = new Frame() { Content = pgadcsgrid };
                        break;
                    case 44:

                        break;
                    default:
                        break;
                }
            }
            else
            {
            }
        }

        #endregion

        #region TCP相关

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
            IPEndPoint endPoint;  //ip地址       
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
            RecvQueue.Enqueue(e.Data);
            StringBuilder builder = new StringBuilder(); //格式化接收
            foreach (byte b in e.Data)
            {
                builder.Append(b.ToString("X2") + " ");
            }
            builder.Append('\n');
            //string asciiString = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            Messenger.Default.Send<string>(builder.ToString(), "INET");
        }
        #endregion

        #region Task相关


        private void TaskInitz()
        {
            //创建和启动任务
            //mainTask = new Task<int>(() => TaskProc(tokenSource.Token), tokenSource.Token);
            //mainTask.Start();

            //恢复任务
            //resetEvent.Reset();   

            //暂停任务
            //resetEvent.Set();

            //取消任务
            //tokenSource.Cancel();
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

                Task.Delay(1000);
                frameTest();
                //Task.WaitAny();
                //localRecvQueue.Dequeue();
             
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
           
        }

        private void btn_test3_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_test4_Click(object sender, RoutedEventArgs e)
        {
            frameTest();
        }
        private void btn_iNet_setting(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Frame() { Content = pginet };
        }

        RecvMsgParse recmsgParse;

        TianYuanMsg tymsg = new TianYuanMsg();

        private void frameTest()
        {

            recmsgParse = new RecvMsgParse(Directory.GetCurrentDirectory() + "\\settings\\tianyuan-1.json");

            excelApp.createNewExcel(recmsgParse.getHouseKeepingPackageHeader());

            try
            {

                DataTable dt0 = recmsgParse.ParseMessage(tymsg.createOBCFrame());
                Messenger.Default.Send<DataTable>(dt0, "Result");
                //Messenger.Default.Send<DataTable>(dt0, "ADCSGrid");

                excelApp.DataTableToExcel(dt0);
                excelApp.DataTableToExcel(recmsgParse.originDataToDataTable());

                DataTable dt = recmsgParse.originDataToDataTable();

                //Messenger.Default.Send<DataTable>(recmsgParse.originDataToDataTable(), "DataGrid");

            }
            catch(Exception ex)
            {
                Trace.WriteLine("frame test错误" + ex.Message);
            }

        }

       
    }
}
