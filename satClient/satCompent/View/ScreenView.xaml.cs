using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TSFCS.DMDS.Client.ViewModel;

namespace TSFCS.DMDS.Client.View
{
    /// <summary>
    /// ScreenView.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenView : Window
    {
        public ScreenView()
        {
            InitializeComponent();  
        }

        public ScreenView(Page page)
        {
            InitializeComponent();
            this.frame.Content = page;   
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            FullScreenControl.ExitFullscreen(this);
            this.Close();
        }
    }
}
