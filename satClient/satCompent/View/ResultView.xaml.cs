using System.Windows.Controls;

namespace satCompent.View
{
    /// <summary>
    /// ResultView.xaml 的交互逻辑
    /// </summary>
    public partial class ResultView : Page
    {
        public ResultView()
        {
            InitializeComponent();

            this.DataContext = new satCompent.ViewModel.ResultViewModel();
        }
    }
}
