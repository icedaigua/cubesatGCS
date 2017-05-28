using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;

namespace HouseKeeping_Wpf
{
    /// <summary>
    /// hk_up.xaml 的交互逻辑
    /// </summary>
    public partial class hk_up : UserControl
    {

        #region 控制指令

         public bool  mwa_open_checked  { get { return cB_mwa_open.IsChecked == true; } } //动量轮A
         public bool  mwa_close_checked { get { return cB_mwa_close.IsChecked == true; } }
         public bool  mwb_open_checked { get { return cB_mwb_close.IsChecked == true; } } //动量轮B
         public bool  mwb_close_checked { get { return cB_mwb_close.IsChecked == true; } }

         public bool  hmra_open_checked  { get { return cB_hmra_open.IsChecked == true; } }      //磁强计
         public bool  hmra_close_checked { get { return cB_hmra_open.IsChecked == true; } }
         public bool  hmrb_open_checked  { get { return cB_hmrb_open.IsChecked == true; } }
         public bool  hmrb_close_checked { get { return cB_hmrb_open.IsChecked == true; } }
  
         public bool  fan_open_start_checked { get { return cB_fan_open_start.IsChecked == true; } } //帆板
         public bool  fan_open_stop_checked { get { return cB_fan_open_stop.IsChecked == true; }}



         public bool   batt_warm_open_checked  { get { return cB_batt_warm_open.IsChecked == true; } }//电池阵加热
         public bool   batt_warm_close_checked { get { return cB_batt_warm_close.IsChecked == true; } }
        
         public bool  atenna_open_checked    { get { return cB_atenna_open.IsChecked == true; } }        //天线展开
         public bool  atenna_pwr_on_checked  { get { return cB_atenna_pwr_open.IsChecked == true; } } //天线电源
         public bool  atenna_pwr_off_checked { get { return cB_atenna_pwr_close.IsChecked == true; } }
        
         public bool hk_reset_checked { get { return cB_hk_reset.IsChecked == true; } }        //星务重启

         public bool  down_start_checked  { get { return cB_start_down.IsChecked == true; } }     //开始下行
         public bool down_stop_checked   { get { return cB_stop_down.IsChecked == true; } }       //停止下行

         public bool  dam_mode_checked     { get { return cB_dam_mode.IsChecked == true; } }      //控制模式
         public bool  ctrl_mode_checked    { get { return cB_ctrl_mode.IsChecked == true; } } 
         public bool  pitch_mode_checked   { get { return cB_pitch_mode.IsChecked == true; } } 
         public bool  redam_checked        { get { return cB_reDam.IsChecked == true; } } 
         public bool always_dam_checked     { get { return cB_always_dam.IsChecked == true; } }



         public bool  adcs_open_checked     { get { return cB_adcs_open.IsChecked == true; } } 
         public bool  adcs_close_checked     { get { return cB_adcs_close.IsChecked == true; } }
  
         public bool error_checked { get { return cB_error.IsChecked == true; } }

        //public bool   pianzhi_mode_checked = false,
        //public bool   zero_mode_checked = false,

        //public bool  close_all_checked = false,

        //public bool  bpsk_1200_checked = false,
        //public bool  bpsk_9600_checked = false,
        //public bool  down_fipex_checked = false,
        #endregion

        #region 参数注入

       
          public bool    para_time_checked    { get { return cB_time_para .IsChecked == true; } }
          public bool    para_P_checked       { get { return cB_P_para.IsChecked == true; } }
          public bool    para_Z_checked       { get { return cB_Z_para.IsChecked == true; } }
          public bool    para_D_checked       { get { return cB_D_para.IsChecked == true; } }
          public bool    para_down_period_checked    { get { return cB_down_period_para.IsChecked == true; } }
          public bool    delay_hk_checked            { get { return cB_delay_hk.IsChecked == true; } }
          //public bool    camera_checked              { get { return cB_always_dam.IsChecked == true; } }
        

         public UInt32 para_P { get { return Convert.ToUInt32(tB_P_para.Text); } }
         public UInt32 para_D { get { return Convert.ToUInt32(tB_D_para.Text); } }
         public UInt32 para_Z { get { return Convert.ToUInt32(tB_Z_para.Text); } }
         public UInt32 para_down_period     { get { return Convert.ToUInt32(tB_down_period.Text); } }
         public UInt32 delay_hk_orbit_cnt   { get { return Convert.ToUInt32(tB_delay_hk.Text); } }
         public UInt32 delay_hk_select      { get { return Convert.ToUInt32(cB_delay_select.SelectedIndex); } }
         public UInt32 delay_hk_index       { get { return Convert.ToUInt32(tB_delay_index.Text); } }

         public UInt32   bias_mode             { get { return Convert.ToUInt32(tB_bias_mode.Text); } }
                     //camera_delay_time = 0
          public UInt32 para_time { get { return Convert.ToUInt32(tB_time_para.Text);}}


#endregion

        #region 轨道注入
        //public bool orbit_modi = false;
        public double[] orbit = new double[8];


        public bool orbit_checked = false;

        #endregion


        #region 特殊指令

        public bool
                 rsh_checked = false;

        public string rsh_str = "", fipex_str = "";

        #endregion
        public hk_up()
        {
            InitializeComponent();

            cB_delay_hk_select_init();
        }


        private void cB_delay_hk_select_init()
        {

            List<String> NetNo_list = new List<String> { "SD", "RAM","FLASH" };
            cB_delay_select.ItemsSource = NetNo_list;
            cB_delay_select.SelectedIndex = cB_delay_select.Items.Count > 0 ? 1 : -1;
        }

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


        #region 姿控计算机指令
        /// <summary>
        /// 动量轮d开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_open_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_adcs_close.IsChecked == true)
                cB_adcs_close.IsChecked = false;

        }

        /// <summary>
        /// 动量轮B关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_adcs_close_Checked(object sender, RoutedEventArgs e)
        {
           

            if (cB_adcs_open.IsChecked == true)
                cB_adcs_open.IsChecked = false;

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


        #region 电池加热
        /// <summary>
        /// 电池加热开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_open_Checked(object sender, RoutedEventArgs e)
        {       
            if (cB_batt_warm_close.IsChecked == true)
                cB_batt_warm_close.IsChecked = false;

        }

        /// <summary>
        /// 电池加热关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_batt_warm_close_Checked(object sender, RoutedEventArgs e)
        {
          
            if (cB_batt_warm_open.IsChecked == true)
                cB_batt_warm_open.IsChecked = false;

        }

        #endregion

        #region 天线电源
        /// <summary>
        /// 天线电源开选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_open_Checked(object sender, RoutedEventArgs e)
        {
         
            if (cB_atenna_pwr_close.IsChecked == true)
                cB_atenna_pwr_close.IsChecked = false;

        }


        /// <summary>
        /// 天线电源关选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_atenna_pwr_close_Checked(object sender, RoutedEventArgs e)
        {

            if (cB_atenna_pwr_open.IsChecked == true)
                cB_atenna_pwr_open.IsChecked = false;

        }

        #endregion

        #region 帆板展开
        /// <summary>
        /// 帆板展开启动选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_start_Checked(object sender, RoutedEventArgs e)
        {
            
            if (cB_fan_open_stop.IsChecked == true)
                cB_fan_open_stop.IsChecked = false;
        }

        /// <summary>
        /// 帆板展开停止选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_fan_open_stop_Checked(object sender, RoutedEventArgs e)
        {
        
            if (cB_fan_open_start.IsChecked == true)
                cB_fan_open_start.IsChecked = false;
        }

        #endregion


        #region 下行控制
        /// <summary>
        /// 开始下行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_start_down_Checked(object sender, RoutedEventArgs e)
        {
          
            if (cB_stop_down.IsChecked == true)
                cB_stop_down.IsChecked = false;
        }

        /// <summary>
        /// 停止下行选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cB_stop_down_Checked(object sender, RoutedEventArgs e)
        {
 
            if (cB_start_down.IsChecked == true)
                cB_start_down.IsChecked = false;
        }

        #endregion

       
        #region 上行参数

        /// <summary>
        /// 星上时间输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_time_para_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                //MessageBox.Show(tB_time_para.Text);
                tB_time_para.Text = xDateSeconds();//DateTime.Now.ToLongTimeString();
                                                   //MessageBox.Show(tB_time_para.Text);
            }
        }

       
       
        /// <summary>
        /// 延时遥测输入框回车键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tB_delay_hk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//如果输入的是回车键 
            {
                MessageBox.Show(tB_delay_hk.Text);

         

                MessageBox.Show(delay_hk_orbit_cnt.ToString());


            }
        }

      
        #endregion

      

        #region  轨道参数
        private void cB_orbit_para_Checked(object sender, RoutedEventArgs e)
        {
            orbit_checked = true;


            string PATH = Directory.GetCurrentDirectory();
            StreamReader Orbit_File;                       ///实时数据存储文件  
            Orbit_File = new StreamReader(PATH + "\\orbit.txt");

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


        private string xDateSeconds()
        {
            long xdateseconds = 0;
            DateTime xdatenow = DateTime.UtcNow;     //当前UTC时间

            long xminute = 60;      //一分种60秒
            long xhour = 3600;
            long xday = 86400;
            long byear = 1970;//从1970-1-1 0：00：00开始到现在所过的秒
            long[] xmonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            long[] xyear = { 365, 366 };
            long num = 0;
            xdateseconds += xdatenow.Second;    //算秒
            xdateseconds += xdatenow.Minute * xminute;      //算分
            xdateseconds += xdatenow.Hour * xhour;      //算时
            xdateseconds += (xdatenow.Day - 1) * xday;        //算天
            //算月(月换成天算)
            if (DateTime.IsLeapYear(xdatenow.Year))
            {
                xdateseconds += (xmonth[xdatenow.Month - 1] + 1) * xday;
            }
            else
            {
                xdateseconds += (xmonth[xdatenow.Month - 1]) * xday;
            }
            //算年（年换成天算）
            long lyear = xdatenow.Year - byear;
            for (int i = 0; i < lyear; i++)
            {
                if (DateTime.IsLeapYear((int)byear + i))
                {
                    num++;
                }
            }
            xdateseconds += ((lyear - num) * xyear[0] + num * xyear[1]) * xday;
            return xdateseconds.ToString();
        }
    }
}

