﻿using System.Windows.Controls;
using satCompent.ViewModel;

namespace satCompent.View
{
    /// <summary>
    /// DataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class ADCSDataGridView : UserControl
    {
        public ADCSDataGridView()
        {
            InitializeComponent();
            this.DataContext = new ADCSDataGridViewModel();
        }
    }
}
