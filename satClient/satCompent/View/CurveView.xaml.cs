using System.Windows.Controls;

namespace satCompent.View
{
    /// <summary>
    /// CurveView.xaml 的交互逻辑
    /// </summary>
    public partial class CurveView : Page
    {
        //public CurveView()
        //{
        //    InitializeComponent();

        //    this.DataContext = new TSFCS.DMDS.Client.ViewModel.CurveViewModel();
        //}

        public CurveView(int id, int type)
        {
            InitializeComponent();

            this.DataContext = new satCompent.ViewModel.CurveViewModel(id, type);
        }
    }
}
