using satCompent.ViewModel;
using System.Windows.Controls;

namespace satCompent.View
{
    /// <summary>
    /// OrbitView.xaml 的交互逻辑
    /// </summary>
    public partial class OrbitView : UserControl
    {
        public OrbitView()
        {
            InitializeComponent();

            this.DataContext = new OrbitViewModel();
        }
    }
}
