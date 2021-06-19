using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Input;

using Dapper;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class CurveAxisViewModel : ViewModelBase
    {
        #region Field
        private readonly string strConnection = "Data Source=db_dmds.db3;Password=1234";
        private int curve;  //唯一标识Curve
        private string xTitle;
        private string xStringFormat;
        private string yTitle;
        private string yMajorStep;
        private string yMinimum;
        private string yMaximum;
        #endregion

        #region Property
        public string XTitle
        {
            get { return xTitle; }
            set
            { 
                xTitle = value;
                RaisePropertyChanged("XTitle");
            }
        }
        public string XStringFormat
        {
            get { return xStringFormat; }
            set 
            { 
                xStringFormat = value;
                RaisePropertyChanged("XStringFormat");
            }
        }
        public string YTitle
        {
            get { return yTitle; }
            set 
            { 
                yTitle = value;
                RaisePropertyChanged("YTitle");
            }
        }
        public string YMajorStep
        {
            get { return yMajorStep; }
            set 
            { 
                yMajorStep = value;
                RaisePropertyChanged("YMajorStep");
            }
        }
        public string YMinimum
        {
            get { return yMinimum; }
            set 
            { 
                yMinimum = value;
                RaisePropertyChanged("YMinimum");
            }
        }
        public string YMaximum
        {
            get { return yMaximum; }
            set 
            {
                yMaximum = value;
                RaisePropertyChanged("YMaximum");
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
        }
        public ICommand LoadedCommand { get { return new RelayCommand(LoadedExecute, CanLoadedExecute); } }

        public bool CanOkExecute()
        {
            return true;
        }
        public void OkExecute()
        {
            UpdateCurveAxis();

            Messenger.Default.Send<bool>(true, "Curve");
        }
        public ICommand OkCommand { get { return new RelayCommand(OkExecute, CanOkExecute); } }

        public bool CanCancelExecute()
        {
            return true;
        }
        public void CancelExecute()
        {
            Messenger.Default.Send<bool>(false, "Curve");
        }
        public ICommand CancelCommand { get { return new RelayCommand(CancelExecute, CanCancelExecute); } }
        #endregion

        #region Constructor
        public CurveAxisViewModel(int curve)
        {
            this.curve = curve;

            this.xTitle = "时间";
            this.xStringFormat = "HH:mm:ss";
            this.yTitle = "遥测参数";
            this.yMajorStep = "50";
            this.yMinimum = "0";
            this.yMaximum = "100";
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void UpdateCurveAxis()
        {
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<CurveAxisModel> axisList = connection.Query<CurveAxisModel>("select * from t_curve_ns2 where curve = @cv", new { cv = this.curve }).AsList<CurveAxisModel>();  //从数据库中查询所有所属Id的记录
                if (axisList != null)  //更新
                {
                    string strSql = @"update t_curve_ns2 set xtitle = @xtitle, xstringformat =@xstringformat, ytitle = @ytitle, ymajorstep = @ymajorstep, yminimum = @yminimum, ymaximum = @ymaximum where curve = @cv";  //数据库更新语句
                    connection.Execute(strSql, new { xtitle = this.XTitle, xstringformat = this.XStringFormat, ytitle = this.YTitle, ymajorstep = this.YMajorStep, yminimum = this.YMinimum, ymaximum = this.YMaximum, cv = this.curve });  //更新数据库记录
                }
            }
        }
        #endregion
    }
}
