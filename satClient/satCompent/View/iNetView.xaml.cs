using satCompent.ViewModel;
using System.Windows.Controls;


namespace satCompent.View
{
    /// <summary>
    /// iNetView.xaml 的交互逻辑
    /// </summary>
    public partial class iNetView : UserControl
    {
        public iNetView()
        {
            InitializeComponent();
            //RefreshSize(width, height);
            this.DataContext = new iNetViewModel();
        }

        private void RefreshSize(uint width,uint height)
        {
            tB_sendbuf.Width = width;
            tB_sendbuf.Height = height;

            tB_recbuf.Width = width;
            tB_recbuf.Height = height;        
        }


    }
}
