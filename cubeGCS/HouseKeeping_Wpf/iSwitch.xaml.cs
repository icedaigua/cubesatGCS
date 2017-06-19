using System.Windows.Controls;

namespace HouseKeeping_Wpf
{

    public partial class iSwitch : UserControl

    {
        public iSwitch()
        {
            InitializeComponent();
        }

        public void SetSwitchName(string name)
        {
            tBk_sensor.Text = name;
        }

        public void SetSwitchColor(string color)
        {
            tB_sensor.Text = color;
        }
    }
}
