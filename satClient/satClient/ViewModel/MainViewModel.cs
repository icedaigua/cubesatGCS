using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using satClient.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace satClient.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            Messenger.Default.Register<object>(this, "Define", HandleDefine);
            Messenger.Default.Register<bool>(this, "Delete", HandleDelete);
        }

        #region 参数
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




        #region command

        private bool CanLoadedExecute()
        {
            return true;
        }
        private void LoadedExecute()
        {
            Navigation = new ObservableCollection<NavigationModel>();  //导航菜单初始化
            LoadNavigation();
            Messenger.Default.Send<string>("Loaded", "Main");  //页面内容初始化
        }
        public ICommand LoadedCommand { get { return new RelayCommand(LoadedExecute, CanLoadedExecute); } }


        private bool CanClosedExecute()
        {
            return true;
        }
        private void ClosedExecute()
        {
            Messenger.Default.Send<string>("closed", "main");
        }
        public ICommand ClosedCommand { get { return new RelayCommand(ClosedExecute, CanClosedExecute); } }


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
            Messenger.Default.Send<object>(obj, "Delete");
            Messenger.Default.Send<object>(obj, "Confirm");
        }
        public ICommand DefineCommand { get { return new RelayCommand<object>(DefineExecute, CanDefineExecute); } }

        #endregion



        #region Messenger Handler
        private void HandleDefine(object obj)
        {
          
        }

        private void HandleDelete(bool isDelete)
        {

        }
        #endregion


        #region Method
        private void LoadNavigation()
        {
            List<NavigationModel> naviList = new List<NavigationModel>();

            NavigationModel model_0 = new NavigationModel() { Id = 1, Name = "遥测", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_1 = new NavigationModel() { Id = 2, Name = "遥控", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_2 = new NavigationModel() { Id = 3, Name = "自定义", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_3 = new NavigationModel() { Id = 4, Name = "原始", Parent = 0, Define = 1, Children = new List<NavigationModel>() };

            NavigationModel model_01 = new NavigationModel() { Id = 41, Name = "遥测帧", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_02 = new NavigationModel() { Id = 42, Name = "平台", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_03 = new NavigationModel() { Id = 43, Name = "姿控", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_04 = new NavigationModel() { Id = 44, Name = "载荷", Parent = 4, Define = 1, Children = new List<NavigationModel>() };



            NavigationModel model_11 = new NavigationModel() { Id = 11, Name = "星务", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_12 = new NavigationModel() { Id = 12, Name = "电源", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_13 = new NavigationModel() { Id = 13, Name = "姿控", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_14 = new NavigationModel() { Id = 14, Name = "温度", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_15 = new NavigationModel() { Id = 15, Name = "元器件载荷", Parent = 1, Define = 1, Children = new List<NavigationModel>() };

            NavigationModel model_21 = new NavigationModel() { Id = 21, Name = "星务", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_22 = new NavigationModel() { Id = 22, Name = "电源", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_23 = new NavigationModel() { Id = 23, Name = "姿控", Parent = 2, Define = 1, Children = new List<NavigationModel>() };

            naviList.Add(model_3); naviList.Add(model_0); naviList.Add(model_1); naviList.Add(model_2);
            naviList.Add(model_01); naviList.Add(model_02); naviList.Add(model_03); naviList.Add(model_04);
            naviList.Add(model_11); naviList.Add(model_12); naviList.Add(model_13); naviList.Add(model_14); naviList.Add(model_15);
            naviList.Add(model_21); naviList.Add(model_22); naviList.Add(model_23);
            List<NavigationModel> firstList = new List<NavigationModel>();//= naviList.Where(x => x.Parent == 0).AsList<NavigationModel>();  //第1级导航菜单

            foreach(NavigationModel nm in naviList)
            {
                if (nm.Parent == 0)
                    firstList.Add(nm);
            }
                for (int i = 0; i < firstList.Count; i++)
                {
                    NavigationModel model = new NavigationModel() { Id = firstList[i].Id, Name = firstList[i].Name, Parent = firstList[i].Parent, Define = firstList[i].Define, Children = new List<NavigationModel>() };
                    List<NavigationModel> secondList = new List<NavigationModel>();

                    foreach (NavigationModel nm in naviList)
                    {
                        if (nm.Parent == firstList[i].Id)
                            secondList.Add(nm);
                    }
                    for (int j = 0; j < secondList.Count; j++)
                    {
                        model.Children.Add(new NavigationModel() { Id = secondList[j].Id, Name = secondList[j].Name, Parent = secondList[j].Parent, Define = secondList[j].Define });
                    }
                    Navigation.Add(model);
                }
        }
        #endregion

    }
}