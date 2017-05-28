using System.Windows.Controls;
using System;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// timerCNT.xaml 的交互逻辑
    /// </summary>
    public partial class timerCNT : UserControl
    {
        public timerCNT()
        {
            InitializeComponent();
        }


        public void setLocaltime(string localTime)
        {
            tBk_local_time.Text = localTime;
        }

        public void setCubetime(string cubeTime)
        {
            tBk_sat_time.Text = cubeTime;
        }

        public void setCounter(UInt32 localCNT,UInt16 cubeCNT)
        {
            tBk_sat_down_frame_cnt.Text = cubeCNT.ToString();
            tBk_rec_frame_cnt.Text = localCNT.ToString();
        }
    }
}
