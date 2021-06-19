using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using System.Data.SQLite;
using System.Linq;

using Dapper;

using satCompent.Model;
using satCompent.Helper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace satCompent.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            Messenger.Default.Register<object>(this, "Define", HandleDefine);
            Messenger.Default.Register<bool>(this, "Delete", HandleDelete);
        }

        #region Override Method
        public override void Cleanup()
        {
            base.Cleanup();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Field
        private readonly string strConnection = "Data Source=db_dmds.db3;Password=1234";
        private ObservableCollection<NavigationModel> navigation;  //导航

        #endregion

        #region Property
        public ObservableCollection<NavigationModel> Navigation
        {
            get { return navigation; }
            set
            {
                navigation = value;
                RaisePropertyChanged("Navigation");
            }
        }
        #endregion

        #region Command
        private bool CanLoadedExecute()
        {
            return true;
        }
        private void LoadedExecute()
        {
            Navigation = new ObservableCollection<NavigationModel>();  //导航菜单初始化

            Configuration config = Configuration.Read(@"settings\login.json");
            if (config == null)
            {
                Trace.WriteLine("satcompent未找到初始配置文件,退出");
                return;
            }

            switch (config.GetInt("TaskId"))  //获取任务ID
            {
                case 0:  //NS2
                    LoadNavigation("select * from t_navi_ns2");
                    break;
                case 1:  //QH
                    LoadNavigation("qh_navigation.xml");
                    break;
                default:
                    break;
            }

            Messenger.Default.Send<string>("Loaded", "Main");  //页面内容初始化

            Task.Factory.StartNew(TaskRecv);  //启动接收任务:ZeroMQ
        }
        public ICommand LoadedCommand { get { return new RelayCommand(LoadedExecute, CanLoadedExecute); } }

        private bool CanClosedExecute()
        {
            return true;
        }
        private void ClosedExecute()
        {
            //Messenger.Default.Send<string>("closed", "main");
        }
        public ICommand ClosedCommand { get { return new RelayCommand(ClosedExecute, CanClosedExecute); } }

        private bool CanSetExecute()
        {
            return true;
        }
        private void SetExecute()
        {
            Messenger.Default.Send<string>("Set", "Main");
        }
        public ICommand SetCommand { get { return new RelayCommand(SetExecute, CanSetExecute); } }

        private bool CanNaviExecute(object obj)
        {
            return true;
        }
        private void NaviExecute(object obj)
        {
            Messenger.Default.Send<object>(obj, "Navi");
        }
        public ICommand NaviCommand { get { return new RelayCommand<object>(NaviExecute, CanNaviExecute); } }

        private bool CanDefineExecute(object obj)
        {
            return true;
        }
        private void DefineExecute(object obj)
        {
            if (obj != null)
            {
                SaveConfig((NavigationModel)obj);  //保存导航菜单
                Messenger.Default.Send<object>(obj, "Delete");
                Messenger.Default.Send<object>(obj, "Confirm");
            }
        }
        public ICommand DefineCommand { get { return new RelayCommand<object>(DefineExecute, CanDefineExecute); } }
        #endregion

        #region Messenger Handler
        private void HandleDefine(object obj)
        {
            NavigationModel model = (NavigationModel)obj;

            string strSql = @"insert into t_navi_ns2 (id, name, parent, define) VALUES (@Id, @Name, @Parent, @Define)";  //数据库插入语句

            NavigationModel defineNavi = new NavigationModel() { Id = 4, Name = "自定义", Parent = 0, Define = 0 };  //自定义导航
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                connection.Execute(strSql, model);  //将自定义节点插入数据库  

                List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //获取树形结构表：导航菜单
                defineNavi.Children = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //第2级导航菜单：自定义
            }
            Navigation.RemoveAt(3);
            Navigation.Add(defineNavi);

            Messenger.Default.Send<int[]>(new int[] { model.Id, model.Define }, "Define");  //初始化自定义Page
        }

        private void HandleDelete(bool isDelete)
        {
            Configuration config = Configuration.Read(@"navi\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return;

            int id = config.GetInt("Id");  //获取导航菜单Id

            string strSql = @"delete from t_navi_ns2 where id = @id";  //数据库删除语句

            NavigationModel defineNavi = new NavigationModel() { Id = 4, Name = "自定义", Parent = 0 };  //自定义导航
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                connection.Execute(strSql, new { id = id });  //将选中自定义节点从数据库中删除  

                List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //获取树形结构表：导航菜单
                defineNavi.Children = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //第2级导航菜单：自定义
            }
            Navigation.RemoveAt(3);
            Navigation.Add(defineNavi);
        }
        #endregion

        #region Method
        private void LoadNavigation(string strSql)
        {
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<NavigationModel> naviList = connection.Query<NavigationModel>(strSql).AsList<NavigationModel>();  //获取树形结构表：导航菜单

                List<NavigationModel> firstList = naviList.Where(x => x.Parent == 0).AsList<NavigationModel>();  //第1级导航菜单
                for (int i = 0; i < firstList.Count; i++)
                {
                    NavigationModel model = new NavigationModel() { Id = firstList[i].Id, Name = firstList[i].Name, Parent = firstList[i].Parent, Define = firstList[i].Define, Children = new List<NavigationModel>() };
                    List<NavigationModel> secondList = naviList.Where(x => x.Parent == firstList[i].Id).AsList<NavigationModel>();  //第2级导航菜单
                    for (int j = 0; j < secondList.Count; j++)
                    {
                        model.Children.Add(new NavigationModel() { Id = secondList[j].Id, Name = secondList[j].Name, Parent = secondList[j].Parent, Define = secondList[j].Define });
                    }
                    Navigation.Add(model);
                }
            }
        }


        /// <summary>
        /// 执行接收任务
        /// </summary>
        private void TaskRecv()
        {
            //using (var context = ZContext.Create())
            //{
            //    using (var socket = new ZSocket(context, ZSocketType.SUB))
            //    {
            //        socket.SubscribeAll();
            //        socket.Connect("tcp://localhost:2222");

            //        while (true)
            //        {
            //            ZFrame frame = socket.ReceiveFrame();
            //            RecvMsg recvMsg = RecvMsg.ParseFrom(frame.Read());
            //            switch (recvMsg.Type)
            //            {
            //                case "TMCODE":
            //                    Messenger.Default.Send<byte[]>(System.Text.Encoding.Unicode.GetBytes(recvMsg.GetS(0).Val), "TMCODE");  //显示遥测源码
            //                    break;
            //                case "TMRSLT":
            //                    //SetResult(recvMsg, ref msgOfResult, ref msgOfGps, ref msgOfAttitude, ref msgOfPanel, ref msgOfVolt, ref msgOfCurr, ref msgOfTemp);  //产生遥测结果，GPS,姿态角，电压，电流，温度
            //                    //Messenger.Default.Send<MsgOfResult>(msgOfResult, "TMRSLT");  //显示遥测结果
            //                    //Messenger.Default.Send<MsgOfGps>(msgOfGps, "TMGPS");  //显示GPS
            //                    //Messenger.Default.Send<MsgOfAttitude>(msgOfAttitude, "TMATTI");  //显示姿态角 
            //                    //Messenger.Default.Send<MsgOfPanel>(msgOfPanel, "TMPANEL");  //显示帆板 
            //                    //Messenger.Default.Send<MsgOfVolt>(msgOfVolt, "TMVOLT");  //显示电压 
            //                    //Messenger.Default.Send<MsgOfCurr>(msgOfCurr, "TMCURR");  //显示电流 
            //                    //Messenger.Default.Send<MsgOfTemp>(msgOfTemp, "TMTEMP");  //显示温度

            //                    Messenger.Default.Send<RecvMsg>(recvMsg, "TMRSLT");  //显示遥测结果
            //                    break;
            //                case "ORBS":
            //                    SetOrbit(recvMsg, ref msgOfOrbit);
            //                    Messenger.Default.Send<MsgOfOrbit>(msgOfOrbit, "ORBS");  //显示轨道
            //                    break;
            //                default:
            //                    break;
            //            }

            //            System.Threading.Thread.Sleep(10);  //休息10ms
            //        }
            //    }
            //}
        }

   
        #endregion

        #region 配置信息
        // 
        // 保存的配置信息如下：
        // 0. 导航菜单Id
        // 1. 导航菜单Name
        // 2. 导航菜单Parent
        //
        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveConfig(NavigationModel model)
        {
            Configuration config = new Configuration();
            if (config != null)
            {
                config.Set("Id", model.Id);  //保存导航菜单Id
                config.Set("Name", model.Name);  //保存导航菜单Name
                config.Set("Parent", model.Parent);  //保存导航菜单Parent
                Configuration.Save(config, @"navi\config.json");  //保存配置信息到磁盘中
                //Configuration.Save(config);  //保存配置信息到磁盘中
            }
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        private NavigationModel LoadConfig()
        {
            Configuration config = Configuration.Read(@"navi\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return null;

            NavigationModel model = new NavigationModel();
            model.Id = config.GetInt("Id");  //获取导航菜单Id
            model.Name = config.GetString("Name");  //获取导航菜单Name
            model.Parent = config.GetInt("Parent");  //获取导航菜单Parent

            return model;
        }
        #endregion

    }
}