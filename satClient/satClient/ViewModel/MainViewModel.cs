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

        #region ����
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




        #region command

        private bool CanLoadedExecute()
        {
            return true;
        }
        private void LoadedExecute()
        {
            Navigation = new ObservableCollection<NavigationModel>();  //�����˵���ʼ��
            LoadNavigation();
            Messenger.Default.Send<string>("Loaded", "Main");  //ҳ�����ݳ�ʼ��
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

            NavigationModel model_0 = new NavigationModel() { Id = 1, Name = "ң��", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_1 = new NavigationModel() { Id = 2, Name = "ң��", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_2 = new NavigationModel() { Id = 3, Name = "�Զ���", Parent = 0, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_3 = new NavigationModel() { Id = 4, Name = "ԭʼ", Parent = 0, Define = 1, Children = new List<NavigationModel>() };

            NavigationModel model_01 = new NavigationModel() { Id = 41, Name = "ң��֡", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_02 = new NavigationModel() { Id = 42, Name = "ƽ̨", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_03 = new NavigationModel() { Id = 43, Name = "�˿�", Parent = 4, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_04 = new NavigationModel() { Id = 44, Name = "�غ�", Parent = 4, Define = 1, Children = new List<NavigationModel>() };



            NavigationModel model_11 = new NavigationModel() { Id = 11, Name = "����", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_12 = new NavigationModel() { Id = 12, Name = "��Դ", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_13 = new NavigationModel() { Id = 13, Name = "�˿�", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_14 = new NavigationModel() { Id = 14, Name = "�¶�", Parent = 1, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_15 = new NavigationModel() { Id = 15, Name = "Ԫ�����غ�", Parent = 1, Define = 1, Children = new List<NavigationModel>() };

            NavigationModel model_21 = new NavigationModel() { Id = 21, Name = "����", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_22 = new NavigationModel() { Id = 22, Name = "��Դ", Parent = 2, Define = 1, Children = new List<NavigationModel>() };
            NavigationModel model_23 = new NavigationModel() { Id = 23, Name = "�˿�", Parent = 2, Define = 1, Children = new List<NavigationModel>() };

            naviList.Add(model_3); naviList.Add(model_0); naviList.Add(model_1); naviList.Add(model_2);
            naviList.Add(model_01); naviList.Add(model_02); naviList.Add(model_03); naviList.Add(model_04);
            naviList.Add(model_11); naviList.Add(model_12); naviList.Add(model_13); naviList.Add(model_14); naviList.Add(model_15);
            naviList.Add(model_21); naviList.Add(model_22); naviList.Add(model_23);
            List<NavigationModel> firstList = new List<NavigationModel>();//= naviList.Where(x => x.Parent == 0).AsList<NavigationModel>();  //��1�������˵�

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