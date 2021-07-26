using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;


namespace satCompent.Model
{
    public class ResultModel : ObservableObject
    {
        #region Field
        public int Grid { get; set; }
        private string no;
        private string name;
        private string val;
        private string unit;
        private string exceed;
        #endregion

        #region Property
        public string No
        {
            get { return no; }
            set
            {
                no = value;
                RaisePropertyChanged("No");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public string Val
        {
            get { return val; }
            set
            {
                val = value;
                RaisePropertyChanged("Val");
            }
        }
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                RaisePropertyChanged("Unit");
            }
        }
        public string Exceed
        {
            get { return exceed; }
            set
            {
                exceed = value;
                RaisePropertyChanged("Exceed");
            }
        }
        #endregion

        #region Method
        public static ObservableCollection<ResultModel> GetAllResult()
        {
            ObservableCollection<ResultModel> list = new ObservableCollection<ResultModel>();

            list.Add(new ResultModel() { No = "0", Name = "帆板电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "1", Name = "+X帆板电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "2", Name = "+X帆板温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "3", Name = "-X帆板电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "4", Name = "-X帆板温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "5", Name = "+Y帆板电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "6", Name = "+Y帆板温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "7", Name = "-Y帆板电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "8", Name = "-Y帆板温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "9", Name = "电池组电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "10", Name = "电池组电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "11", Name = "电池组温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "12", Name = "+14V母线电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "13", Name = "+14V母线电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "14", Name = "+12V供电电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "15", Name = "+12V供电电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "16", Name = "-12V供电电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "17", Name = "-12V供电电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "18", Name = "+5V供电电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "19", Name = "+5V供电电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "20", Name = "PCM电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "21", Name = "BD2电压", Val = string.Empty, Unit = "V", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "22", Name = "OBC电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "23", Name = "OBC工作温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "24", Name = "TX电流", Val = string.Empty, Unit = "mA", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "25", Name = "RF温度", Val = string.Empty, Unit = "℃", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "26", Name = "RX AGC电平", Val = string.Empty, Unit = "dBm", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "27", Name = "RX AFC电平", Val = string.Empty, Unit = "MHz", Exceed = string.Empty });
            list.Add(new ResultModel() { No = "28", Name = "TX 功放输出", Val = string.Empty, Unit = "mW", Exceed = string.Empty });

            //JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("tm/result.json"));
            //for (int i = 0; i < item[0].Count; i++)
            //    list.Add(new ResultModel() { No = Convert.ToString(item[0][i]["No"]), Name = Convert.ToString(item[0][i]["Name"]), Val = string.Empty, Unit = Convert.ToString(item[0][i]["Unit"]), Exceed = string.Empty });

            return list;
        }
        #endregion
    }
}
