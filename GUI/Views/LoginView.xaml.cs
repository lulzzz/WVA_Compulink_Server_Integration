using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.Utility.ActionLogging;
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
            PlaceCursorInLoginTextBox();
        }

        private void PlaceCursorInLoginTextBox()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    UsernameTextBox.Focus();        
                    Keyboard.Focus(UsernameTextBox);
            }));
        }

        private void Login()
        {
            try
            {
                int roleId = loginViewModel.GetLoginRole(UsernameTextBox.Text, Crypto.ConvertToHash(PasswordTextBox.Password));

                if (roleId > 0)
                {
                    // Report all action data 
                    ActionLogger.ReportAllDataNow();

                    // Add login action to action logger
                    ActionLogger.Log(GetType().FullName + nameof(Login), UsernameTextBox.Text, roleId, $"<User_Login>");

                    // Open admin main view
                    foreach (Window window in Application.Current.Windows)
                        if (window.GetType() == typeof(MainWindow))
                            (window as MainWindow).MainContentControl.DataContext = new AdminMainView(roleId, UsernameTextBox.Text);
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

        private void NotifyInvalidLoginCredentials()
        {
            NotifyLabel.Text = "Invalid login credentials";
            NotifyLabel.Visibility = Visibility.Visible;
        }

        private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotifyLabel.Visibility = Visibility.Hidden;
        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Enter)
                Login();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NotifyLabel.Visibility = Visibility.Hidden;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }
    }
}
