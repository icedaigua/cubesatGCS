using System;
using System.Windows.Controls;
using System.Windows.Media;



namespace HouseKeeping_Wpf
{
    /// <summary>
    /// sensor.xaml 的交互逻辑
    /// </summary>
    public partial class sensor : UserControl
    {

        const byte  ANTS1 = (0x01 << 0),       //第一个字节
                    ANTS2 = (0x01 << 1),
                    ANTS3 = (0x01 << 2),
                    ANTS4 = (0x01 << 3),
                    ARM = (0x01 << 4),
                    //ANTSMSK = (0x1F),       //No sue

                    PANELA = (0x01 << 5),
                    PANELB = (0x01 << 6),

                    GPS_EN = (0x01 << 0),      //第二个字节
                    ANTS_EN = (0x01 << 1),
                    FI_5V_EN = (0x01 << 2),
                    FI_3V_EN = (0x01 << 3),
                    ADCS_EN = (0x01 << 4),
                    PANEL_EN = (0x01 << 5),
                    HEAT_EN = (0x01 << 6),

                    //0x21    
                    M1_POWER_MASK = 0x01,        //第三个字节
                    M2_POWER_MASK = 0x02,
                    M3_POWER_MASK = 0x04,
                    M4_POWER_MASK = 0x08,
                    OUT_EN_5V = 0x10,       //No use
                    GR_POWER_MASK = 0x20,   //陀螺仪
                                            //0x20          =             
                    MAG_POWER_MASK = 0x40,
                    GPS_POWER_MASK = 0x80,


                    //0x20                     
                    MAGA_EN_MASK = 0x01,    //第四个字节
                    MAGB_EN_MASK = 0x02,
                    GPSA_EN_MASK = 0x04,    //No use
                    GPSB_EN_MASK = 0x08,    //No use 
                                            //0x21         
                    MAGBAR_EN_MASK = 0x10;  //磁棒


        public sensor()
        {
            InitializeComponent();

        }

        public void dis_cubesat_status(UInt32 sensor_onoff, byte sat_sta)
        {

            byte[] c = System.BitConverter.GetBytes(sensor_onoff);

            set_label_color(c);
            set_atenna_color(c[0]);
            //set_sat_color(sat_sta);
        }

        private void set_atenna_color(byte atenna_sat)
        {
            tB_atenna_1.Text = ((atenna_sat & 0x01) == 1) ? "Green" : "Red";
            tB_atenna_2.Text = ((atenna_sat & 0x02) == 2) ? "Green" : "Red";
            tB_atenna_3.Text = ((atenna_sat & 0x04) == 4) ? "Green" : "Red";
            tB_atenna_4.Text = ((atenna_sat & 0x08) == 8) ? "Green" : "Red";
            tB_atenna_arm.Text = ((atenna_sat & 0x10) == 16) ? "Green" : "Red";

        }

        public void set_sat_color(byte sat_sta)
        {
            tB_dam.Text = ((sat_sta & 0x01) == 1) ? "Green" : "Red";
            tB_pitch.Text = ((sat_sta & 0x02) == 2) ? "Green" : "Red";
            tB_ctrl.Text = ((sat_sta & 0x04) == 4) ? "Green" : "Red";
            tB_always_dam.Text = ((sat_sta & 0x40) == 0x40) ? "Green" : "Red";
        }

        private void set_label_color(byte[] sensor_onoff)
        {

          

            if ((sensor_onoff[1] & ADCS_EN) == ADCS_EN)  //2
            {
                tB_gps_b.Text = "Green";
            }
            else
            {
                tB_gps_b.Text = "Red";
            }

            if ((sensor_onoff[2] & M1_POWER_MASK) == M1_POWER_MASK) //3
            {
                tB_mw_a.Text = "Green";
            }
            else
            {
                tB_mw_a.Text = "Red";
            }

            if ((sensor_onoff[2] & M2_POWER_MASK) == M2_POWER_MASK)  //4
            {
                tB_mw_b.Text = "Green";
            }
            else
            {
                tB_mw_b.Text = "Red";
            }

            if ((sensor_onoff[3] & MAGA_EN_MASK) == MAGA_EN_MASK)      //5
            {
                tB_hmr_a.Text = "Green";
            }
            else
            {
                tB_hmr_a.Text = "Red";
            }

            if ((sensor_onoff[3] & MAGB_EN_MASK) == MAGB_EN_MASK)      //6
            {
                tB_hmr_b.Text = "Green";
            }
            else
            {
                tB_hmr_b.Text = "Red";
            }


            if ((sensor_onoff[2] & MAG_POWER_MASK) == MAG_POWER_MASK)      //7
            {
                tB_hmr_pwr.Text = "Green";
            }
            else
            {
                tB_hmr_pwr.Text = "Red";
            }

         

            if ((sensor_onoff[1] & ANTS_EN) == ANTS_EN)    //10
            {
                tB_atenna_pwr.Text = "Green";
            }
            else
            {
                tB_atenna_pwr.Text = "Red";
            }



        }
    }
}
