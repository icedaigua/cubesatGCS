using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace TSFCS.DMDS.Client.Model
{
    public class CmdModel : ObservableObject
    {
        #region Field
        private string time;
        private string called;
        private string content;
        private string chain;
        private string parameters;
        #endregion

        #region Property
        public string Time
        {
            get { return time; }
            set 
            { 
                time = value;
                RaisePropertyChanged("Time");
            }
        }
        public string Called
        {
            get { return called; }
            set
            { 
                called = value;
                RaisePropertyChanged("Called");
            }
        }
        public string Content
        {
            get { return content; }
            set
            { 
                content = value;
                RaisePropertyChanged("Content");
            }
        }
        public string Chain
        {
            get { return chain; }
            set 
            {
                chain = value;
                RaisePropertyChanged("Chain");
            }
        }
        public string Parameters
        {
            get { return parameters; }
            set 
            { 
                parameters = value;
                RaisePropertyChanged("Parameters");
            }
        }
        #endregion

        #region Method
        public static ObservableCollection<CmdModel> GetCmd()
        {
            //ObservableCollection<CmdModel> list = new ObservableCollection<CmdModel>();
            //list.Add(new CmdModel() { Time = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now), Called = "K0", Content = "OBC电源ON", Chain = string.Empty, Parameters = string.Empty });
            //list.Add(new CmdModel() { Time = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now), Called = "K1", Content = "OBC电源OFF", Chain = string.Empty, Parameters = string.Empty });

            //return list;

            return new ObservableCollection<CmdModel>();
        }
        #endregion
    }
}
