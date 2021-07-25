using System.Windows.Controls;
using satCompent.ViewModel;

namespace satCompent.View
{
    /// <summary>
    /// DataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridView : UserControl
    {
        public DataGridView()
        {
            InitializeComponent();
            this.DataContext = new DataGridViewModel();
        }
    }
}
