using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

namespace TSFCS.DMDS.Client.Model
{
    public class TaskModel : ObservableObject
    {
        #region Field
        private int id;
        private string name;
        #endregion

        #region Property
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        #endregion

        #region Constructor and Method
        public TaskModel()
        { 
        }

        public static ObservableCollection<TaskModel> GetTask()
        {
            ObservableCollection<TaskModel> list = new ObservableCollection<TaskModel>();
            list.Add(new TaskModel() { Id = 0, Name = "NJUST-1" });
            list.Add(new TaskModel() { Id = 1, Name = "EnLai" });

            return list;
        }
        #endregion
    }
}
