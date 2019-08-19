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

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
        }

        private void UserNameTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UserNameTextBox.Focus();
            UserNameTextBox.SelectAll();
        }

        private void PasswordTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordTextBox.Focus();
            PasswordTextBox.SelectAll();
        }

        private void EmailTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            EmailTextBox.Focus();
            EmailTextBox.SelectAll();
        }
    }
}
