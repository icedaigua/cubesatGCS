using System;
using System.IO;
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


        private void btn_send_camera_cmd_click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_image_click(object sender, RoutedEventArgs e)
        {

            string PATH = Directory.GetCurrentDirectory();

            BitmapImage myBitmapImage  = new BitmapImage(new Uri(PATH +"\\resource\\lena.png"));
            Img_camera.Source = myBitmapImage;

            byte[] img_byte = ImageToByte(myBitmapImage);

            BitmapImage myBitmapImage_tmp = GetBitmapImage(img_byte);

            SavePhoto(PATH+"\\camera\\", myBitmapImage_tmp);

        }

        public  BitmapImage GetBitmapImage(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public Byte[] ImageToByte(BitmapImage imageSource)
        {

            MemoryStream ms = new MemoryStream();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(ms);

            byte[] image_bytes =  ms.ToArray();

            ms.Close();
            return image_bytes;
        }

        public Guid SavePhoto(string istrImagePath, BitmapImage imageSource)
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
