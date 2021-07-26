using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace satCompent.ViewModel
{
    public class OBCDataGridViewModel : ViewModelBase
    {
        #region Field
        private DataTable dt;
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


        public OBCDataGridViewModel()
        {
            dt = new DataTable();
            DataTb = new  DataTable();
            Messenger.Default.Register<DataTable>(this, "OBCGrid", HandleOBCGrid);
        }

        #region Override Method
        public override void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        int kc = 0;
        public void HandleOBCGrid(DataTable info)
        {
            try
            {
                DataTb = info.Copy();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("OBC DataGridView错误:" + ex.Message);
            }

            
        }
        #endregion


   
    }
}
