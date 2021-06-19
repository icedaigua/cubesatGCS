using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class DefineViewModel : ViewModelBase
    {
        #region Field
        private string defineType;
        private string defineName;
        private int defineId;  //自定义Id, 保存于配置文件
        #endregion

        #region Property
        public string DefineType
        {
            get { return defineType; }
            set
            {
                defineType = value;
                RaisePropertyChanged("DefineType");
            }
        }
        public string DefineName
        {
            get { return defineName; }
            set
            {
                defineName = value;
                RaisePropertyChanged("DefineName");
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
            this.DefineType = "Grid";  //默认表格

            Messenger.Default.Send<string>("Loaded", "Define");
        }
        public ICommand LoadedCommand { get { return new RelayCommand(LoadedExecute, CanLoadedExecute); } }

        public bool CanOkExecute()
        {
            return true;
        }
        public void OkExecute()
        {
            SaveConfig();  //保存当前ID
            switch (this.DefineType)
            {
                case "Grid":
                    Messenger.Default.Send<object>(new NavigationModel() { Id = this.defineId, Name = this.DefineName, Parent = 4, Define = 1 }, "Define");
                    break;
                case "Curve":
                    Messenger.Default.Send<object>(new NavigationModel() { Id = this.defineId, Name = this.DefineName, Parent = 4, Define = 2 }, "Define");
                    break;
                default:
                    break;
            }

            Messenger.Default.Send<bool>(true, "Define");
        }
        public ICommand OkCommand { get { return new RelayCommand(OkExecute, CanOkExecute); } }

        public bool CanCancelExecute()
        {
            return true;
        }
        public void CancelExecute()
        {
            Messenger.Default.Send<bool>(false, "Define");
        }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExecute, CanCancelExecute); } }
        #endregion

        #region 配置信息
        // 
        // 保存的配置信息如下：
        // 0. DefineId:150开始计数
        //
        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveConfig()
        {
            if (LoadConfig(ref defineId))
            {
                Configuration config = new Configuration();
                if (config != null)
                {
                    config.Set("DefineId", ++defineId);  //保存DefineId
                    Configuration.Save(config, @"define\config.json");  //保存配置信息到磁盘中
                    //Configuration.Save(config);  //保存配置信息到磁盘中
                }
            }
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        private bool LoadConfig(ref int id)
        {
            Configuration config = Configuration.Read(@"define\config.json");
            //Configuration config = Configuration.Read();
            if (config == null)
                return false;

            this.defineId = config.GetInt("DefineId");  //获取DefineId

            return true;
        }
        #endregion

    }
}
