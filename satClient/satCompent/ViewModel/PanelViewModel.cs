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
    public class PanelViewModel : ViewModelBase
    {
        #region Field
        private PlotModel panelVoltModel;
        private PlotModel panelCurrModel;
        private float panelVolt;
        private float panelCurrXP;
        private float panelCurrXN;
        private float panelCurrYP;
        private float panelCurrYN;
        private PlotModel panelTempModel;
        private float panelTempXP;
        private float panelTempXN;
        private float panelTempYP;
        private float panelTempYN;

        private List<DataPoint> voltPoints;
        private List<DataPoint> currXPPoints;
        private List<DataPoint> currXNPoints;
        private List<DataPoint> currYPPoints;
        private List<DataPoint> currYNPoints;
        private List<DataPoint> tempXPPoints;
        private List<DataPoint> tempXNPoints;
        private List<DataPoint> tempYPPoints;
        private List<DataPoint> tempYNPoints;
        #endregion

        #region Property
        public PlotModel PanelVoltModel
        {
            get { return panelVoltModel; }
            set 
            { 
                panelVoltModel = value;
                RaisePropertyChanged("PanelVoltModel");
            }
        }

        public PlotModel PanelCurrModel
        {
            get { return panelCurrModel; }
            set 
            { 
                panelCurrModel = value;
                RaisePropertyChanged("PanelCurrModel");
            }
        }

        public float PanelVolt
        {
            get { return panelVolt; }
            set 
            { 
                panelVolt = value;
                RaisePropertyChanged("PanelVolt");
            }
        }
        
        public float PanelCurrXP
        {
            get { return panelCurrXP; }
            set 
            { 
                panelCurrXP = value;
                RaisePropertyChanged("PanelCurrXP");
            }
        }
        
        public float PanelCurrXN
        {
            get { return panelCurrXN; }
            set
            { 
                panelCurrXN = value;
                RaisePropertyChanged("PanelCurrXN");
            }
        }
        
        public float PanelCurrYP
        {
            get { return panelCurrYP; }
            set 
            { 
                panelCurrYP = value;
                RaisePropertyChanged("PanelCurrYP");
            }
        }
        
        public float PanelCurrYN
        {
            get { return panelCurrYN; }
            set 
            { 
                panelCurrYN = value;
                RaisePropertyChanged("PanelCurrYN");
            }
        }
        public PlotModel PanelTempModel
        {
            get { return panelTempModel; }
            set
            {
                panelTempModel = value;
                RaisePropertyChanged("PanelTempModel");
            }
        }
        public float PanelTempXP
        {
            get { return panelTempXP; }
            set 
            {
                panelTempXP = value;
                RaisePropertyChanged("PanelTempXP");
            }
        }
        public float PanelTempXN
        {
            get { return panelTempXN; }
            set 
            {
                panelTempXN = value;
                RaisePropertyChanged("PanelTempXN");
            }
        }
        public float PanelTempYP
        {
            get { return panelTempYP; }
            set 
            {
                panelTempYP = value;
                RaisePropertyChanged("PanelTempYP");
            }
        }
        public float PanelTempYN
        {
            get { return panelTempYN; }
            set 
            {
                panelTempYN = value;
                RaisePropertyChanged("PanelTempYN");
            }
        }
        #endregion

        #region Contructor
        public PanelViewModel()
        {
            InitPanelModel();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfPanel>(this, "TMPANEL", HandlePanel);

            //PanelVoltModel = new PlotModel();  //线条
            //var lineSerial = new LineSeries() { Title = "帆板电压" };
            //lineSerial.Points.Add(new DataPoint(0, 0));
            //lineSerial.Points.Add(new DataPoint(10, 10));
            //PanelVoltModel.Series.Add(lineSerial);

            //LinearAxis bottomAxis = new LinearAxis()  //定义x轴
            //{
            //    Position = AxisPosition.Bottom,
            //    Minimum = 0,
            //    Maximum = 10,
            //    Title = "X轴",  //显示标题内容
            //    TitlePosition = 1,  //显示标题位置
            //    TitleColor = OxyColor.Parse("#d3d3d3")  ,//显示标题位置
            //    IsZoomEnabled = false,  //坐标轴缩放关闭
            //    IsPanEnabled = false,  //图表缩放功能关闭
            //    //MajorGridlineStyle = LineStyle.Solid,  //主刻度设置格网
            //    //MajorGridlineColor = OxyColor.Parse("#7379a0"),
            //    //MinorGridlineStyle = LineStyle.Dot,  //子刻度设置格网样式
            //    //MinorGridlineColor = OxyColor.Parse("#666b8d")
            //};

            //LinearAxis leftAxis = new LinearAxis()  //定义y轴
            //{
            //    Position = AxisPosition.Left,
            //    Minimum = 0,
            //    Maximum = 10,
            //    Title = "Y轴",  //显示标题内容
            //    TitlePosition = 1,  //显示标题位置
            //    TitleColor = OxyColor.Parse("#d3d3d3"),  //显示标题位置
            //    IsZoomEnabled = false,  //坐标轴缩放关闭
            //    IsPanEnabled = false,  //图表缩放功能关闭
            //    //MajorGridlineStyle = LineStyle.Solid,  //主刻度设置格网
            //    //MajorGridlineColor = OxyColor.Parse("#7379a0"),
            //    //MinorGridlineStyle = LineStyle.Dot,  //子刻度设置格网样式
            //    //MinorGridlineColor = OxyColor.Parse("#666b8d")
            //};
            //PanelVoltModel.Axes.Add(leftAxis);
            //PanelVoltModel.Axes.Add(bottomAxis);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        private void InitPanelModel()
        {
            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("param_panel.json"));

            DateTimeAxis xAxisVolt = new DateTimeAxis() 
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
            };
            LinearAxis yAxisVolt = new LinearAxis()
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

            panelVoltModel = new PlotModel() { Title = "帆板电压(V)" };  //线条
            panelVoltModel.Axes.Add(xAxisVolt);
            panelVoltModel.Axes.Add(yAxisVolt);
            voltPoints = new List<DataPoint>();

            DateTimeAxis xAxisCurr = new DateTimeAxis()
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
                IntervalLength = 60
            };
            LinearAxis yAxisCurr = new LinearAxis()
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
                //IntervalLength = 300
            };

            panelCurrModel = new PlotModel() { Title = "帆板电流(mA)" };  //线条
            panelCurrModel.Axes.Add(xAxisCurr);
            panelCurrModel.Axes.Add(yAxisCurr);
            currXPPoints = new List<DataPoint>();
            currXNPoints = new List<DataPoint>();
            currYPPoints = new List<DataPoint>();
            currYNPoints = new List<DataPoint>();

            DateTimeAxis xAxisTemp = new DateTimeAxis()
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
                IntervalLength = 60
            };
            LinearAxis yAxisTemp = new LinearAxis()
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
                //IntervalLength = 300
            };
            panelTempModel = new PlotModel() { Title = "帆板温度(℃)" };  //线条
            panelTempModel.Axes.Add(xAxisTemp);
            panelTempModel.Axes.Add(yAxisTemp);
            tempXPPoints = new List<DataPoint>();
            tempXNPoints = new List<DataPoint>();
            tempYPPoints = new List<DataPoint>();
            tempYNPoints = new List<DataPoint>();
        }

        private void HandlePanel(MsgOfPanel msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            if (voltPoints.Count > 3600)
            {
                voltPoints.RemoveRange(0, 3600);
                panelVoltModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                panelVoltModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }
            PanelVolt = msg.panelVolt;
            voltPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelVolt));
            LineSeries voltSerial = new LineSeries() { Title = "帆板电压" };
            voltSerial.Points.AddRange(voltPoints);

            PanelVoltModel.Series.Clear();
            PanelVoltModel.Series.Add(voltSerial);
            PanelVoltModel.InvalidatePlot(true);  //刷新

            if (currXPPoints.Count > 3600)
            {
                currXPPoints.RemoveRange(0, 3600);
                currXNPoints.RemoveRange(0, 3600);
                currYPPoints.RemoveRange(0, 3600);
                currYNPoints.RemoveRange(0, 3600);
                panelCurrModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                panelCurrModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }


            PanelCurrXP = msg.panelCurrXP;
            currXPPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelCurrXP));
            LineSeries currXPSerial = new LineSeries() { Title = "+X帆板电流" };
            currXPSerial.Points.AddRange(currXPPoints);

            PanelCurrXN = msg.panelCurrXN;
            currXNPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelCurrXN));
            LineSeries currXNSerial = new LineSeries() { Title = "-X帆板电流" };
            currXNSerial.Points.AddRange(currXNPoints);

            PanelCurrYP = msg.panelCurrYP;
            currYPPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelCurrXP));
            LineSeries currYPSerial = new LineSeries() { Title = "+Y帆板电流" };
            currYPSerial.Points.AddRange(currYPPoints);

            PanelCurrYN = msg.panelCurrYN;
            currYNPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelCurrYN));
            LineSeries currYNSerial = new LineSeries() { Title = "-Y帆板电流" };
            currYNSerial.Points.AddRange(currYNPoints);

            PanelCurrModel.Series.Clear();
            PanelCurrModel.Series.Add(currXPSerial);
            PanelCurrModel.Series.Add(currXNSerial);
            PanelCurrModel.Series.Add(currYPSerial);
            PanelCurrModel.Series.Add(currYNSerial);
            PanelCurrModel.InvalidatePlot(true);  //刷新

            if (currXPPoints.Count > 3600)
            {
                tempXPPoints.RemoveRange(0, 3600);
                tempXNPoints.RemoveRange(0, 3600);
                tempYPPoints.RemoveRange(0, 3600);
                tempYNPoints.RemoveRange(0, 3600);
                panelTempModel.Axes[0].Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                panelTempModel.Axes[0].Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(60));
            }

            PanelTempXP = msg.panelTempXP;
            tempXPPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelTempXP));
            LineSeries tempXPSerial = new LineSeries() { Title = "+X帆板温度" };
            tempXPSerial.Points.AddRange(tempXPPoints);

            PanelTempXN = msg.panelTempXN;
            tempXNPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelTempXN));
            LineSeries tempXNSerial = new LineSeries() { Title = "-X帆板温度" };
            tempXNSerial.Points.AddRange(tempXNPoints);

            PanelTempYP = msg.panelTempYP;
            tempYPPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelTempYP));
            LineSeries tempYPSerial = new LineSeries() { Title = "+Y帆板温度" };
            tempYPSerial.Points.AddRange(tempYPPoints);

            PanelTempYN = msg.panelTempYN;
            tempYNPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dt), PanelTempYN));
            LineSeries tempYNSerial = new LineSeries() { Title = "-Y帆板温度" };
            tempYNSerial.Points.AddRange(tempYNPoints);

            PanelTempModel.Series.Clear();
            PanelTempModel.Series.Add(tempXPSerial);
            PanelTempModel.Series.Add(tempXNSerial);
            PanelTempModel.Series.Add(tempYPSerial);
            PanelTempModel.Series.Add(tempYNSerial);
            PanelTempModel.InvalidatePlot(true);  //刷新
        }
        #endregion

    }
}
