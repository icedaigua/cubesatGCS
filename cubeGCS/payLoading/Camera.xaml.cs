using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using CubeCOM;
using System.Text;

namespace payLoading
{
    /// <summary>
    /// Camera.xaml 的交互逻辑
    /// </summary>
    public partial class Camera : UserControl
    {

        private byte[] img = new byte[1024 * 1024 * 2]; //2M
        byte frameCnt = 0,length = 0;

        public Camera()
        {
            InitializeComponent();
            cB_camera_initz();
        }


        public void cB_camera_initz()
        {
            string[] camera_list = new string[23] { "本机复位","恢复默认参数表","LEOP开", "LEOP关",
                                                    "CCSDS连续发射开", "CCSDS连续发射关","相机开机", "相机关机",
                                                    "建立JPG任务", "建立Download任务","设置相机下行延迟", "读FRAM",
                                                    "设置TX增益", "转发开","转发关", "下传本机工程参数",
                                                    "建立CAM任务", "重新发送指定相机数据包","AVR复位", "设置分辨率640*480",
                                                    "设置分辨率800*600", "设置压缩率","设置信标发送周期"};


            cB_camera.ItemsSource = camera_list;
            cB_camera.SelectedIndex = cB_camera.Items.Count > 0 ? 0 : -1;
        }


        public void send_camera_cmd(byte[] cmd)
        {
            UInt32 delay_time = Convert.ToUInt32(tB_delay_time.Text);
            switch (cB_camera.Text)
            {
                case "建立Download任务":
                    cubeCOMM.generate_up_ctrl_cmd_cs(cmd, 1,
                                       cubeCOMM.INS_APP_STR_DOWN,
                                       delay_time
                                       );
                    break;
                default:
                    break;
            }

            tB_delay_time.Text = 0.ToString();

        }

        public void CameraProcess(byte[] camerabuffer)
        {
            length = camerabuffer[2];
            frameCnt = camerabuffer[3];

            tB_frameCnt.Text = frameCnt.ToString();
            for (byte kc = 0;kc<100;kc++)
            {
                img[frameCnt*100 + kc] = camerabuffer[4+kc];
            }
        }


        public void image_proc()
        {
            string PATH = Directory.GetCurrentDirectory();

            byte[] img_dst = new byte[((frameCnt - 1) * 100 + length)];

            //img.CopyTo(img_dst,0);
            Array.Copy(img, img_dst, img_dst.Length);

            StreamWriter camFrame = new StreamWriter(PATH + "\\camera\\" + "\\Cam.jpg");

            string camStr = Encoding.Default.GetString(img_dst);

            camFrame.WriteLine(camStr);

            camFrame.Close();

            try
            {
                BitmapImage myBitmapImage = GetBitmapImage(img_dst);
                Img_camera.Source = myBitmapImage;

                SavePhoto(PATH + "\\camera\\", myBitmapImage);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show("图像处理错误："+ e.Message);
                return;
            }





            //BitmapImage myBitmapImage = new BitmapImage(new Uri(PATH + "\\resource\\lena.png"));
            //Img_camera.Source = myBitmapImage;

            //byte[] img_byte = ImageToByte(myBitmapImage);

            //BitmapImage myBitmapImage_tmp = GetBitmapImage(img_byte);

            //SavePhoto(PATH + "\\camera\\", myBitmapImage_tmp);

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
    }
}
