using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Data;

namespace satCompent.ViewModel
{
    public class ADCSDataGridViewModel : ViewModelBase
    {
        #region Field
        private DataTable dt = new DataTable();
        #endregion

        #region Property
        public DataTable DataTb
        {
            get { return dt; }
            set
            {
                dt = value;
                RaisePropertyChanged("DataTb");
            }
        }
        #endregion


        public ADCSDataGridViewModel()
        {
            Messenger.Default.Register<DataTable>(this, "ADCSGrid", HandleOBCGrid);
        }

        #region Override Method
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void HandleOBCGrid(DataTable info)
        {
            DataTb = info.Copy();
        }
        #endregion


   
    }
}
