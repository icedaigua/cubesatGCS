using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using satCompent.Model;
using System;
using System.Collections.Generic;

namespace satCompent.ViewModel
{
    public class CurrViewModel : ViewModelBase
    {
        #region Field
        private PlotModel curr14Model;
        private PlotModel curr12PModel;
        private PlotModel curr12NModel;
        private PlotModel curr5Model;
        #endregion

        #region Property
        public PlotModel Curr14Model
        {
            get { return curr14Model; }
            set
            {
                curr14Model = value;
                RaisePropertyChanged("Curr14Model");
            }
        }
        public PlotModel Curr12PModel
        {
            get { return curr12PModel; }
            set
            {
                curr12PModel = value;
                RaisePropertyChanged("Curr12PModel");
            }
        }
        public PlotModel Curr12NModel
        {
            get { return curr12NModel; }
            set
            {
                curr12NModel = value;
                RaisePropertyChanged("Curr12NModel");
            }
        }
        public PlotModel Curr5Model
        {
            get { return curr5Model; }
            set
            {
                curr5Model = value;
                RaisePropertyChanged("Curr5Model");
            }
        }
        #endregion

        #region Contructor
        public CurrViewModel()
        {
            InitCurrModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfCurr>(this, "TMCURR", HandleCurr);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitCurrModel()
        {

            DateTimeAxis xAxisCurr14 = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "xyz",
                TitlePosition = 1.0,
                StringFormat = "HH:mm:ss",
                IsZoomEnabled = false, //坐标轴缩放关闭
                IsPanEnabled = false, //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60))
                //IntervalLength = 60
            };
            LinearAxis yAxisCurr14 = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = "ppp",
                TitlePosition = 1.0,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = 800,
                Minimum = 0.0,
                Maximum = 1600.0
            };
            curr14Model = new PlotModel() { Title = "+14V母线电流(mA)" };  //线条
            curr14Model.Axes.Add(xAxisCurr14);
            curr14Model.Axes.Add(yAxisCurr14);


            var series = new LineSeries
            {
                Title = "P & L",
                DataFieldX = "Time",
                DataFieldY = "Value",
                Color = OxyColor.Parse("#4CAF50"),
                MarkerSize = 3,
                MarkerFill = OxyColor.Parse("#FFFFFFFF"),
                MarkerStroke = OxyColor.Parse("#4CAF50"),
                MarkerStrokeThickness = 1.5,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
            };
            curr14Model.Series.Add(series);
            //curr14Points = new List<DataPoint>();
            //Curr14Model = new PlotModel() { Title = "+14V母线电流(mA)" };  //线条
            //Curr14Model.Axes.Add(xAxisCurr14);
            //Curr14Model.Axes.Add(yAxisCurr14);


        }

        private void HandleCurr(MsgOfCurr msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            Curr14Model.Series.Clear();
            //Curr14Model.Series.Add(curr14Serial);
            Curr14Model.InvalidatePlot(true);  //刷新

            //Curr12P = msg.lineCurr12P;
            //curr12PPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr12P));
            LineSeries curr12PSerial = new LineSeries() { Title = "+12V供电电流" };
            //curr12PSerial.Points.AddRange(curr12PPoints);

            Curr12PModel.Series.Clear();
            Curr12PModel.Series.Add(curr12PSerial);
            Curr12PModel.InvalidatePlot(true);  //刷新

            //Curr12N = msg.lineCurr12N;
            //curr12NPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr12N));
            LineSeries curr12NSerial = new LineSeries() { Title = "-12V供电电流" };
            //curr12NSerial.Points.AddRange(curr12NPoints);

            Curr12NModel.Series.Clear();
            Curr12NModel.Series.Add(curr12NSerial);
            Curr12NModel.InvalidatePlot(true);  //刷新

            //Curr5 = msg.lineCurr5;
            //curr5Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr5));
            LineSeries curr5Serial = new LineSeries() { Title = "+5V供电电流" };
            //curr5Serial.Points.AddRange(curr5Points);

            Curr5Model.Series.Clear();
            Curr5Model.Series.Add(curr5Serial);
            Curr5Model.InvalidatePlot(true);  //刷新
        }
        #endregion
    }
}
