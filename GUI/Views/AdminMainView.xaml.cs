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

        public AdminMainView(int roleId)
        {
            InitializeComponent();
            ResizeView();
            SetRole(roleId);
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

        private void SetRole(int roleId)
        {
            UserRole = new Role(roleId).DetermineRole();
        }

        private void SetUpView()
        {
            if (UserRole is SuperAdminRole)
            {
                // Gives user full access to all controls in this view
                AdminMainViewContentControl.Content = new OrdersView(UserRole.RoleId);
            }
            else if (UserRole is ITAdminRole) // Gives an IT Admin the ability to access the 'Users' view
            {
                // Removes the 'Orders' button from the view 
                HeaderButtonStackPanel.Children.Remove(OrdersButton);

                // Set current view to users view 
                AdminMainViewContentControl.Content = new UsersView(UserRole.RoleId);
            }
            else if (UserRole is ManagerRole) // Gives a manager the ability to see the 'Orders' view that contains patient information
            {
                // Removes the 'Users' button element from the view
                HeaderButtonStackPanel.Children.Remove(UsersButton);

                // Set current view to orders view 
                AdminMainViewContentControl.Content = new OrdersView(UserRole.RoleId);
            }
            else
            {
                // Remove header elements so they cannot be accessed 
                HeaderButtonStackPanel.Children.Remove(OrdersButton);
                HeaderButtonStackPanel.Children.Remove(UsersButton);
            }
        }

        private void SetUpView(string view)
        {
            switch (view)
            {
                case "users":
                    if (UserRole.CanViewUsers)
                        AdminMainViewContentControl.Content = new UsersView(UserRole.RoleId);
                    break;
                case "orders":
                    if (UserRole.CanViewUsers)
                        AdminMainViewContentControl.Content = new OrdersView(UserRole.RoleId);
                    break;
                case "orderdetails":
                    if (UserRole.CanViewUsers)
                        AdminMainViewContentControl.Content = new OrdersView(UserRole.RoleId);
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            SetUpView("orders");
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            SetUpView("users");
        }
    }
}
