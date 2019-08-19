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
using WVA_Connect_CSI.Roles;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for AdminMainView.xaml
    /// </summary>
    public partial class AdminMainView : UserControl
    {
        private Role UserRole;

        public AdminMainView(string name, int value)
        {
            InitializeComponent();
            ResizeView();
            SetRole(name, value);
            SetUpView();
        }

        private void ResizeView()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).ResizeMode = ResizeMode.CanResize;
                    (window as MainWindow).MinHeight = 490;
                    (window as MainWindow).MinWidth = 850;
                }
        }

        private void SetRole(string name, int value)
        {
            UserRole = new Role(name, value).DetermineRole();
        }

        private void SetUpView()
        {
            if (UserRole is SuperAdminRole)
            {
                // Gives user full access to all controls in this view
                AdminMainViewContentControl.Content = new OrdersView();
            }
            else if (UserRole is ITAdminRole)
            {
                // Gives an IT Admin the ability to access the 'Users' view
                OrdersButton.Visibility = Visibility.Hidden;
                OrdersButton.IsEnabled = false;

                AdminMainViewContentControl.Content = new UsersView();
            }
            else if (UserRole is ManagerRole)
            {
                // Gives a manager the ability to see the 'Orders' view that contains patient information
                UsersButton.Visibility = Visibility.Hidden;
                UsersButton.IsEnabled = false;

                AdminMainViewContentControl.Content = new OrdersView();
            }
            else
            {
                // Hide header controls, limiting access to other views
                OrdersButton.Visibility = Visibility.Hidden;
                OrdersButton.IsEnabled = false;

                UsersButton.Visibility = Visibility.Hidden;
                UsersButton.IsEnabled = false;
            }

            //switch (view)
            //{
            //    case "ordersView":
            //        AdminMainViewContentControl.Content = new OrdersView();
            //        break;
            //    case "orderDetailsView":
            //        AdminMainViewContentControl.Content = new OrderDetailsView();
            //        break;
            //    case "usersView":
            //        AdminMainViewContentControl.Content = new UsersView();
            //        break;
            //    default:
                    
            //        break;
            //}
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            SetUpView();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            SetUpView();
        }
    }
}
