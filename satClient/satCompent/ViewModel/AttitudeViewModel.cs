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
    public class AttitudeViewModel : ViewModelBase
    {
        #region Field
        private PlotModel rollModel;
        private PlotModel yawModel;
        private PlotModel pitchModel;
        private double roll;
        private double yaw;
        private double pitch;

        private List<DataPoint> rollPoints;
        private List<DataPoint> yawPoints;
        private List<DataPoint> pitchPoints;
        #endregion

        #region Property
        public PlotModel RollModel
        {
            get { return rollModel; }
            set 
            { 
                rollModel = value;
                RaisePropertyChanged("RollModel");
            }
        }
        public PlotModel YawModel
        {
            get { return yawModel; }
            set
            { 
                yawModel = value;
                RaisePropertyChanged("YawModel");
            }
        }
        public PlotModel PitchModel
        {
            get { return pitchModel; }
            set 
            { 
                pitchModel = value;
                RaisePropertyChanged("PitchModel");
            }
        }
        public double Roll
        {
            get { return roll; }
            set 
            { 
                roll = value;
                RaisePropertyChanged("Roll");
            }
        }
        public double Yaw
        {
            get { return yaw; }
            set 
            { 
                yaw = value;
                RaisePropertyChanged("Yaw");
            }
        }
        public double Pitch
        {
            get { return pitch; }
            set
            { 
                pitch = value;
                RaisePropertyChanged("Pitch");
            }
        }
        #endregion

        #region Constructor
        public AttitudeViewModel()
        {
            InitAttitudeModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfAttitude>(this, "TMATTI", HandleAttitude);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitAttitudeModel()
        {
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_attitude.json"));

            DateTimeAxis xAxisRoll = new DateTimeAxis()
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
            LinearAxis yAxisRoll = new LinearAxis()
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
            DateTimeAxis xAxisYaw = new DateTimeAxis()
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
            LinearAxis yAxisYaw = new LinearAxis()
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
            DateTimeAxis xAxisPitch = new DateTimeAxis()
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
            LinearAxis yAxisPitch = new LinearAxis()
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

            rollModel = new PlotModel() { Title = "滚动角(º)" };  //线条
            rollModel.Axes.Add(xAxisRoll);
            rollModel.Axes.Add(yAxisRoll);
            rollPoints = new List<DataPoint>();

            yawModel = new PlotModel() { Title = "偏航角(º)" };  //线条
            yawModel.Axes.Add(xAxisYaw);
            yawModel.Axes.Add(yAxisYaw);
            yawPoints = new List<DataPoint>();

            pitchModel = new PlotModel() { Title = "俯仰角(º)" };  //线条
            pitchModel.Axes.Add(xAxisPitch);
            pitchModel.Axes.Add(yAxisPitch);
            pitchPoints = new List<DataPoint>();
        }

        private void HandleAttitude(MsgOfAttitude msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            if (rollPoints.Count > 3600)
            {
                rollPoints.RemoveRange(0, 3600);
                rollModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                rollModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (yawPoints.Count > 3600)
            {
                yawPoints.RemoveRange(0, 3600);
                yawModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                yawModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            if (pitchPoints.Count > 3600)
            {
                pitchPoints.RemoveRange(0, 3600);
                pitchModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                pitchModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }

            Roll = Math.Round(msg.roll, 2);
            rollPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Roll));
            LineSeries rollSerial = new LineSeries() { Title = "滚动角" };
            rollSerial.Points.AddRange(rollPoints);

            RollModel.Series.Clear();
            RollModel.Series.Add(rollSerial);
            RollModel.InvalidatePlot(true);  //刷新

            Yaw = Math.Round(msg.yaw, 2);
            yawPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Yaw));
            LineSeries yawSerial = new LineSeries() { Title = "偏航角" };
            yawSerial.Points.AddRange(yawPoints);

            YawModel.Series.Clear();
            YawModel.Series.Add(yawSerial);
            YawModel.InvalidatePlot(true);  //刷新

            Pitch = Math.Round(msg.pitch, 2);
            pitchPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Pitch));
            LineSeries pitchSerial = new LineSeries() { Title = "俯仰角" };
            pitchSerial.Points.AddRange(pitchPoints);

            PitchModel.Series.Clear();
            PitchModel.Series.Add(pitchSerial);
            PitchModel.InvalidatePlot(true);  //刷新
        }
        #endregion

    }
}
