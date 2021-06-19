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

using GalaSoft.MvvmLight.Messaging;

namespace TSFCS.DMDS.Client.View
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, "login", HandleLogin);

            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void HandleLogin(string info)
        {
            if ("loaded".Equals(info))
            {
            }
            else if ("dragmove".Equals(info))
            {
                try
                {
                    this.DragMove();
                }
                catch
                {
                }
            }
            else if ("min".Equals(info))
            {
                if (this.WindowState == System.Windows.WindowState.Normal)
                    this.WindowState = System.Windows.WindowState.Minimized;
                else
                    this.WindowState = System.Windows.WindowState.Normal;
            }
            else if ("closed".Equals(info))
            {
                this.Close();
            }
            else if ("login".Equals(info))
            {
                MainView main = new MainView();
                main.Show();
                this.Close();
            }
            else
            {
            }
        }

    }
}
