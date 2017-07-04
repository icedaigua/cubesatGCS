using CubeCOM;
using Dongzr.MidiLite;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace payLoading
{
    /// <summary>
    /// Camera.xaml 的交互逻辑
    /// </summary>
    public partial class Camera : UserControl
    {

        private const UInt16 CAMERA_LENGTH_UP = 800;
        private const byte CAMERA_LENGTH_DOWN = 240;

        string PATH ="";
        private DirectoryInfo CameraPATH;


        private byte[] img = new byte[1024 * 1024 * 2]; //2M
        UInt16 frameCnt = 0;
        byte length = 0;


        #region 初始化


        public Camera()
        {
            InitializeComponent();
            cB_camera_initz();

        }

        public void CameraModulesInitz()
        {
            CameraListInitz();
            local_timer_start();
        }


        private void cB_camera_initz()
        {
  
            string[] camera_list = new string[5] {"相机复位","相机开机", "相机关机",
                                                    "成像指令", "下行图像"};

            cB_camera.ItemsSource = camera_list;
            cB_camera.SelectedIndex = cB_camera.Items.Count > 0 ? 4 : -1;


            string[] hk_list = new string[2] { "SD", "SRAM" };
            cB_delay_select.ItemsSource = hk_list;
            cB_delay_select.SelectedIndex = cB_delay_select.Items.Count > 0 ? 1 : -1;
        }

        #endregion


        #region 上行图像指令

        private byte[] imagebuf;
        private byte[] startCmd = { 0x30, 0xAA, 0xBB, 0xCC }, endCmd = { 0x30, 0xDD, 0xEE, 0xFF };
        private string time_now ="";

        private byte imageUp = 0;

        public void send_camera_cmd(byte[] cmd)
        {
            UInt32 delay_time = Convert.ToUInt32(tB_delay_time.Text);
            switch (cB_camera.Text)
            {
                case "下行图像":

                    time_now = DateTime.Now.ToString("yyyy-MM-dd") +
                 '(' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ')';

                    CameraPATH = new DirectoryInfo(PATH + "\\camera\\" + time_now);
                    CameraPATH.Create();

                    UInt32 para1 = (UInt32)cB_delay_select.SelectedIndex;

                    UInt32 para2 = Convert.ToUInt32(tB_camera_params.Text);


                    cubeCOMM.generate_up_para_cmd_cs(cmd, 1,
                                       cubeCOMM.INS_APP_STR_DOWN,
                                       delay_time,
                                       para1, para2
                                       );

                    break;

          
                default:
                    break;
            }

            tB_delay_time.Text = 0.ToString();

        }


        public void upImage()
        {     

            if (imageUp > 0)
            {
                System.Windows.MessageBox.Show("已有上行中的任务,等待结束");
                return;
            }
            imagebuf = getNewImage();
            if (imagebuf == null)
            {
                System.Windows.MessageBox.Show("未能获取图像");
                return;
            }
            UInt32 imageID = (UInt32)(xDateSeconds() + Convert.ToUInt32(tB_delay_time.Text));

            addCameraItem(imageID);

            if (CameraPort.IsOpen)
                CameraPort.Close();
            serial_create("COM5");
            serial_send(startCmd, 4);
            imageUp = 1;

        }
        #endregion

        #region 图像数据绑定
        private UInt32 LatestCameraNo = 0;

        ObservableCollection<CameraLIst> memberData = new ObservableCollection<CameraLIst>();

        public class CameraLIst
        {
            public string Number { get; set; }
            public string Date { get; set; }
            public string ImageID { get; set; }
        }

        private void CameraListInitz()
        {
            PATH = Directory.GetCurrentDirectory();
            StreamReader allImage = null;
            try
            {
                allImage = new StreamReader(PATH + "\\camera\\allImage.txt");
                
                while (!allImage.EndOfStream)
                {
                    string str = allImage.ReadLine();

                    string[] xxsplit;
                    xxsplit = str.Split(new Char[] { ',', ' ', '，', '\n', '\t' });

                    if(xxsplit.Length<2)
                        continue;

                    LatestCameraNo = Convert.ToUInt32(xxsplit[0]);
                    memberData.Add(new CameraLIst()
                    {
                        Number = Convert.ToUInt32(xxsplit[0]).ToString(),
                        Date = ConvertTimer(Convert.ToUInt32(xxsplit[1])),
                        ImageID = Convert.ToUInt32(xxsplit[1]).ToString(),
                    });
                }
                allImage.Close();
            }
            catch (Exception e)
            {
                if(allImage != null)
                    allImage.Close();
                Trace.TraceError("图像链表初始化错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("图像链表初始化错误:" + e.Message);
            }

            dG_camera_time.DataContext = memberData;

        }

        private void addCameraItem(UInt32 utc)
        {
            StreamWriter allImage = null;
            try
            {
                FileStream allImageF = new FileStream(PATH + "\\camera\\allImage.txt", FileMode.Append, FileAccess.Write);
                allImage = new StreamWriter(allImageF);

                LatestCameraNo += 1;

                memberData.Add(new CameraLIst()
                {
                    Number = LatestCameraNo.ToString(),
                    Date = ConvertTimer(utc),
                    ImageID = utc.ToString(),
                });

                allImage.WriteLine(LatestCameraNo.ToString() + '\t' + utc.ToString());

                allImage.Close();
            }
            catch (Exception e)
            {
                if (allImage != null)
                    allImage.Close();
                Trace.TraceError("向链表中增加新图像错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("向链表中增加新图像错误:" + e.Message);
            }

            dG_camera_time.DataContext = memberData;
        }


        #endregion

        #region 定时器

        private int sendLength = 0;
        private byte[] sendbuf = new byte[CAMERA_LENGTH_UP];
        private MmTimer local_time_timer = new MmTimer();



        private void local_timer_start()
        {
            local_time_timer.Interval = 100;
            local_time_timer.Mode = MmTimerMode.Periodic;
            local_time_timer.Tick += new EventHandler(local_timer_handler);
            local_time_timer.Start();
        }

        private void local_timer_handler(object sender, EventArgs e)
        {
            switch (imageUp)
            {
                case 0:
                    break;
                case 1:
                    try
                    {
                        if (imagebuf.Length - sendLength <= CAMERA_LENGTH_UP)
                        {
                            Array.Copy(imagebuf, sendLength, sendbuf, 0, imagebuf.Length - sendLength);
                            serial_send(sendbuf, imagebuf.Length - sendLength);

                            sendLength = 0;

                            imageUp = 2;

                        }
                        else
                        {
                            Array.Copy(imagebuf, sendLength, sendbuf, 0, CAMERA_LENGTH_UP);
                            sendLength = sendLength + CAMERA_LENGTH_UP;
                            serial_send(sendbuf, CAMERA_LENGTH_UP);
                        }
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError("上行图像过程错误:" + ex.Message + ex.StackTrace);
                    }
   
                    break;

                case 2:
                    serial_send(endCmd, 4);
                    imageUp = 0;
                    break;
                default:
                    break;
            }

            
        }

        #endregion


        #region 串口

        private SerialPort CameraPort = new SerialPort();      ///接收数据串口
    

        /// <summary>
        /// 创建串口
        /// </summary>
        private void serial_create(string portID)
        {
            if (CameraPort.IsOpen)
            {
                MessageBox.Show("串口已打开！");
                return;
            }

            CameraPort.PortName = portID;
            CameraPort.BaudRate = 1500000;

            CameraPort.Parity = Parity.None;
            CameraPort.StopBits = StopBits.One;
            CameraPort.DataBits = 8;
            //In_Port.ReadTimeout = 200;
            //In_Port.WriteTimeout = 50;

            CameraPort.ReadBufferSize = 4096;

            try
            {
                CameraPort.Open();
            }
            catch(Exception e)
            {
                Trace.TraceError("图像串口打开错误:" + e.Message + e.StackTrace);
            }

        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void SerialPort_Close()
        {
            if (CameraPort.IsOpen) CameraPort.Close();
        }


        private void serial_send(byte[] sendbuf,int length)
        {
            try
            {
                CameraPort.Write(sendbuf, 0, length);
            }
            catch(Exception e)
            {
                Trace.TraceError("图像串口数据发送错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("串口发送错误:" + e.Message);
                CameraPort.Close();
            }
            
        }

        #endregion


        #region 图像处理

        public void CameraProcess(byte[] camerabuffer)
        {
            length = camerabuffer[2];
            frameCnt = (UInt16)((UInt16)(camerabuffer[4]<<8) + camerabuffer[3]);

            tB_frameCnt.Text = frameCnt.ToString();

            try
            {
                Array.Copy(camerabuffer, 5, img, frameCnt * CAMERA_LENGTH_DOWN, length);
            }
            catch(Exception e)
            {
                Trace.TraceError("下行图像处理错误:" + e.Message + e.StackTrace);
                return;
            }

            if ((frameCnt % 50) == 0)
                image_proc();

        }

        public void image_trans()
        {
            //string PATH = Directory.GetCurrentDirectory();
            //try
            //{
            //    StreamReader camFrame = new StreamReader(PATH + "\\camera\\" + "\\2085979490.txt");
            //    string ss = camFrame.ReadToEnd();
            //    byte[] img_dst = Encoding.Default.GetBytes(ss);
            //    BitmapImage myBitmapImage = GetBitmapImage(img_dst);
            //    Img_camera.Source = myBitmapImage;

            //    SavePhoto(PATH + "\\camera\\", myBitmapImage);
            //}
            //catch (Exception e)
            //{
            //    System.Windows.MessageBox.Show("图像处理错误：" + e.Message);
            //    return;
            //}
        }

        public void image_proc()
        {
            if (frameCnt < 10) return;

            byte[] img_dst = new byte[((frameCnt - 1) * CAMERA_LENGTH_DOWN + length)];

            Array.Copy(img, img_dst, img_dst.Length);


            try
            {
                BitmapImage myBitmapImage = GetBitmapImage(img_dst);
                Img_camera.Source = myBitmapImage;

                SavePhoto(CameraPATH+"\\", myBitmapImage);
            }
            catch(Exception e)
            {
                StreamWriter camFrame = new StreamWriter(CameraPATH + "\\Cam.jpg");

                string camStr = Encoding.Default.GetString(img_dst);

                camFrame.WriteLine(camStr);

                camFrame.Close();
                Trace.TraceError("图像显示处理错误:" + e.Message + e.StackTrace);
                //System.Windows.MessageBox.Show("图像处理错误："+ e.Message);
                return;
            }



        }

        private  BitmapImage GetBitmapImage(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private Byte[] ImageToByte(BitmapImage imageSource)
        {

            MemoryStream ms = new MemoryStream();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(ms);

            byte[] image_bytes =  ms.ToArray();

            ms.Close();
            return image_bytes;
        }



        private Guid SavePhoto(string istrImagePath, BitmapImage imageSource)
        {
       
            Guid photoID = System.Guid.NewGuid();
            string photolocation = istrImagePath + photoID.ToString() + ".jpg";  //file name
            FileStream filestream = new FileStream(photolocation, FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(filestream);

            return photoID;

            //反向
            //Image back_image = Image.FromStream(new MemoryStream(image_bytes));
            //保存文件
            //back_image.Save("文件名.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private byte[] getNewImage()
        {
            string PATH = Directory.GetCurrentDirectory();
            try
            {
                BitmapImage myBitmapImage = new BitmapImage(new Uri(PATH + "\\resource\\" + tB_image_id.Text));
                Img_camera.Source = myBitmapImage;
                return  ImageToByte(myBitmapImage);
            }
            catch(Exception e)
            {
                Trace.TraceError("Get New Image Error:" + e.Message + e.StackTrace);
                System.Windows.MessageBox.Show("Get Image Error:" + e.Message);
                return null;
            }



            //BitmapImage myBitmapImage_tmp = GetBitmapImage(img_byte);

            //SavePhoto(PATH + "\\camera\\", myBitmapImage_tmp);
        }

        private void Img_camera_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Img_camera.Source = null;
        }
        #endregion


        #region 通用函数

        private string ConvertTimer(UInt32 utc)
        {
            DateTime dt = new DateTime(                 //显示为本地时间
                            1970, 1, 1, 0, 0, 0, DateTimeKind.Local).
                            AddSeconds(Convert.ToDouble(utc));

            return (dt.ToString("yyyy年MM月dd日hh:mm:ss"));
        }



        private long xDateSeconds()
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
            return xdateseconds;
        }
        #endregion
    }
}
