using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace satCompent.ViewModel
{
    public class DataGridViewModel : ViewModelBase
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


        public DataGridViewModel()
        {
            Messenger.Default.Register<DataTable>(this, "DataGrid", HandleDataGrid);
        }


        #region Override Method
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void HandleDataGrid(DataTable info)
        {

            if (DataTb.Rows.Count < 1)
                DataTb = info.Copy();
            else
                DataTb.Rows.Add(info.Rows[0].ItemArray);
        }
        #endregion


   
    }
}
