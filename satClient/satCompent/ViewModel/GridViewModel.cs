using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

using Dapper;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class GridViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<ResultModel> results;
        private ResultModel result;
        private ObservableCollection<ResultModel> cal;
        private int grid;  //唯一标识Grid
        private readonly string strConnection = "Data Source=db_dmds.db3;Password=1234";
        #endregion

        #region Property
        public ObservableCollection<ResultModel> Results
        {
            get { return results; }
            set 
            {
                results = value;
                RaisePropertyChanged("Results");
            }
        }
        public ResultModel Result
        {
            get { return result; }
            set 
            { 
                result = value;
                RaisePropertyChanged("Result");
            }
        }
        public ObservableCollection<ResultModel> Cal
        {
            get { return cal; }
            set 
            { 
                cal = value;
                RaisePropertyChanged("Cal");
            }
        }
        #endregion

        #region Command
        private bool CanAddExecute()
        {
            return true;
        }
        private void AddExecute()
        {
            this.Cal.Add(Result);

            AddResult(Result);
        }
        public ICommand AddCommand { get { return new RelayCommand(AddExecute, CanAddExecute); } }

        private bool CanDeleteExecute(object obj)
        {
            return true;
        }
        private void DeleteExecute(object obj)
        {
            this.Cal.Remove((ResultModel)obj);

            DeleteResult((ResultModel)obj);
        }
        public ICommand DeleteCommand { get { return new RelayCommand<object>(DeleteExecute, CanDeleteExecute); } }
        #endregion

        #region Contructor
        public GridViewModel(int grid)
        {
            this.results = ResultModel.GetAllResult();
            this.result = this.results[0];
            this.grid = grid;
            this.cal = InitResult("select * from t_grid_ns2 where grid = @grid");
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfResult>(this, "TMRSLT", HandleGrid);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private ObservableCollection<ResultModel> InitResult(string strSql)
        {
            ObservableCollection<ResultModel> models = null;

            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<ResultModel> resultList = connection.Query<ResultModel>(strSql, new { grid = grid }).AsList<ResultModel>();  //从数据库中查询所有所属Id的记录
                if (resultList != null)
                {
                    models = new ObservableCollection<ResultModel>(resultList);
                }
                else
                {
                    models = new ObservableCollection<ResultModel>(); 
                }
            }

            return models;
        }

        private void AddResult(ResultModel result)
        {
            string strSql = @"insert into t_grid_ns2 (grid, no, name, val, unit, exceed) VALUES (@Grid, @No, @Name, @Val, @Unit, @Exceed)";  //数据库插入语句

            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                result.Grid = this.grid;  //获取所属自定义Grid
                connection.Execute(strSql, result);  //将选中记录插入数据库  
            }
        }

        private void DeleteResult(ResultModel result)
        {
            string strSql = @"delete from t_grid_ns2 where name = @name";  //数据库删除语句

            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                connection.Execute(strSql, new { name = result.Name });  //将选中自定义节点从数据库中删除  
            }
        }
        #endregion

        #region Messenger Handler
        public void HandleGrid(MsgOfResult msg)
        {
        }
        #endregion
    }
}
