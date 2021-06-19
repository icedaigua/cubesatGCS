using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using GalaSoft.MvvmLight;

using LitJson;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class StationViewModel : ViewModelBase
    {
        #region Field
        private PlotModel distModel;
        private PlotModel speedModel;
        private PlotModel azimuthModel;
        private PlotModel pitchModel;
        private double dist;
        private double speed;
        private double azimuth;
        private double pitch;

        private List<DataPoint> distPoints;
        private List<DataPoint> speedPoints;
        private List<DataPoint> azimuthPoints;
        private List<DataPoint> pitchPoints;
        private List<DateTime> times;
        private List<double> dists;
        private List<double> speeds;
        private List<double> azimuths;
        private List<double> pitchs;
        private int counter;
        #endregion

        #region Property
        public PlotModel DistModel
        {
            get { return distModel; }
            set
            { 
                distModel = value;
                RaisePropertyChanged("DistModel");
            }
        }
        public PlotModel SpeedModel
        {
            get { return speedModel; }
            set
            { 
                speedModel = value;
                RaisePropertyChanged("SpeedModel");
            }
        }
        public PlotModel AzimuthModel
        {
            get { return azimuthModel; }
            set 
            { 
                azimuthModel = value;
                RaisePropertyChanged("AzimuthModel");
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
        public double Dist
        {
            get { return dist; }
            set
            { 
                dist = value;
                RaisePropertyChanged("Dist");
            }
        }
        public double Speed
        {
            get { return speed; }
            set 
            { 
                speed = value;
                RaisePropertyChanged("Speed");
            }
        }
        public double Azimuth
        {
            get { return azimuth; }
            set 
            { 
                azimuth = value;
                RaisePropertyChanged("Azimuth");
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
        public StationViewModel()
        {
            InitStationData();
            InitStationModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<byte[]>(this, "TMCODE", HandleStation);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitStationData()
        {
            times = new List<DateTime>();
            dists = new List<double>();
            speeds = new List<double>();
            azimuths = new List<double>();
            pitchs = new List<double>();

            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\data\\station.txt", Encoding.Default))
            {
                string content = string.Empty;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortTimePattern = "HH:mm:ss.fff";
                while ((content = sr.ReadLine()) != null)
                {
                    string[] station = content.Split('\t');
                    DateTime dt = Convert.ToDateTime(station[1], dtFormat);
                    double d = double.Parse(station[2]) / 1000.0;
                    double s = double.Parse(station[3]);
                    double a = double.Parse(station[4]);
                    double p = double.Parse(station[5]);
                    times.Add(dt);
                    dists.Add(d);
                    speeds.Add(s);
                    azimuths.Add(a);
                    pitchs.Add(p);
                }
            }
        }
        private void InitStationModel()
        {
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_station.json"));

            DateTimeAxis xAxisDist = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][0]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][0]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][0]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][0]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][0]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(times[0]),
                Maximum = DateTimeAxis.ToDouble(times[times.Count - 1]),
                //Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                //Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(30)),
                //IntervalLength = 60
            };
            LinearAxis yAxisDist = new LinearAxis()
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
                Maximum = Double.Parse(Convert.ToString(item[0][1]["Maximum"])),
                //IntervalLength = 2
            };
            distModel = new PlotModel() { Title = "测距(km)" };  //线条
            distModel.Axes.Add(xAxisDist);
            distModel.Axes.Add(yAxisDist);
            distPoints = new List<DataPoint>();

            DateTimeAxis xAxisSpeed = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][2]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][2]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][2]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][2]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][2]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(times[0]),
                Maximum = DateTimeAxis.ToDouble(times[times.Count - 1]),
                //Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                //Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(30)),
                //IntervalLength = 60
            };
            LinearAxis yAxisSpeed = new LinearAxis()
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
                Maximum = Double.Parse(Convert.ToString(item[0][3]["Maximum"])),
                //IntervalLength = 2
            };
            speedModel = new PlotModel() { Title = "测速(m/s)" };  //线条
            speedModel.Axes.Add(xAxisSpeed);
            speedModel.Axes.Add(yAxisSpeed);
            speedPoints = new List<DataPoint>();

            DateTimeAxis xAxisAzimuth = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][4]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][4]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][4]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][4]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][4]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(times[0]),
                Maximum = DateTimeAxis.ToDouble(times[times.Count - 1]),
                //Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                //Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(30)),
                //IntervalLength = 60
            };
            LinearAxis yAxisAzimuth = new LinearAxis()
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
                Maximum = Double.Parse(Convert.ToString(item[0][5]["Maximum"])),
                //IntervalLength = 2
            };
            azimuthModel = new PlotModel() { Title = "方位角(º)" };  //线条
            azimuthModel.Axes.Add(xAxisAzimuth);
            azimuthModel.Axes.Add(yAxisAzimuth);
            azimuthPoints = new List<DataPoint>();

            DateTimeAxis xAxisPitch = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = Convert.ToString(item[0][6]["Title"]),
                TitlePosition = Double.Parse(Convert.ToString(item[0][6]["TitlePosition"])),
                StringFormat = Convert.ToString(item[0][6]["StringFormat"]),
                IsZoomEnabled = Boolean.Parse(Convert.ToString(item[0][6]["IsZoomEnabled"])),  //坐标轴缩放关闭
                IsPanEnabled = Boolean.Parse(Convert.ToString(item[0][6]["IsPanEnabled"])),  //图表缩放功能关闭
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(times[0]),
                Maximum = DateTimeAxis.ToDouble(times[times.Count - 1]),
                //Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                //Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(30)),
                //IntervalLength = 60
            };
            LinearAxis yAxisPitch = new LinearAxis()
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
                Maximum = Double.Parse(Convert.ToString(item[0][7]["Maximum"])),
                //IntervalLength = 2
            };
            pitchModel = new PlotModel() { Title = "俯仰角(º)" };  //线条
            pitchModel.Axes.Add(xAxisPitch);
            pitchModel.Axes.Add(yAxisPitch);
            pitchPoints = new List<DataPoint>();
        }

        private void HandleStation(byte[] data)
        {
            if (++counter >= times.Count)
            {
                counter = 0;
                distPoints.Clear();
                speedPoints.Clear();
                azimuthPoints.Clear();
                pitchPoints.Clear();
            }

            Dist = dists[counter];
            distPoints.Add(new DataPoint(DateTimeAxis.ToDouble(times[counter]), Dist));
            LineSeries distSerial = new LineSeries() { Title = "测距" };
            distSerial.Points.AddRange(distPoints);

            DistModel.Series.Clear();
            DistModel.Series.Add(distSerial);
            DistModel.InvalidatePlot(true);  //刷新

            Speed = speeds[counter];
            speedPoints.Add(new DataPoint(DateTimeAxis.ToDouble(times[counter]), Speed));
            LineSeries speedSerial = new LineSeries() { Title = "测速" };
            speedSerial.Points.AddRange(speedPoints);

            SpeedModel.Series.Clear();
            SpeedModel.Series.Add(speedSerial);
            SpeedModel.InvalidatePlot(true);  //刷新

            Azimuth = azimuths[counter];
            azimuthPoints.Add(new DataPoint(DateTimeAxis.ToDouble(times[counter]), Azimuth));
            LineSeries azimuthSerial = new LineSeries() { Title = "方位角" };
            azimuthSerial.Points.AddRange(azimuthPoints);

            AzimuthModel.Series.Clear();
            AzimuthModel.Series.Add(azimuthSerial);
            AzimuthModel.InvalidatePlot(true);  //刷新

            Pitch = pitchs[counter];
            pitchPoints.Add(new DataPoint(DateTimeAxis.ToDouble(times[counter]), Pitch));
            LineSeries pitchSerial = new LineSeries() { Title = "俯仰角" };
            pitchSerial.Points.AddRange(pitchPoints);

            PitchModel.Series.Clear();
            PitchModel.Series.Add(pitchSerial);
            PitchModel.InvalidatePlot(true);  //刷新
        }
        #endregion

    }
}
