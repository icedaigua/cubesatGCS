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
    public class CurrViewModel : ViewModelBase
    {
        #region Field
        private PlotModel curr14Model;
        private PlotModel curr12PModel;
        private PlotModel curr12NModel;
        private PlotModel curr5Model;
        private float curr14;
        private float curr12P;
        private float curr12N;
        private float curr5;

        private List<DataPoint> curr14Points;
        private List<DataPoint> curr12PPoints;
        private List<DataPoint> curr12NPoints;
        private List<DataPoint> curr5Points;
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
        public float Curr14
        {
            get { return curr14; }
            set
            {
                curr14 = value;
                RaisePropertyChanged("Curr14");
            }
        }
        public float Curr12P
        {
            get { return curr12P; }
            set
            {
                curr12P = value;
                RaisePropertyChanged("Curr12P");
            }
        }
        public float Curr12N
        {
            get { return curr12N; }
            set
            {
                curr12N = value;
                RaisePropertyChanged("Curr12N");
            }
        }
        public float Curr5
        {
            get { return curr5; }
            set
            {
                curr5 = value;
                RaisePropertyChanged("Curr5");
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
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_curr.json"));

            DateTimeAxis xAxisCurr14 = new DateTimeAxis()
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
            LinearAxis yAxisCurr14 = new LinearAxis()
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
            curr14Model = new PlotModel() { Title = "+14V母线电流(mA)" };  //线条
            curr14Model.Axes.Add(xAxisCurr14);
            curr14Model.Axes.Add(yAxisCurr14);
            curr14Points = new List<DataPoint>();

            DateTimeAxis xAxisCurr12P = new DateTimeAxis()
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
            LinearAxis yAxisCurr12P = new LinearAxis()
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
            curr12PModel = new PlotModel() { Title = "+12V供电电流(mA)" };  //线条
            curr12PModel.Axes.Add(xAxisCurr12P);
            curr12PModel.Axes.Add(yAxisCurr12P);
            curr12PPoints = new List<DataPoint>();

            DateTimeAxis xAxisCurr12N = new DateTimeAxis()
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
            LinearAxis yAxisCurr12N = new LinearAxis()
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
            curr12NModel = new PlotModel() { Title = "-12V供电电流(mA)" };  //线条
            curr12NModel.Axes.Add(xAxisCurr12N);
            curr12NModel.Axes.Add(yAxisCurr12N);
            curr12NPoints = new List<DataPoint>();

            DateTimeAxis xAxisCurr5 = new DateTimeAxis()
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
            LinearAxis yAxisCurr5 = new LinearAxis()
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
            curr5Model = new PlotModel() { Title = "+5V供电电流(mA)" };  //线条
            curr5Model.Axes.Add(xAxisCurr5);
            curr5Model.Axes.Add(yAxisCurr5);
            curr5Points = new List<DataPoint>();
        }

        private void HandleCurr(MsgOfCurr msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            if (curr14Points.Count > 3600)
            {
                curr14Points.RemoveRange(0, 3600);

                curr14Model.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                curr14Model.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (curr12PPoints.Count > 3600)
            {
                curr12PPoints.RemoveRange(0, 3600);

                curr12PModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                curr12PModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (curr12NPoints.Count > 3600)
            {
                curr12NPoints.RemoveRange(0, 3600);

                curr12NModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                curr12NModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (curr5Points.Count > 3600)
            {
                curr5Points.RemoveRange(0, 3600);

                curr5Model.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                curr5Model.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }

            Curr14 = msg.lineCurr14;
            curr14Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr14));
            LineSeries curr14Serial = new LineSeries() { Title = "+14V母线电流" };
            curr14Serial.Points.AddRange(curr14Points);

            Curr14Model.Series.Clear();
            Curr14Model.Series.Add(curr14Serial);
            Curr14Model.InvalidatePlot(true);  //刷新

            Curr12P = msg.lineCurr12P;
            curr12PPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr12P));
            LineSeries curr12PSerial = new LineSeries() { Title = "+12V供电电流" };
            curr12PSerial.Points.AddRange(curr12PPoints);

            Curr12PModel.Series.Clear();
            Curr12PModel.Series.Add(curr12PSerial);
            Curr12PModel.InvalidatePlot(true);  //刷新

            Curr12N = msg.lineCurr12N;
            curr12NPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr12N));
            LineSeries curr12NSerial = new LineSeries() { Title = "-12V供电电流" };
            curr12NSerial.Points.AddRange(curr12NPoints);

            Curr12NModel.Series.Clear();
            Curr12NModel.Series.Add(curr12NSerial);
            Curr12NModel.InvalidatePlot(true);  //刷新

            Curr5 = msg.lineCurr5;
            curr5Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Curr5));
            LineSeries curr5Serial = new LineSeries() { Title = "+5V供电电流" };
            curr5Serial.Points.AddRange(curr5Points);

            Curr5Model.Series.Clear();
            Curr5Model.Series.Add(curr5Serial);
            Curr5Model.InvalidatePlot(true);  //刷新
        }
        #endregion
    }
}
