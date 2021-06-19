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
        private ObservableCollection<NavigationModel> navigation;  //����

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
            Navigation = new ObservableCollection<NavigationModel>();  //�����˵���ʼ��

            Configuration config = Configuration.Read(@"settings\login.json");
            if (config == null)
            {
                Trace.WriteLine("satcompentδ�ҵ���ʼ�����ļ�,�˳�");
                return;
            }

            switch (config.GetInt("TaskId"))  //��ȡ����ID
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

            Messenger.Default.Send<string>("Loaded", "Main");  //ҳ�����ݳ�ʼ��

            Task.Factory.StartNew(TaskRecv);  //������������:ZeroMQ
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
                SaveConfig((NavigationModel)obj);  //���浼���˵�
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

            string strSql = @"insert into t_navi_ns2 (id, name, parent, define) VALUES (@Id, @Name, @Parent, @Define)";  //���ݿ�������

            NavigationModel defineNavi = new NavigationModel() { Id = 4, Name = "�Զ���", Parent = 0, Define = 0 };  //�Զ��嵼��
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                connection.Execute(strSql, model);  //���Զ���ڵ�������ݿ�  

                List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //��ȡ���νṹ�������˵�
                defineNavi.Children = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //��2�������˵����Զ���
            }
            Navigation.RemoveAt(3);
            Navigation.Add(defineNavi);

            Messenger.Default.Send<int[]>(new int[] { model.Id, model.Define }, "Define");  //��ʼ���Զ���Page
        }

        private void HandleDelete(bool isDelete)
        {
            Configuration config = Configuration.Read(@"navi\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return;

            int id = config.GetInt("Id");  //��ȡ�����˵�Id

            string strSql = @"delete from t_navi_ns2 where id = @id";  //���ݿ�ɾ�����

            NavigationModel defineNavi = new NavigationModel() { Id = 4, Name = "�Զ���", Parent = 0 };  //�Զ��嵼��
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                connection.Execute(strSql, new { id = id });  //��ѡ���Զ���ڵ�����ݿ���ɾ��  

                List<NavigationModel> naviList = connection.Query<NavigationModel>("select * from t_navi_ns2").AsList<NavigationModel>();  //��ȡ���νṹ�������˵�
                defineNavi.Children = naviList.Where(x => x.Parent == 4).AsList<NavigationModel>();  //��2�������˵����Զ���
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
                List<NavigationModel> naviList = connection.Query<NavigationModel>(strSql).AsList<NavigationModel>();  //��ȡ���νṹ�������˵�

                List<NavigationModel> firstList = naviList.Where(x => x.Parent == 0).AsList<NavigationModel>();  //��1�������˵�
                for (int i = 0; i < firstList.Count; i++)
                {
                    NavigationModel model = new NavigationModel() { Id = firstList[i].Id, Name = firstList[i].Name, Parent = firstList[i].Parent, Define = firstList[i].Define, Children = new List<NavigationModel>() };
                    List<NavigationModel> secondList = naviList.Where(x => x.Parent == firstList[i].Id).AsList<NavigationModel>();  //��2�������˵�
                    for (int j = 0; j < secondList.Count; j++)
                    {
                        model.Children.Add(new NavigationModel() { Id = secondList[j].Id, Name = secondList[j].Name, Parent = secondList[j].Parent, Define = secondList[j].Define });
                    }
                    Navigation.Add(model);
                }
            }
        }


        /// <summary>
        /// ִ�н�������
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
            //                    Messenger.Default.Send<byte[]>(System.Text.Encoding.Unicode.GetBytes(recvMsg.GetS(0).Val), "TMCODE");  //��ʾң��Դ��
            //                    break;
            //                case "TMRSLT":
            //                    //SetResult(recvMsg, ref msgOfResult, ref msgOfGps, ref msgOfAttitude, ref msgOfPanel, ref msgOfVolt, ref msgOfCurr, ref msgOfTemp);  //����ң������GPS,��̬�ǣ���ѹ���������¶�
            //                    //Messenger.Default.Send<MsgOfResult>(msgOfResult, "TMRSLT");  //��ʾң����
            //                    //Messenger.Default.Send<MsgOfGps>(msgOfGps, "TMGPS");  //��ʾGPS
            //                    //Messenger.Default.Send<MsgOfAttitude>(msgOfAttitude, "TMATTI");  //��ʾ��̬�� 
            //                    //Messenger.Default.Send<MsgOfPanel>(msgOfPanel, "TMPANEL");  //��ʾ���� 
            //                    //Messenger.Default.Send<MsgOfVolt>(msgOfVolt, "TMVOLT");  //��ʾ��ѹ 
            //                    //Messenger.Default.Send<MsgOfCurr>(msgOfCurr, "TMCURR");  //��ʾ���� 
            //                    //Messenger.Default.Send<MsgOfTemp>(msgOfTemp, "TMTEMP");  //��ʾ�¶�

            //                    Messenger.Default.Send<RecvMsg>(recvMsg, "TMRSLT");  //��ʾң����
            //                    break;
            //                case "ORBS":
            //                    SetOrbit(recvMsg, ref msgOfOrbit);
            //                    Messenger.Default.Send<MsgOfOrbit>(msgOfOrbit, "ORBS");  //��ʾ���
            //                    break;
            //                default:
            //                    break;
            //            }

            //            System.Threading.Thread.Sleep(10);  //��Ϣ10ms
            //        }
            //    }
            //}
        }

   
        #endregion

        #region ������Ϣ
        // 
        // �����������Ϣ���£�
        // 0. �����˵�Id
        // 1. �����˵�Name
        // 2. �����˵�Parent
        //
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        private void SaveConfig(NavigationModel model)
        {
            Configuration config = new Configuration();
            if (config != null)
            {
                config.Set("Id", model.Id);  //���浼���˵�Id
                config.Set("Name", model.Name);  //���浼���˵�Name
                config.Set("Parent", model.Parent);  //���浼���˵�Parent
                Configuration.Save(config, @"navi\config.json");  //����������Ϣ��������
                //Configuration.Save(config);  //����������Ϣ��������
            }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        private NavigationModel LoadConfig()
        {
            Configuration config = Configuration.Read(@"navi\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return null;

            NavigationModel model = new NavigationModel();
            model.Id = config.GetInt("Id");  //��ȡ�����˵�Id
            model.Name = config.GetString("Name");  //��ȡ�����˵�Name
            model.Parent = config.GetInt("Parent");  //��ȡ�����˵�Parent

            return model;
        }
        #endregion

    }
}