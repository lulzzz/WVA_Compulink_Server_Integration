using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WVA_Connect_CSI.Memory;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            SetDebuggerStatus();
        }

        private void SetDebuggerStatus()
        {
            if (Storage.DebugOn)
                DebuggerStatusCheckBox.IsChecked = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }

        private void DebuggerStatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Storage.DebugOn = true;
        }

        private void DebuggerStatusCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Storage.DebugOn = false;
        }
    }
}
