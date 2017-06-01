using System.Windows.Controls;
using CubeCOM;


namespace HouseKeeping_Wpf
{
    /// <summary>
    /// Eps.xaml 的交互逻辑
    /// </summary>
    public partial class hk_eps : UserControl
    {
        public hk_eps()
        {
            InitializeComponent();
        }


        public void display_eps_info(cubeCOMM.down_obc_ST eps)
        {
  
            tB_sunC_1.Text = eps.sun_c[0].ToString();
            tB_sunC_2.Text = eps.sun_c[1].ToString();
            tB_sunC_3.Text = eps.sun_c[2].ToString();
            tB_sunC_4.Text = eps.sun_c[3].ToString();
            tB_sunC_5.Text = eps.sun_c[4].ToString();
            tB_sunC_6.Text = eps.sun_c[5].ToString();

            tB_sunV_1.Text = eps.sun_v[0].ToString();
            tB_sunV_2.Text = eps.sun_v[1].ToString();
            tB_sunV_3.Text = eps.sun_v[2].ToString();
            tB_sunV_4.Text = eps.sun_v[3].ToString();
            tB_sunV_5.Text = eps.sun_v[4].ToString();
            tB_sunV_6.Text = eps.sun_v[5].ToString();

            tB_BusC.Text = eps.out_BusC.ToString();
            tB_BusV.Text = eps.out_BusV.ToString();


            tB_outC_1.Text = eps.Vol_5_C[0].ToString();
            tB_outC_2.Text = eps.Vol_5_C[1].ToString();
            tB_outC_3.Text = eps.Vol_5_C[2].ToString();
            tB_outC_4.Text = eps.Vol_5_C[3].ToString();
            tB_outC_5.Text = eps.Vol_5_C[4].ToString();
            tB_outC_6.Text = eps.Vol_5_C[5].ToString();

            tB_outBusC_1.Text = eps.Bus_c[0].ToString();
            tB_outBusC_2.Text = eps.Bus_c[1].ToString();
            tB_outBusC_3.Text = eps.Bus_c[2].ToString();
            tB_outBusC_4.Text = eps.Bus_c[3].ToString();
            tB_outBusC_5.Text = eps.Bus_c[4].ToString();

            tB_batt_tempe_1.Text = eps.temp_batt_board[0].ToString();
            tB_batt_tempe_2.Text = eps.temp_batt_board[1].ToString();
            tB_batt_tempe_3.Text = eps.temp_eps[0].ToString();
            tB_batt_tempe_4.Text = eps.temp_eps[1].ToString();
            tB_batt_tempe_5.Text = eps.temp_eps[2].ToString();
            tB_batt_tempe_6.Text = eps.temp_eps[3].ToString();

            tB_batt_on_off.Text = eps.on_off_status.ToString();

 
        }
    }
}
