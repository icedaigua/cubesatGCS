using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
