using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GalaSoft.MvvmLight;

using LitJson;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class VoltViewModel : ViewModelBase
    {
        #region Field
        private PlotModel volt14Model;
        private PlotModel volt12PModel;
        private PlotModel volt12NModel;
        private PlotModel volt5Model;
        private float volt14;
        private float volt12P;
        private float volt12N;
        private float volt5;

        private List<DataPoint> volt14Points;
        private List<DataPoint> volt12PPoints;
        private List<DataPoint> volt12NPoints;
        private List<DataPoint> volt5Points;
        #endregion

        #region Property
        public PlotModel Volt14Model
        {
            get { return volt14Model; }
            set 
            {
                volt14Model = value;
                RaisePropertyChanged("Volt14Model");
            }
        }
        public PlotModel Volt12PModel
        {
            get { return volt12PModel; }
            set
            { 
                volt12PModel = value;
                RaisePropertyChanged("Volt12PModel");
            }
        }
        public PlotModel Volt12NModel
        {
            get { return volt12NModel; }
            set 
            { 
                volt12NModel = value;
                RaisePropertyChanged("Volt12NModel");
            }
        }
        public PlotModel Volt5Model
        {
            get { return volt5Model; }
            set
            { 
                volt5Model = value;
                RaisePropertyChanged("Volt5Model");
            }
        }
        public float Volt14
        {
            get { return volt14; }
            set 
            { 
                volt14 = value;
                RaisePropertyChanged("Volt14");
            }
        }
        public float Volt12P
        {
            get { return volt12P; }
            set
            { 
                volt12P = value;
                RaisePropertyChanged("Volt12P");
            }
        }
        public float Volt12N
        {
            get { return volt12N; }
            set 
            { 
                volt12N = value;
                RaisePropertyChanged("Volt12N");
            }
        }
        public float Volt5
        {
            get { return volt5; }
            set
            { 
                volt5 = value;
                RaisePropertyChanged("Volt5");
            }
        }
        #endregion

        #region Contructor
        public VoltViewModel()
        {
            InitVoltModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfVolt>(this, "TMVOLT", HandleVolt);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitVoltModel()
        {
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_volt.json"));

            DateTimeAxis xAxisVolt14 = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][0]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][0]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][0]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][0]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][0]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                //IntervalLength = 60
            };
            LinearAxis yAxisVolt14 = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = Convert.ToString(item[0][1]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][1]["TitlePosition"])),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][1]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][1]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = Double.Parse(Convert.ToString(item[0][1]["MajorStep"])),
                Minimum = Double.Parse(Convert.ToString(item[0][1]["Minimum"])),
                Maximum = Double.Parse(Convert.ToString(item[0][1]["Maximum"]))
            };
            volt14Model = new PlotModel() { Title = "+14V母线电压(V)" };  //线条
            volt14Model.Axes.Add(xAxisVolt14);
            volt14Model.Axes.Add(yAxisVolt14);
            volt14Points = new List<DataPoint>();

            DateTimeAxis xAxisVolt12P = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][2]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][2]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][2]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][2]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][2]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                //IntervalLength = 60
            };
            LinearAxis yAxisVolt12P = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = Convert.ToString(item[0][3]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][3]["TitlePosition"])),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][3]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][3]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = Double.Parse(Convert.ToString(item[0][3]["MajorStep"])),
                Minimum = Double.Parse(Convert.ToString(item[0][3]["Minimum"])),
                Maximum = Double.Parse(Convert.ToString(item[0][3]["Maximum"]))
            };
            volt12PModel = new PlotModel() { Title = "+12V供电电压(V)" };  //线条
            volt12PModel.Axes.Add(xAxisVolt12P);
            volt12PModel.Axes.Add(yAxisVolt12P);
            volt12PPoints = new List<DataPoint>();

            DateTimeAxis xAxisVolt12N = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][4]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][4]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][4]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][4]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][4]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                //IntervalLength = 60
            };
            LinearAxis yAxisVolt12N = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = Convert.ToString(item[0][5]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][5]["TitlePosition"])),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][5]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][5]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = Double.Parse(Convert.ToString(item[0][5]["MajorStep"])),
                Minimum = Double.Parse(Convert.ToString(item[0][5]["Minimum"])),
                Maximum = Double.Parse(Convert.ToString(item[0][5]["Maximum"]))
            };
            volt12NModel = new PlotModel() { Title = "-12V供电电压(V)" };  //线条
            volt12NModel.Axes.Add(xAxisVolt12N);
            volt12NModel.Axes.Add(yAxisVolt12N);
            volt12NPoints = new List<DataPoint>();

            DateTimeAxis xAxisVolt5 = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][6]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][6]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][6]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][6]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][6]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60)),
                //IntervalLength = 60
            };
            LinearAxis yAxisVolt5 = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = Convert.ToString(item[0][7]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][7]["TitlePosition"])),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][7]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][7]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorStep = Double.Parse(Convert.ToString(item[0][7]["MajorStep"])),
                Minimum = Double.Parse(Convert.ToString(item[0][7]["Minimum"])),
                Maximum = Double.Parse(Convert.ToString(item[0][7]["Maximum"]))
            };
            volt5Model = new PlotModel() { Title = "+5V供电电压(V)" };  //线条
            volt5Model.Axes.Add(xAxisVolt5);
            volt5Model.Axes.Add(yAxisVolt5);
            volt5Points = new List<DataPoint>();
        }

        private void HandleVolt(MsgOfVolt msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            if (volt14Points.Count > 3600)
            {
                volt14Points.RemoveRange(0, 3600);
                volt14Model.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                volt14Model.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (volt12PPoints.Count > 3600)
            {
                volt12PPoints.RemoveRange(0, 3600);
                volt12PModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                volt12PModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (volt12NPoints.Count > 3600)
            {
                volt12NPoints.RemoveRange(0, 3600);
                volt12NModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                volt12NModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (volt5Points.Count > 3600)
            {
                volt5Points.RemoveRange(0, 3600);
                volt5Model.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                volt5Model.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            Volt14 = msg.lineVolt14;
            volt14Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Volt14));
            LineSeries volt14Serial = new LineSeries() { Title = "+14V母线电压" };
            volt14Serial.Points.AddRange(volt14Points);

            Volt14Model.Series.Clear();
            Volt14Model.Series.Add(volt14Serial);
            Volt14Model.InvalidatePlot(true);  //刷新

            Volt12P = msg.lineVolt12P;
            volt12PPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Volt12P));
            LineSeries volt12PSerial = new LineSeries() { Title = "+12V供电电压" };
            volt12PSerial.Points.AddRange(volt12PPoints);

            Volt12PModel.Series.Clear();
            Volt12PModel.Series.Add(volt12PSerial);
            Volt12PModel.InvalidatePlot(true);  //刷新

            Volt12N = msg.lineVolt12N;
            volt12NPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Volt12N));
            LineSeries volt12NSerial = new LineSeries() { Title = "-12V供电电压" };
            volt12NSerial.Points.AddRange(volt12NPoints);

            Volt12NModel.Series.Clear();
            Volt12NModel.Series.Add(volt12NSerial);
            Volt12NModel.InvalidatePlot(true);  //刷新

            Volt5 = msg.lineVolt5;
            volt5Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Volt5));
            LineSeries volt5Serial = new LineSeries() { Title = "+5V供电电压" };
            volt5Serial.Points.AddRange(volt5Points);

            Volt5Model.Series.Clear();
            Volt5Model.Series.Add(volt5Serial);
            Volt5Model.InvalidatePlot(true);  //刷新
        }
        #endregion


    }
}
