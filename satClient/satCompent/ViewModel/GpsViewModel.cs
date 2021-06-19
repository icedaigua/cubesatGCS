using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class GpsViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<ElementModel> gps;
        #endregion

        #region Property
        public ObservableCollection<ElementModel> Gps
        {
          get { return gps; }
          set 
          {
              gps = value;
              RaisePropertyChanged("Gps");
          }
        }
        #endregion

        #region Constructor
        public GpsViewModel()
        {
            gps = ElementModel.GetGps();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfGps>(this, "TMGPS", HandleGps);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void HandleGps(MsgOfGps msg)
        {
            Gps[0].Val = Convert.ToString(msg.gpsWeek);
            Gps[1].Val = Convert.ToString(msg.gpsSecond);
            Gps[2].Val = Convert.ToString(msg.posX);
            Gps[3].Val = Convert.ToString(msg.posY);
            Gps[4].Val = Convert.ToString(msg.posZ);
            Gps[5].Val = Convert.ToString(msg.speedX);
            Gps[6].Val = Convert.ToString(msg.speedY);
            Gps[7].Val = Convert.ToString(msg.speedZ);
        }
        #endregion
    }
}
