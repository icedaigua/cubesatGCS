using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<TaskModel> tasks;
        private TaskModel task;
        private string username;
        private string password;
        private bool isRemained;
        #endregion

        #region Property
        public ObservableCollection<TaskModel> Tasks
        {
            get { return tasks; }
            set
            { 
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        public TaskModel Task
        {
            get { return task; }
            set 
            {
                task = value;
                RaisePropertyChanged("Task");
            }
        }
        public string Username
        {
            get { return username; }
            set 
            { 
                username = value;
                RaisePropertyChanged("Username");
            }
        }
        public string Password
        {
            get { return password; }
            set 
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        public bool IsRemained
        {
            get { return isRemained; }
            set 
            { 
                isRemained = value;
                RaisePropertyChanged("IsRemained");
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
            LoadConfig();  //加载配置
            //Messenger.Default.Send<string>("loaded", "login");
        }
        public ICommand LoadedCommand { get { return new RelayCommand(LoadedExecute, CanLoadedExecute); } }

        private bool CanDragMoveExecute()
        {
            return true;
        }
        private void DragMoveExecute()
        {
            Messenger.Default.Send<string>("dragmove", "login");
        }
        public ICommand DragMoveCommand { get { return new RelayCommand(DragMoveExecute, CanDragMoveExecute); } }

        private bool CanMinExecute()
        {
            return true;
        }
        private void MinExecute()
        {
            Messenger.Default.Send<string>("min", "login");
        }
        public ICommand MinCommand { get { return new RelayCommand(MinExecute, CanMinExecute); } }

        private bool CanClosedExecute()
        {
            return true;
        }
        private void ClosedExecute()
        {
            Messenger.Default.Send<string>("closed", "login");
        }
        public ICommand ClosedCommand { get { return new RelayCommand(ClosedExecute, CanClosedExecute); } }

        private bool CanLoginExecute()
        {
            return true;
        }
        private void LoginExecute()
        {
            SaveConfig();  //保存配置
            Messenger.Default.Send<string>("login", "login");
        }
        public ICommand LoginCommand { get { return new RelayCommand(LoginExecute, CanLoginExecute); } }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            this.tasks = TaskModel.GetTask();  //任务列表
            this.task = tasks[0];  //默认选中第0个任务
            this.username = "admin";
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            base.Cleanup();
        }
        #endregion

        #region 配置信息
        // 
        // 保存的配置信息如下：
        // 0. 任务ID
        // 1. 用户名
        // 2. 密码
        // 3. 记住密码
        //
        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveConfig()
        {
            Configuration config = new Configuration();
            if (config != null)
            {
                config.Set("TaskId", this.Task.Id);  //保存任务ID
                config.Set("Username", this.Username);  //保存用户名
                config.Set("Password", this.Password != null ? this.Password : string.Empty);  //保存密码
                config.Set("IsRemained", this.IsRemained);  //保存记住密码
                Configuration.Save(config, @"login\config.json");  //保存配置信息到磁盘中
                //Configuration.Save(config);  //保存配置信息到磁盘中
            }
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        private bool LoadConfig()
        {
            Configuration config = Configuration.Read(@"login\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return false;

            this.IsRemained = config.GetBool("IsRemained");  //获取记住密码
            if (this.IsRemained)
                this.Password = config.GetString("Password");  //获取密码

            return true;
        }
        #endregion
    }
}
