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
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.ViewModels;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        LoginViewModel loginViewModel;

        public LoginView()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
        }

        private void NotifyInvalidLoginCredentials()
        {
            NotifyLabel.Text = "Invalid login credentials";
            NotifyLabel.Visibility = Visibility.Visible;
        }

        private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int roleId = loginViewModel.GetLoginRole(UsernameTextBox.Text, Crypto.ConvertToHash(PasswordTextBox.Password));

                if (roleId > 0)
                {
                    foreach (Window window in Application.Current.Windows)
                        if (window.GetType() == typeof(MainWindow))
                            (window as MainWindow).MainContentControl.DataContext = new AdminMainView(roleId);
                }
                else
                {
                    NotifyInvalidLoginCredentials();
                }
            }
            catch
            {
                NotifyInvalidLoginCredentials();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }
    }
}
