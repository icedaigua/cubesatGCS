using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using satCompent.Model;

namespace satCompent.ViewModel
{
    public class OrbitViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<ElementModel> orbit;
        #endregion

        #region Property
        public ObservableCollection<ElementModel> Orbit
        {
            get { return orbit; }
            set 
            {
                orbit = value;
                RaisePropertyChanged("Orbit");
            }
        }
        #endregion

        #region Constructor
        public OrbitViewModel()
        {
            orbit = ElementModel.GetOrbit();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfOrbit>(this, "ORBS", HandleOrbit);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void HandleOrbit(MsgOfOrbit msg)
        {
            DateTime dt = new DateTime(msg.year, msg.month, msg.day, msg.hour, msg.minute, (int)msg.second);

            orbit[0].Val = string.Format("{0}/{1}/{2} {3}:{4}:{5}.{6}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            orbit[1].Val = Convert.ToString(msg.axis);  //轨道半长轴a(m)
            orbit[2].Val = Convert.ToString(msg.ecc);  //轨道偏心率e
            orbit[3].Val = Convert.ToString(msg.dip);  //轨道倾角i
            orbit[4].Val = Convert.ToString(msg.lng);  //升交点赤经Ω
            orbit[5].Val = Convert.ToString(msg.arg);  //近地点幅角ω
            orbit[6].Val = Convert.ToString(msg.mean);  //平近点角M
        }
        #endregion
    }
}
