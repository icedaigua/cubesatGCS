using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

using satCompent.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace satCompent.ViewModel
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
            //Messenger.Default.Send<int>(this.cv, "Curve");
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
            //using (IDbConnection connection = new SQLiteConnection(strConnection))
            //{
            //    List<CurveAxisModel> axisList = connection.Query<CurveAxisModel>("select * from t_curve_ns2 where curve = @cv", new { cv = this.cv }).AsList<CurveAxisModel>();  //从数据库中查询所有所属Id的记录
            //    if (axisList != null)  //加载
            //    {
            //        this.CurveModel.Axes[0].Title = axisList[0].XTitle;
            //        this.CurveModel.Axes[0].StringFormat = axisList[0].XStringFormat;
            //        this.CurveModel.Axes[1].Title = axisList[0].YTitle;
            //        this.CurveModel.Axes[1].MajorStep = axisList[0].YMajorStep;
            //        this.CurveModel.Axes[1].Minimum = axisList[0].YMinimum;
            //        this.CurveModel.Axes[1].Maximum = axisList[0].YMaximum;

            //        this.CurveModel.InvalidatePlot(true);
            //    }
            //}
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

            //this.CurveModel.InvalidatePlot(true);
            
            var lineSerial = new LineSeries() { Title = "直线实例" };
            lineSerial.Points.Add(new DataPoint(0, 0));
            lineSerial.Points.Add(new DataPoint(10, 10));
            CurveModel.Series.Add(lineSerial);

            //函数sin(x)
            var funcSerial = new FunctionSeries((x) => { return Math.Sin(x); }, 0, 10, 0.1, "y=sin(x)");
            CurveModel.Series.Add(funcSerial);


        }

        private void LoadCurveModel(string strSql)
        {
           // using (IDbConnection connection = new SQLiteConnection(strConnection))
            {
                //List<CurveAxisModel> axisList = connection.Query<CurveAxisModel>(strSql, new { cv = this.cv }).AsList<CurveAxisModel>();  //从数据库中查询所有所属Id的记录
                //if (axisList != null)  //加载
                {
                    DateTimeAxis xAxisCurve = new DateTimeAxis()
                    {
                        Position = AxisPosition.Bottom,
                        Title = "xxxx",//axisList[0].XTitle,
                        TitlePosition = 1,
                        StringFormat = "HH:mm:ss",//axisList[0].XStringFormat,
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
                        Title = "yyyy",//axisList[0].YTitle,
                        TitlePosition = 1,
                        IsZoomEnabled = false,  //坐标轴缩放关闭
                        IsPanEnabled = false,  //图表缩放功能关闭
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        MajorStep = 800,//axisList[0].YMajorStep,
                        Minimum = 0,//axisList[0].YMinimum,
                        Maximum = 1600,//axisList[0].YMaximum,
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
