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
            this.DataContext = new iNetViewModel();
        }

    }
}
