using System;
using System.IO;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using CubeCOM;
using Dongzr.MidiLite;

using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace payLoading
{
    /// <summary>
    /// Camera.xaml 的交互逻辑
    /// </summary>
    public partial class Camera : UserControl
    {

        private const UInt16 CAMERA_LENGTH_UP = 200;
        private const byte CAMERA_LENGTH_DOWN = 240;

        string PATH = Directory.GetCurrentDirectory();
        private DirectoryInfo CameraPATH;
        private byte[] img = new byte[1024 * 1024 * 2]; //2M
        UInt16 frameCnt = 0;
        byte length = 0;

        public Camera()
        {
            InitializeComponent();
            cB_camera_initz();

            local_timer_start();
            //CameraListInitz();
            //dG_camera_time.ItemsSource = LoadCollectionData();
        }


        public void cB_camera_initz()
        {
            //string[] camera_list = new string[23] { "本机复位","恢复默认参数表","LEOP开", "LEOP关",
            //                                        "CCSDS连续发射开", "CCSDS连续发射关","相机开机", "相机关机",
            //                                        "建立JPG任务", "建立Download任务","设置相机下行延迟", "读FRAM",
            //                                        "设置TX增益", "转发开","转发关", "下传本机工程参数",
            //                                        "建立CAM任务", "重新发送指定相机数据包","AVR复位", "设置分辨率640*480",
            //                                        "设置分辨率800*600", "设置压缩率","设置信标发送周期"};

            string[] camera_list = new string[6] {"相机复位","相机开机", "相机关机",
                                                    "成像指令", "下行图像", "上传图片"};

            cB_camera.ItemsSource = camera_list;
            cB_camera.SelectedIndex = cB_camera.Items.Count > 0 ? 4 : -1;
        }

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
                    cubeCOMM.generate_up_para_cmd_cs(cmd, 1,
                                       cubeCOMM.INS_APP_STR_DOWN,
                                       delay_time,
                                       0,Convert.ToUInt32(tB_camera_params.Text)
                                       );

                    time_now = DateTime.Now.ToString("yyyy-MM-dd") +
                                    '(' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ')';

                    CameraPATH = new DirectoryInfo(PATH + "\\camera\\" + time_now);
                    CameraPATH.Create();
                    break;

                case "上传图片":

                    imagebuf = getNewImage();
                    if (imagebuf == null) break;
                    if (CameraPort.IsOpen)
                        CameraPort.Close();
                    serial_create("COM5");
                    serial_send(startCmd, 4);
                    imageUp = 1;

                    break;
                default:
                    break;
            }

            tB_delay_time.Text = 0.ToString();

        }

        #endregion

        #region 图像数据绑定
        List<CameraLIst> camList = new List<CameraLIst>();

        public class CameraLIst
        {
            public string ID { get; set; }
            public string Name { get; set; }
           // public string time { get; set; }
        }

        private void CameraListInitz()
        {
            ObservableCollection<CameraLIst> memberData = new ObservableCollection<CameraLIst>();
            memberData.Add(new CameraLIst()
            {
                ID = 101.ToString(),
                Name = 12324244.ToString(),
            });

            dG_camera_time.DataContext = memberData;
        }

        private List<CameraLIst> LoadCollectionData()
        {
           
            camList.Add(new CameraLIst()
            {
                ID = 101.ToString(),
                Name = 12324244.ToString(),
            });
    
            return camList;
        }


        private void dG_camera_time_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            camList.Add(new CameraLIst()
            {
                ID = 112.ToString(),
                Name = 12324244.ToString(),
            });
        }

        #endregion

        #region 定时器

        private int sendLength = 0;
        private byte[] sendbuf = new byte[CAMERA_LENGTH_UP];
        private MmTimer local_time_timer = new MmTimer();



        private void local_timer_start()
        {
            local_time_timer.Interval = 500;
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
     
            CameraPort.Open();
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
                System.Windows.MessageBox.Show("串口发送错误:" + e.Message);
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

            Array.Copy(camerabuffer, 5, img, frameCnt * CAMERA_LENGTH_DOWN, CAMERA_LENGTH_DOWN);

            if ((frameCnt % 50) == 0)
                image_proc();

        }

        public void image_trans()
        {
            string PATH = Directory.GetCurrentDirectory();
            try
            {
                StreamReader camFrame = new StreamReader(PATH + "\\camera\\" + "\\2085979490.txt");
                string ss = camFrame.ReadToEnd();
                byte[] img_dst = Encoding.Default.GetBytes(ss);
                BitmapImage myBitmapImage = GetBitmapImage(img_dst);
                Img_camera.Source = myBitmapImage;

                SavePhoto(PATH + "\\camera\\", myBitmapImage);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("图像处理错误：" + e.Message);
                return;
            }
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
                System.Windows.MessageBox.Show("图像处理错误："+ e.Message);
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
                System.Windows.MessageBox.Show("Get Image Error:" + e.Message);
                return null;
            }



            //BitmapImage myBitmapImage_tmp = GetBitmapImage(img_byte);

            //SavePhoto(PATH + "\\camera\\", myBitmapImage_tmp);
        }
        #endregion
    }
}
