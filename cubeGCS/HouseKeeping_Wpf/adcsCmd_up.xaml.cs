using CubeCOM;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// adcsCmd_up.xaml 的交互逻辑
    /// </summary>
    public partial class adcsCmd_up : UserControl
    {
        public adcsCmd_up()
        {
            InitializeComponent();
        }

        #region 控制指令

        public bool mwa_open_checked { get { return cB_mwa_open.IsChecked == true; } } //动量轮A
        public bool mwa_close_checked { get { return cB_mwa_close.IsChecked == true; } }
        public bool mwb_open_checked { get { return cB_mwb_close.IsChecked == true; } } //动量轮B
        public bool mwb_close_checked { get { return cB_mwb_close.IsChecked == true; } }

        public bool hmra_open_checked { get { return cB_hmra_open.IsChecked == true; } }      //磁强计
        public bool hmra_close_checked { get { return cB_hmra_open.IsChecked == true; } }
        public bool hmrb_open_checked { get { return cB_hmrb_open.IsChecked == true; } }
        public bool hmrb_close_checked { get { return cB_hmrb_open.IsChecked == true; } }

     

        public bool dam_mode_checked { get { return cB_dam_mode.IsChecked == true; } }      //控制模式
        public bool ctrl_mode_checked { get { return cB_ctrl_mode.IsChecked == true; } }
        public bool pitch_mode_checked { get { return cB_pitch_mode.IsChecked == true; } }
        public bool redam_checked { get { return cB_reDam.IsChecked == true; } }
        public bool always_dam_checked { get { return cB_always_dam.IsChecked == true; } }



        #endregion

        #region 参数注入



        public bool para_P_checked { get { return cB_P_para.IsChecked == true; } }
        public bool para_Z_checked { get { return cB_Z_para.IsChecked == true; } }
        public bool para_D_checked { get { return cB_D_para.IsChecked == true; } }

    

        public UInt32 para_P { get { return Convert.ToUInt32(tB_P_para.Text); } }
        public UInt32 para_D { get { return Convert.ToUInt32(tB_D_para.Text); } }
        public UInt32 para_Z { get { return Convert.ToUInt32(tB_Z_para.Text); } }


        #endregion

        #region 轨道注入
      
        public double[] orbit = new double[8];


        public bool orbit_checked = false;

        #endregion


        #region 控制指令生成
        /// <summary>
        /// 生成控制指令
        /// </summary>
        public void createCtrlCmd(byte[] up_buf, byte selectIndex, UInt32 delay_time, ref byte cmd_cnt)
        {

            if (mwa_open_checked || mwa_close_checked)    //动量轮A
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
                                        (mwa_open_checked) ? cubeCOMM.INS_MW_A_ON : cubeCOMM.INS_MW_A_OFF,
                                        delay_time
                                        );

                cmd_cnt++;
            }

            if (mwb_open_checked || mwb_close_checked)    //动量轮B
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
                                      (mwb_open_checked) ? cubeCOMM.INS_MW_B_ON : cubeCOMM.INS_MW_B_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }


        

            if (hmra_open_checked || hmra_close_checked)                                     //磁强计A
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
                                       (hmra_open_checked) ? cubeCOMM.INS_SW_MAG_A_ON : cubeCOMM.INS_SW_MAG_A_OFF,
                                      delay_time
                                      );

                cmd_cnt++;
            }

            if (hmrb_open_checked || hmrb_close_checked)                                     //磁强计B
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
                                      (hmrb_open_checked) ? cubeCOMM.INS_SW_MAG_B_ON : cubeCOMM.INS_SW_MAG_B_OFF,
                                      delay_time
                                      );
                cmd_cnt++;
            }


            if (redam_checked)//重阻尼
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex,
                    cubeCOMM.INS_DET,
                     delay_time);
                cmd_cnt++;
            }

            if (always_dam_checked)//永久阻尼
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_STA,
                     delay_time);
                cmd_cnt++;
            }

            if (dam_mode_checked) //阻尼置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_DMP_FLAG,
                     delay_time);

                cmd_cnt++;
            }

            if (ctrl_mode_checked) //三轴稳定置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_CTL_FLAG,
                     delay_time);

                cmd_cnt++;
            }

            if (pitch_mode_checked) //俯仰控制置位
            {
                cubeCOMM.generate_up_ctrl_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_FLT_FLAG,
                     delay_time);

                cmd_cnt++;
            }

        }


        #endregion

        #region 参数指令生成
        /// <summary>
        /// 生成注入指令
        /// </summary>
        public void createParametersCmd(byte[] up_buf, byte selectIndex, UInt32 delay_time, ref byte cmd_cnt)
        {


            if (para_D_checked)   //D参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_CTL_D_PRA,
                    delay_time,
                   para_D, 0);

                cmd_cnt++;
            }

            if (para_Z_checked)   //Z参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_ZJD_CTL,
                     delay_time,
                    para_Z, 0);

                cmd_cnt++;
            }

            if (para_P_checked)  //P参数注入
            {
                cubeCOMM.generate_up_para_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_CTL_P_PRA,
                     delay_time,
                    para_P, 0);

                cmd_cnt++;
            }


        }
        #endregion

        #region 轨道指令生成
        /// <summary>
        /// 生成轨道参数指令
        /// </summary>
        public void createOrbitCmd(byte[] up_buf, byte selectIndex, UInt32 delay_time, ref byte cmd_cnt)
        {
            if (orbit_checked)
            {
                cubeCOMM.generate_up_orbit_cmd_cs(up_buf, selectIndex, cubeCOMM.INS_ORB_TLE_FLAG,
                     delay_time,
                     orbit);

                cmd_cnt++;

            }


        }
        #endregion

        #region 动量轮
        /// <summary>
        /// 动量轮A开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_open_Checked(object sender, RoutedEventArgs e)
        {
            if (cB_mwa_close.IsChecked == true)
                cB_mwa_close.IsChecked = false;
        }

        /// <summary>
        /// 动量轮A关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwa_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_mwa_open.IsChecked == true)
                cB_mwa_open.IsChecked = false;

        }


        /// <summary>
        /// 动量轮B开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_open_Checked(object sender, RoutedEventArgs e)
        {
            if (cB_mwb_close.IsChecked == true)
                cB_mwb_close.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_mwb_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_mwb_open.IsChecked == true)
                cB_mwb_open.IsChecked = false;

        }

        #endregion




        #region 磁强计
        /// <summary>
        /// 磁强计A开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_open_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_hmra_close.IsChecked == true)
                cB_hmra_close.IsChecked = false;
        }

        /// <summary>
        /// 磁强计A关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmra_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_hmra_open.IsChecked == true)
                cB_hmra_open.IsChecked = false;

        }
        /// <summary>
        /// 磁强计B开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_open_Checked(object sender, RoutedEventArgs e)
        {
            if (cB_hmrb_close.IsChecked == true)
                cB_hmrb_close.IsChecked = false;

        }
        /// <summary>
        /// 磁强计B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_hmrb_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_hmrb_open.IsChecked == true)
                cB_hmrb_open.IsChecked = false;

        }

        #endregion



        #region  轨道参数
        private void cB_orbit_para_Checked(object sender, RoutedEventArgs e)
        {
            orbit_checked = true;


            string PATH = Directory.GetCurrentDirectory();
            StreamReader Orbit_File;                       ///实时数据存储文件  
            Orbit_File = new StreamReader(PATH + "\\resource\\orbit.txt");

            tB_TLEJdsatepoch.Text = Orbit_File.ReadLine();
            tB_TLEBstar.Text = Orbit_File.ReadLine();
            tB_TLEcco.Text = Orbit_File.ReadLine();
            tB_TLEInclo.Text = Orbit_File.ReadLine();
            tB_TLEArgpo.Text = Orbit_File.ReadLine();
            tB_TLEMo.Text = Orbit_File.ReadLine();
            tB_TLENodeo.Text = Orbit_File.ReadLine();
            tB_TLENo.Text = Orbit_File.ReadLine();

            orbit[0] = Convert.ToDouble(tB_TLEBstar.Text);   //大气系数
            orbit[1] = Convert.ToDouble(tB_TLEcco.Text);      //偏心率
            orbit[2] = Convert.ToDouble(tB_TLEInclo.Text);      //倾角
            orbit[3] = Convert.ToDouble(tB_TLEArgpo.Text);      //近地点幅角
            orbit[4] = Convert.ToDouble(tB_TLEJdsatepoch.Text);   //儒略时间
            orbit[5] = Convert.ToDouble(tB_TLEMo.Text);    //平近点角
            orbit[6] = Convert.ToDouble(tB_TLENo.Text);    //平均运动
            orbit[7] = Convert.ToDouble(tB_TLENodeo.Text); //升交点赤经

            Orbit_File.Close();


        }

        private void cB_orbit_para_UnChecked(object sender, RoutedEventArgs e)
        {
            orbit_checked = false;
        }

        private void tB_TLEJdsatepoch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[4] = Convert.ToDouble(tB_TLEJdsatepoch.Text);   //儒略时间

        }

        private void tB_TLEBstar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[0] = Convert.ToDouble(tB_TLEBstar.Text);   //大气系数
        }

        private void tB_TLEcco_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[1] = Convert.ToDouble(tB_TLEcco.Text);      //偏心率
        }

        private void tB_TLEInclo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[2] = Convert.ToDouble(tB_TLEInclo.Text);      //倾角
        }

        private void tB_TLEArgpo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[3] = Convert.ToDouble(tB_TLEArgpo.Text);      //近地点幅角
        }

        private void tB_TLEMo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[5] = Convert.ToDouble(tB_TLEMo.Text);    //平近点角
        }

        private void tB_TLENodeo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[7] = Convert.ToDouble(tB_TLENodeo.Text); //升交点赤经
        }

        private void tB_TLENo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                orbit[6] = Convert.ToDouble(tB_TLENo.Text);    //平均运动
        }
        #endregion
    }
}
