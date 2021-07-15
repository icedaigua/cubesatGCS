using Dapper;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using satCompent.Helper;
using satCompent.Model;
using satCompent.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Controls;


using System.Diagnostics;

namespace satCompent.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl
    {

        #region Page

        //private CodeView pgCode = null;  //源码
        //private ResultView pgResult = null;  //参数
        //private GpsView pgGps = null;  //GPS
        //private AttitudeView pgAttitude = null;  //姿态角
        //private PanelView pgPanel = null;  //帆板
        //private VoltView pgVolt = null;  //电压
        //private CurrView pgCurr = null;  //电流
        //private TempView pgTemp = null;  //温度
        //private SchematicView pgSchematic = null;  //电路图
         private OrbitView pgOrbit = null;  //轨道
        //private StationView pgStation = null;  //测站
        //private CmdView pgCmd = null;  //遥控

        private Dictionary<int, Page> pgDefine = new Dictionary<int, Page>();  //自定义

        #endregion

        #region Field
        private readonly string strConnection = "Data Source=db_dmds.db3;Password=1234";
        private List<string> pageList = new List<string>();  //树形导航页面
        private string currentPage = string.Empty;  //当前页面
        #endregion

        public MainView()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
            Messenger.Default.Register<string>(this, "Alert", HandleAlert);
            Messenger.Default.Register<object>(this, "Confirm", HandleConfrim);
            Messenger.Default.Register<string>(this, "Main", HandleMain);
            Messenger.Default.Register<object>(this, "Navi", HandleNavi);
            Messenger.Default.Register<object>(this, "Delete", HandleDelete);
            Messenger.Default.Register<int[]>(this, "Define", HandleDefine);
            Messenger.Default.Register<int>(this, "Curve", HandleCurve);

            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        #region Messenger Handler
        private void HandleAlert(string info)
        {
            //this.ShowMessageAsync("提示", info);
            //GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync(new Action(() => { this.ShowMessageAsync("提示", info); }));
        }

        private void HandleConfrim(object obj)
        {
            NavigationModel navigation = (NavigationModel)obj;
            if (navigation.Parent == 4)  //第2级导航菜单：自定义
            {
                MessageDialogResult result = new MessageDialogResult();
                //MessageDialogResult result = this.ShowModalMessageExternal(this.Title, "确定要删除吗?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)  //确认删除
                {
                    if (pgDefine.Keys.Contains(navigation.Id))
                    {
                        pgDefine.Remove(navigation.Id);
                    }

                    Messenger.Default.Send<bool>(true, "Delete");
                }
            }
        }

        private void HandleMain(string info)
        {
            Configuration config = Configuration.Read(@"settings\login.json");
            if (config == null)
                return;

            if ("Loaded".Equals(info))
            {
                switch (config.GetInt("TaskId"))  //获取任务ID
                {
                    case 0:  //NS2
                        //pgCode = new CodeView();
                        //pgResult = new ResultView();
                        //pgGps = new GpsView();
                        //pgAttitude = new AttitudeView();
                        //pgPanel = new PanelView();
                        //pgVolt = new VoltView();
                        //pgCurr = new CurrView();
                        //pgTemp = new TempView();
                        //pgSchematic = new SchematicView();
                        pgOrbit = new OrbitView();
                        //pgStation = new StationView();
                        //pgCmd = new CmdView();
                        InitDefinePage();  //初始化自定义Page
                        break;
                    case 1:  //QH
                        break;
                    default:
                        break;
                }
            }
            else if ("Closed".Equals(info))
            {
            }
            else if ("Set".Equals(info))
            {
                //ScreenView winScreen = null;
                //switch (config.GetInt("TaskId"))  //获取任务ID
                //{
                //    case 0:  //NS2
                //        switch (currentPage)
                //        {
                //            case "源码":
                //                winScreen = new ScreenView(new CodeView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "参数":
                //                winScreen = new ScreenView(new ResultView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "GPS":
                //                winScreen = new ScreenView(new GpsView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "姿态角":
                //                winScreen = new ScreenView(new AttitudeView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "帆板":
                //                winScreen = new ScreenView(new PanelView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "电压":
                //                winScreen = new ScreenView(new VoltView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "电流":
                //                winScreen = new ScreenView(new CurrView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "温度":
                //                winScreen = new ScreenView(new TempView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "轨道":
                //                winScreen = new ScreenView(new OrbitView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "测站":
                //                winScreen = new ScreenView(new StationView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            case "遥控":
                //                winScreen = new ScreenView(new CmdView());
                //                FullScreenControl.GoFullscreen(winScreen);
                //                winScreen.ShowDialog();
                //                break;
                //            default:
                //                break;
                //        }
                //        break;
                //    case 1:  //QH
                //        break;
                //    default:
                //        break;
                //}


                //ScreenView screen = new ScreenView();
                //FullScreenControl.GoFullscreen(screen);
                //screen.ShowDialog();

            }
            else
            {
            }
        }

        private void HandleNavi(object obj)
        {
            NavigationModel navigation = (NavigationModel)obj;

            Configuration config = Configuration.Read(@"settings\login.json");
            if (config == null)
            {
                Trace.WriteLine("HandelNavi未找到配置文件");
                return;
            }

            switch (config.GetInt("TaskId"))  //获取任务ID
            {
                case 0:  //NS2
                    if (navigation.Parent == 0)  //第1级导航菜单
                    {
                    }
                    else if (navigation.Parent == 1)  //第2级导航菜单：遥测
                    {
                        switch (navigation.Id)
                        {
                            //case 11:
                            //    this.content.Content = new Frame() { Content = pgCode };
                            //    break;
                            //case 12:
                            //    this.content.Content = new Frame() { Content = pgResult };
                            //    break;
                            //case 13:
                            //    this.content.Content = new Frame() { Content = pgGps };
                            //    break;
                            //case 14:
                            //    this.content.Content = new Frame() { Content = pgAttitude };
                            //    break;
                            //case 15:
                            //    this.content.Content = new Frame() { Content = pgPanel };
                            //    break;
                            //case 16:
                            //    this.content.Content = new Frame() { Content = pgVolt };
                            //    break;
                            //case 17:
                            //    this.content.Content = new Frame() { Content = pgCurr };
                            //    break;
                            //case 18:
                            //    this.content.Content = new Frame() { Content = pgTemp };
                            //    break;
                            default:
                                break;
                        }
                    }
                    else if (navigation.Parent == 2)  //第2级导航菜单：外测
                    {
                        switch (navigation.Id)
                        {
                            case 101:
                                this.content.Content = new Frame() { Content = pgOrbit };
                                break;
                            case 102:
                                //this.content.Content = new Frame() { Content = pgStation };
                                break;
                            default:
                                break;
                        }
                    }
                    else if (navigation.Parent == 3)  //第2级导航菜单：遥控
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
                    else if (navigation.Parent == 4)  //第2级导航菜单：自定义
                    {
                        //using (IDbConnection connection = new SQLiteConnection(strConnection))
                        //{
                        //    List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //获取树形结构表：导航菜单

                        //    List<NavigationModel> defineList = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //第2级导航菜单：自定义
                        //    if (defineList != null)
                        //    {
                        //        for (int i = 0; i < defineList.Count; i++)
                        //        {
                        //            if (navigation.Id == defineList[i].Id)
                        //            {
                        //                this.content.Content = new Frame() { Content = pgDefine[navigation.Id] };
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                    }




                    //using (IDbConnection connection = new SQLiteConnection(strConnection))
                    //{
                    //    List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi").AsList<NavigationModel>();  //获取树形结构表：导航菜单

                    //    List<NavigationModel> firstList = naviList.Where(x => x.Parent == 0).AsList<NavigationModel>();  //第1级导航菜单
                    //    for (int i = 0; i < firstList.Count; i++)
                    //    {
                    //        NavigationModel model = new NavigationModel() { Id = firstList[i].Id, Name = firstList[i].Name, Parent = firstList[i].Parent, Grid = firstList[i].Grid, Curve = firstList[i].Curve, Children = new List<NavigationModel>() };
                    //        List<NavigationModel> secondList = naviList.Where(x => x.Parent == firstList[i].Id).AsList<NavigationModel>();  //第2级导航菜单
                    //        for (int j = 0; j < secondList.Count; j++)
                    //        {
                    //            model.Children.Add(new NavigationModel() { Id = secondList[j].Id, Name = secondList[j].Name, Parent = secondList[j].Parent, Grid = secondList[j].Grid, Curve = secondList[j].Curve });
                    //        }
                    //    }
                    //}


                    //switch (navigation.Id)
                    //{
                    //    case 11:
                    //        this.content.Content = new Frame() { Content = pgCode };
                    //        break;
                    //    case 12:
                    //        this.content.Content = new Frame() { Content = pgResult };
                    //        break;
                    //    case 13:
                    //        this.content.Content = new Frame() { Content = pgGps };
                    //        break;
                    //    case 14:
                    //        this.content.Content = new Frame() { Content = pgAttitude };
                    //        break;
                    //    case 15:
                    //        this.content.Content = new Frame() { Content = pgPanel };
                    //        break;
                    //    case 16:
                    //        this.content.Content = new Frame() { Content = pgVolt };
                    //        break;
                    //    case 17:
                    //        this.content.Content = new Frame() { Content = pgCurr };
                    //        break;
                    //    case 18:
                    //        this.content.Content = new Frame() { Content = pgTemp };
                    //        break;
                    //    case 101:
                    //        this.content.Content = new Frame() { Content = pgOrbit };
                    //        break;
                    //    case 102:
                    //        this.content.Content = new Frame() { Content = pgStation };
                    //        break;
                    //    case 103:
                    //        this.content.Content = new Frame() { Content = pgCmd };
                    //        break;
                    //    case 4:
                    //        DefineView define = new DefineView();
                    //        define.ShowDialog();
                    //        break;
                    //case "301":
                    //    this.content.Content = new Frame() { Content = pgGrid0 };
                    //    break;
                    //case "302":
                    //    this.content.Content = new Frame() { Content = pgGrid1 };
                    //    break;
                    //case "303":
                    //    this.content.Content = new Frame() { Content = pgGrid2 };
                    //    break;
                    //case "304":
                    //    this.content.Content = new Frame() { Content = pgGrid3 };
                    //    break;
                    //case "305":
                    //    this.content.Content = new Frame() { Content = pgGrid4 };
                    //    break;
                    //case "351":
                    //    this.content.Content = new Frame() { Content = pgCurve0 };
                    //    break;
                    //case "352":
                    //    this.content.Content = new Frame() { Content = pgCurve1 };
                    //    break;
                    //case "353":
                    //    this.content.Content = new Frame() { Content = pgCurve2 };
                    //    break;
                    //case "354":
                    //    this.content.Content = new Frame() { Content = pgCurve3 };
                    //    break;
                    //case "355":
                    //    this.content.Content = new Frame() { Content = pgCurve4 };
                    //    break;
                    //    default:
                    //        break;
                    //}
                    break;
                default:
                    break;
            }
        }

        //private void HandleDefine(string id)
        //{
        //    Configuration config = Configuration.Read(@"login\config.json");
        //    if (config == null)
        //        return;
        //    switch (config.GetInt("TaskId"))  //获取任务ID
        //    {
        //        case 0:  //NS2
        //            switch (id)
        //            {
        //                case "301":
        //                    pgGrid0 = new GridResultView("301");
        //                    break;
        //                case "302":
        //                    pgGrid1 = new GridResultView("302");
        //                    break;
        //                case "303":
        //                    pgGrid2 = new GridResultView("303");
        //                    break;
        //                case "304":
        //                    pgGrid3 = new GridResultView("304");
        //                    break;
        //                case "305":
        //                    pgGrid4 = new GridResultView("305");
        //                    break;
        //                case "351":
        //                    pgCurve0 = new CurveResultView("351");
        //                    break;
        //                case "352":
        //                    pgCurve1 = new CurveResultView("352");
        //                    break;
        //                case "353":
        //                    pgCurve2 = new CurveResultView("353");
        //                    break;
        //                case "354":
        //                    pgCurve3 = new CurveResultView("354");
        //                    break;
        //                case "355":
        //                    pgCurve4 = new CurveResultView("355");
        //                    break;
        //                default:
        //                    break;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void HandleDelete(object obj)
        {
            NavigationModel navigation = (NavigationModel)obj;
            if (navigation.Id == 4)  //第1级导航菜单：自定义
            {
                //DefineView define = new DefineView();
                //define.ShowDialog();
            }
        }

        private void HandleDefine(int[] define)
        {
            switch (define[1])
            {
                case 1:
                    //this.pgDefine.Add(define[0], new GridView(define[0]));
                    break;
                case 2:
                    //this.pgDefine.Add(define[0], new CurveView(define[0], 0));
                    break;
                default:
                    break;
            }
        }

        private void HandleCurve(int curve)
        {
            //CurveAxisView axis = new CurveAxisView(curve);
            //axis.ShowDialog();
        }
        #endregion

        #region Method
        private void InitDefinePage()
        {
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //获取树形结构表：导航菜单

                List<NavigationModel> defineList = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //第2级导航菜单：自定义
                if (defineList != null)
                {
                    for (int i = 0; i < defineList.Count; i++)
                    {
                        switch (defineList[i].Define)
                        {
                            case 1:
                               // this.pgDefine[defineList[i].Id] = new GridView(defineList[i].Id);
                                break;
                            case 2:
                              //  this.pgDefine[defineList[i].Id] = new CurveView(defineList[i].Id, 1);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
        }
        #endregion
    }
}
