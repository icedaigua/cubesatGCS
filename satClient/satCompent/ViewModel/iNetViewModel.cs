
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;

using System.Windows.Input;

namespace satCompent.ViewModel
{
    public class iNetViewModel:ViewModelBase
    {

        #region Field
        private string recvmsg;
        private string sendmsg;
        private string netip;
        private string netno;
        private bool defineType;
        #endregion

        #region Property
        public string RecvMsg
        {
            get { return recvmsg; }
            set
            {
                recvmsg = value;
                RaisePropertyChanged("RecvMsg");
            }
        }

        public string SendMsg
        {
            get { return sendmsg; }
            set
            {
                sendmsg = value;
                RaisePropertyChanged("SendMsg");
            }
        }

        public string NetIP
        {
            get { return netip; }
            set
            {
                netip = value;
                RaisePropertyChanged("NetIP");
            }
        }

        public string NetNO
        {
            get { return netno; }
            set
            {
                netno = value;
                RaisePropertyChanged("NetNO");
            }
        }

        public bool DefineType
        {
            get { return defineType; }
            set
            {
                defineType = value;
                RaisePropertyChanged("DefineType");
            }
        }
        #endregion


        #region Constructor
        public iNetViewModel()
        {
            NetIP = "192.168.1.101";
            NetNO = "9889";
            Messenger.Default.Register<string>(this, "INET", HandleINet);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }
        #endregion


        #region Method
        public void HandleINet(string info)
        {
            if(defineType)
                RecvMsg = info;
        }
        #endregion

        #region Command
        public bool CanOkExecute()
        {
            return true;
        }
        public void OkExecute()
        {
            string inetmsg = "Open" + "," + NetIP + "," + NetNO;
            Messenger.Default.Send<string>(inetmsg, "MainNet");
        }
        public ICommand OpenNetCommand { get { return new RelayCommand(OkExecute, CanOkExecute); } }

        public bool CanCancelExecute()
        {
            return true;
        }
        public void CancelExecute()
        {
            string inetmsg = "Close";
            Messenger.Default.Send<string>(inetmsg, "MainNet");
        }
        public ICommand CloseNetCommand { get { return new RelayCommand(CancelExecute, CanCancelExecute); } }

        public bool CanSendExecute()
        {
            return true;
        }
        public void SendExecute()
        {
            string inetmsg = "Send"+","+ SendMsg;
            Messenger.Default.Send<string>(inetmsg, "MainNet");
        }
        public ICommand SendCommand { get { return new RelayCommand(SendExecute, CanSendExecute); } }

        public bool CanClearExecute()
        {
            return true;
        }
        public void ClearExecute()
        {
            
        }
        public ICommand ClearCommand { get { return new RelayCommand(ClearExecute, CanClearExecute); } }

        #endregion


    }
}
