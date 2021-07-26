using System.Windows.Controls;

namespace satCompent.View
{
    /// <summary>
    /// CurrView.xaml 的交互逻辑
    /// </summary>
    public partial class CurrView : Page
    {
        public CurrView()
        {
            InitializeComponent();
            this.DataContext = new satCompent.ViewModel.CurrViewModel();
        }
    }
}
