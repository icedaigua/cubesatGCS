using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using satClient.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


using System.Data;

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
            //Messenger.Default.Send<string>("define", "INET");
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


            NavigationModel model_3 = new NavigationModel() { Id = 11, Name = "星务", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_4 = new NavigationModel() { Id = 12, Name = "电源", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_5 = new NavigationModel() { Id = 13, Name = "姿控", Parent = 1, Define = 1, Children = new List<NavigationModel>() };

            NavigationModel model_6 = new NavigationModel() { Id = 21, Name = "星务", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_7 = new NavigationModel() { Id = 22, Name = "电源", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_8 = new NavigationModel() { Id = 23, Name = "姿控", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
  
            naviList.Add(model_0);
            naviList.Add(model_1); naviList.Add(model_2); naviList.Add(model_3); naviList.Add(model_4); naviList.Add(model_5); naviList.Add(model_6);
            naviList.Add(model_7); naviList.Add(model_8);
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