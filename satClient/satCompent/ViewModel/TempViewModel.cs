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
    public class TempViewModel : ViewModelBase
    {
        #region Field
        private PlotModel tempRfModel;
        private float tempRf;
        private PlotModel tempObcModel;
        private float tempObc;
        private List<DataPoint> rfPoints;
        private List<DataPoint> obcPoints;
        #endregion

        #region Property
        public PlotModel TempRfModel
        {
            get { return tempRfModel; }
            set 
            {
                tempRfModel = value;
                RaisePropertyChanged("TempRfModel");
            }
        }
        public PlotModel TempObcModel
        {
            get { return tempObcModel; }
            set
            {
                tempObcModel = value;
                RaisePropertyChanged("TempObcModel");
            }
        }
        public float TempRf
        {
            get { return tempRf; }
            set
            { 
                tempRf = value;
                RaisePropertyChanged("TempRf");
            }
        }
        public float TempObc
        {
            get { return tempObc; }
            set 
            { 
                tempObc = value;
                RaisePropertyChanged("TempObc");
            }
        }
        #endregion

        #region Contructor
        public TempViewModel()
        {
            InitTempModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfTemp>(this, "TMTEMP", HandleTemp);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitTempModel()
        {
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_temp.json"));

            DateTimeAxis xAxisRf = new DateTimeAxis()
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
                IntervalLength = 60
            };
            LinearAxis yAxisRf = new LinearAxis()
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
                //IntervalLength = 300
            };
            tempRfModel = new PlotModel() { Title = "射频温度(℃)" };  //线条
            tempRfModel.Axes.Add(xAxisRf);
            tempRfModel.Axes.Add(yAxisRf);
            rfPoints = new List<DataPoint>();

            DateTimeAxis xAxisObc = new DateTimeAxis()
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
            LinearAxis yAxisObc = new LinearAxis()
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
            tempObcModel = new PlotModel() { Title = "OBC工作温度(℃)" };  //线条
            tempObcModel.Axes.Add(xAxisObc);
            tempObcModel.Axes.Add(yAxisObc);
            obcPoints = new List<DataPoint>();
        }

        private void HandleTemp(MsgOfTemp msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            if (rfPoints.Count > 3600)
            {
                rfPoints.RemoveRange(0, 3600);
                tempRfModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                tempRfModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }

            TempRf = msg.tempRf;
            rfPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), TempRf));
            LineSeries rfSerial = new LineSeries() { Title = "+X帆板温度" };
            rfSerial.Points.AddRange(rfPoints);

            TempRfModel.Series.Clear();
            TempRfModel.Series.Add(rfSerial);
            TempRfModel.InvalidatePlot(true);  //刷新

            if (obcPoints.Count > 3600)
            {
                obcPoints.RemoveRange(0, 3600);
                tempObcModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                tempObcModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            TempObc = msg.tempObc;
            obcPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), TempObc));
            LineSeries obcSerial = new LineSeries() { Title = "OBC工作温度" };
            obcSerial.Points.AddRange(obcPoints);

            TempObcModel.Series.Clear();
            TempObcModel.Series.Add(obcSerial);
            TempObcModel.InvalidatePlot(true);  //刷新
        }
        #endregion
    }
}
