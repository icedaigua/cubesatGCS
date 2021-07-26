using System.Windows.Controls;
using satCompent.ViewModel;

namespace satCompent.View
{
    /// <summary>
    /// DataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class OBCDataGridView : UserControl
    {
        public OBCDataGridView()
        {
            InitializeComponent();
            this.DataContext = new OBCDataGridViewModel();
        }
    }
}
