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
    /// Interaction logic for AdminMainView.xaml
    /// </summary>
    public partial class AdminMainView : UserControl
    {
        private string UserRole;

        private string GetUserRole()
        {
            return UserRole;
        }

        private void SetUserRole(string value)
        {
            UserRole = value;
        }

        public AdminMainView(string role)
        {
            InitializeComponent();
            ResizeView();
            SetRole(role);
            SetView(role);
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
        private void SetRole(string role)
        {
            UserRole = role;
        }

        private void SetView(string view)
        {
            switch (view)
            {
                case "ordersView":
                    AdminMainViewContentControl.Content = new OrdersView();
                    break;
                case "orderDetailsView":
                    AdminMainViewContentControl.Content = new OrderDetailsView();
                    break;
                case "usersView":
                    AdminMainViewContentControl.Content = new UsersView();
                    break;
                default:
                    
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
            SetView("ordersView");
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            SetView("usersView");
        }
    }
}
