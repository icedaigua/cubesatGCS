using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using LitJson;

namespace satCompent.Model
{
    public class ElementModel : ObservableObject
    {
        #region Field
        private string name;
        private string val;
        #endregion

        #region Property
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
        #endregion

        #region Method
        public static ObservableCollection<ElementModel> GetGps()
        {
            ObservableCollection<ElementModel> list = new ObservableCollection<ElementModel>();

            //list.Add(new ElementModel() { Name = "GPS周", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "GPS秒", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星位置X方向(m)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星位置Y方向(m)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星位置Z方向(m)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星速度X方向(m/s)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星速度Y方向(m/s)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "卫星速度Z方向(m/s)", Val = string.Empty });

            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("grid/gps.json"));
            for (int i = 0; i < item[0].Count; i++)
                list.Add(new ElementModel() {Name = Convert.ToString(item[0][i]["Name"]), Val = string.Empty });

            return list;
        }

        public static ObservableCollection<ElementModel> GetOrbit()
        {
            ObservableCollection<ElementModel> list = new ObservableCollection<ElementModel>();

            //list.Add(new ElementModel() { Name = "轨道历元时间", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "轨道半长轴a(m)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "轨道偏心率e", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "轨道倾角i(º)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "升交点赤经Ω(º)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "近地点幅角ω(º)", Val = string.Empty });
            //list.Add(new ElementModel() { Name = "平近点角M(º)", Val = string.Empty });

            JsonData item = JsonMapper.ToObject(System.IO.File.ReadAllText("settings/orbit.json"));
            for (int i = 0; i < item[0].Count; i++)
                list.Add(new ElementModel() { Name = Convert.ToString(item[0][i]["Name"]), Val = string.Empty });

            return list;
        }
        #endregion
    }
}
