using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Dapper;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class CurveViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<ResultModel> results;
        private ResultModel result;

        private PlotModel curveModel;
        private double curve;
        private List<DataPoint> curvePoints;

        private int cv;  //唯一标识Curve
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
        public PlotModel CurveModel
        {
            get { return curveModel; }
            set
            {
                curveModel = value;
                RaisePropertyChanged("CurveModel");
            }
        }
        public double Curve
        {
            get { return curve; }
            set
            {
                curve = value;
                RaisePropertyChanged("Curve");
            }
        }
        #endregion

        #region Command
        private bool CanSetExecute()
        {
            return true;
        }
        private void SetExecute()
        {
            this.curveModel.Title = this.Result.Name;
            this.curveModel.InvalidatePlot(true);
        }
        public ICommand SetCommand { get { return new RelayCommand(SetExecute, CanSetExecute); } }

        private bool CanAxisExecute()
        {
            return true;
        }
        private void AxisExecute()
        {
            Messenger.Default.Send<int>(this.cv, "Curve");
        }
        public ICommand AxisCommand { get { return new RelayCommand(AxisExecute, CanAxisExecute); } }
        #endregion

        #region Contructor
        public CurveViewModel(int cv, int type)
        {
            this.results = ResultModel.GetAllResult();
            this.result = this.results[0];
            this.cv = cv;
            switch (type)
            {
                case 0:  //第1次初始化
                    InitCurveModel();
                    break;
                case 1:  //第n次加载：n > 1
                    LoadCurveModel("select * from t_curve_ns2 where curve = @cv");
                    break;
                default:
                    break;
            }
            
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<bool>(this, "Curve", HandleCurve);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfResult>(this, "TMRSLT", HandleCurve);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Messenger Handler
        public void HandleCurve(MsgOfResult msg)
        {
        }

        public void HandleCurve(bool isCurveAxis)
        {
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<CurveAxisModel> axisList = connection.Query<CurveAxisModel>("select * from t_curve_ns2 where curve = @cv", new { cv = this.cv }).AsList<CurveAxisModel>();  //从数据库中查询所有所属Id的记录
                if (axisList != null)  //加载
                {
                    this.CurveModel.Axes[0].Title = axisList[0].XTitle;
                    this.CurveModel.Axes[0].StringFormat = axisList[0].XStringFormat;
                    this.CurveModel.Axes[1].Title = axisList[0].YTitle;
                    this.CurveModel.Axes[1].MajorStep = axisList[0].YMajorStep;
                    this.CurveModel.Axes[1].Minimum = axisList[0].YMinimum;
                    this.CurveModel.Axes[1].Maximum = axisList[0].YMaximum;

                    this.CurveModel.InvalidatePlot(true);
                }
            }
        }
        #endregion

        #region Method
        private void InitCurveModel()
        {
            DateTimeAxis xAxisCurve = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "时间",
                TitlePosition = 1,
                StringFormat = "HH:mm:ss",
                IsZoomEnabled = false,  //坐标轴缩放关闭
                IsPanEnabled = false,  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                IntervalLength = 60
            };
            LinearAxis yAxisCurve = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = "遥测参数",
                TitlePosition = 1,
                IsZoomEnabled = false,  //坐标轴缩放关闭
                IsPanEnabled = false,  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = 50,
                Minimum = 0,
                Maximum = 100,
                //IntervalLength = 50
            };
            curveModel = new PlotModel() { Title = "遥测参数曲线" };  //线条
            curveModel.Axes.Add(xAxisCurve);
            curveModel.Axes.Add(yAxisCurve);
            curvePoints = new List<DataPoint>();

            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                string strSql = @"insert into t_curve_ns2 (curve, title, xtitle, xstringformat, ytitle, ymajorstep, yminimum, ymaximum) VALUES (@curve, @title, @xtitle, @xstringformat, @ytitle, @ymajorstep, @yminimum, @ymaximum)";  //数据库插入语句
                connection.Execute(strSql, new { curve = this.cv, title = curveModel.Title, xtitle = curveModel.Axes[0].Title, xstringformat = curveModel.Axes[0].StringFormat, ytitle = curveModel.Axes[1].Title, ymajorstep = curveModel.Axes[1].MajorStep, yminimum = curveModel.Axes[1].Minimum, ymaximum = curveModel.Axes[1].Maximum });  //将记录插入数据库
            }
        }

        private void LoadCurveModel(string strSql)
        {
            using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                List<CurveAxisModel> axisList = connection.Query<CurveAxisModel>(strSql, new { cv = this.cv }).AsList<CurveAxisModel>();  //从数据库中查询所有所属Id的记录
                if (axisList != null)  //加载
                {
                    DateTimeAxis xAxisCurve = new DateTimeAxis()
                    {
                        Position = AxisPosition.Bottom,
                        Title = axisList[0].XTitle,
                        TitlePosition = 1,
                        StringFormat = axisList[0].XStringFormat,
                        IsZoomEnabled = false,  //坐标轴缩放关闭
                        IsPanEnabled = false,  //图表缩放功能关闭
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                        Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                        IntervalLength = 60
                    };
                    LinearAxis yAxisCurve = new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Title = axisList[0].YTitle,
                        TitlePosition = 1,
                        IsZoomEnabled = false,  //坐标轴缩放关闭
                        IsPanEnabled = false,  //图表缩放功能关闭
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        MajorStep = axisList[0].YMajorStep,
                        Minimum = axisList[0].YMinimum,
                        Maximum = axisList[0].YMaximum,
                        //IntervalLength = 50
                    };
                    curveModel = new PlotModel() { Title = "遥测参数曲线" };  //线条
                    curveModel.Axes.Add(xAxisCurve);
                    curveModel.Axes.Add(yAxisCurve);
                    curvePoints = new List<DataPoint>();
                }
            }
        }
        
        #endregion
    }
}
