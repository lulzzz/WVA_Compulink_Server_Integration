using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Utility.ActionLogging;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for AdminMainView.xaml
    /// </summary>
    public partial class AdminMainView : UserControl
    {
        private Role UserRole;

        public AdminMainView(int roleId, string userName)
        {
            InitializeComponent();
            ResizeView();
            SetRole(roleId, userName);
            SetUpView();
        }

        public AdminMainView(int roleId, string userName, string view, Order order)
        {
            InitializeComponent();
            ResizeView();
            SetRole(roleId, userName);
            SetUpView(view, order);
        }

        //
        // Window Setup Functions
        //

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

        private void SetRole(int roleId, string userName)
        {
            UserRole = new Role(roleId, userName).DetermineRole();
        }

        private void SetUpView()
        {
            if (UserRole is SuperAdminRole)
            {
                // Gives user full access to all controls in this view
                AdminMainViewContentControl.Content = new OrdersView(UserRole);
            }
            else if (UserRole is ITAdminRole) // Gives an IT Admin the ability to access the 'Users' view
            {
                // Removes the 'Orders' button from the view 
                HeaderButtonStackPanel.Children.Remove(OrdersButton);

                // Set current view to users view 
                AdminMainViewContentControl.Content = new UsersView(UserRole);
            }
            else if (UserRole is ManagerRole) // Gives a manager the ability to see the 'Orders' view that contains patient information
            {
                // Removes the 'Users' button element from the view
                HeaderButtonStackPanel.Children.Remove(UsersButton);

                // Set current view to orders view 
                AdminMainViewContentControl.Content = new OrdersView(UserRole);
            }
            else
            {
                // Remove header elements so they cannot be accessed 
                HeaderButtonStackPanel.Children.Remove(OrdersButton);
                HeaderButtonStackPanel.Children.Remove(UsersButton);
            }
        }

        private void SetUpView(string view, Order order = null)
        {
            switch (view)
            {
                case "users":
                    if (UserRole.CanViewUsers)
                        AdminMainViewContentControl.Content = new UsersView(UserRole);
                    break;
                case "orders":
                    if (UserRole.CanViewOrders)
                        AdminMainViewContentControl.Content = new OrdersView(UserRole);
                    break;
                case "orderdetails":
                    if (UserRole.CanViewOrders)
                        AdminMainViewContentControl.Content = new OrderDetailsView(order, UserRole);
                    break;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            ActionLogger.Log(GetType().FullName + nameof(LogoutButton_Click), UserRole, "<User_Logout>");

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.ShouldCheckForInactivity = true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.ShouldCheckForInactivity = false;
        }
    }
}
