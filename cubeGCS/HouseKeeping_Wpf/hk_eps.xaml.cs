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

            //tB_batt_on_off.Text = eps.on_off_status.ToString();

            tB_5_33V_1.Text = (((eps.eps_switch_status & 0x0001)>0)?1:0).ToString();
            tB_5_33V_2.Text = (((eps.eps_switch_status & 0x0002) > 0) ? 1 : 0).ToString();
            tB_5_33V_3.Text = (((eps.eps_switch_status & 0x0004) > 0) ? 1 : 0).ToString();
            tB_5_33V_4.Text = (((eps.eps_switch_status & 0x0008) > 0) ? 1 : 0).ToString();
            tB_5_33V_5.Text = (((eps.eps_switch_status & 0x0010) > 0) ? 1 : 0).ToString();
            tB_5_33V_6.Text = (((eps.eps_switch_status & 0x0020) > 0) ? 1 : 0).ToString();

            tB_7V_1.Text = (((eps.eps_switch_status & 0x0100) > 0) ? 1 : 0).ToString();
            tB_7V_2.Text = (((eps.eps_switch_status & 0x0200) > 0) ? 1 : 0).ToString();
            tB_7V_3.Text = (((eps.eps_switch_status & 0x0400) > 0) ? 1 : 0).ToString();
            tB_7V_4.Text = (((eps.eps_switch_status & 0x0800) > 0) ? 1 : 0).ToString();
            tB_7V_5.Text = (((eps.eps_switch_status & 0x1000) > 0) ? 1 : 0).ToString();

        }
    }
}
