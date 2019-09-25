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
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.ViewModels;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        Role userRole;
        OrdersViewModel ordersViewModel;
        List<Order> Orders;

        public OrdersView()
        {

        }

        public OrdersView(Role role)
        {
            InitializeComponent();
            ordersViewModel = new OrdersViewModel();
            userRole = role;
            Orders = new List<Order>();
            SetUpDataGrid();
        }

        private void WvaOrdersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int index = GetSelectedOrderIndex();

                foreach (Window window in Application.Current.Windows)
                    if (window.GetType() == typeof(MainWindow))
                        (window as MainWindow).MainContentControl.DataContext = new AdminMainView(userRole.RoleId, userRole.UserName, "orderdetails", (Order)WvaOrdersDataGrid.Items[index]);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private int GetSelectedOrderIndex()
        {
            return WvaOrdersDataGrid.SelectedIndex;
        }

        private List<Order> GetOrderActionFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return ordersViewModel.GetAllOrders();
                case 1:
                    return ordersViewModel.GetSubmittedOrders(); 
                case 2:
                    return ordersViewModel.GetUnsubmittedOrders();
                default:
                    return ordersViewModel.GetAllOrders();
            }
        }

        private void SetUpDataGrid()
        {
            WvaOrdersDataGrid.Items.Clear();
            Orders.Clear();

            foreach (Order order in GetOrderActionFromIndex(OrderTypeComboBox.SelectedIndex))
                WvaOrdersDataGrid.Items.Add(order);

            Orders.AddRange(WvaOrdersDataGrid.Items.Cast<Order>().ToList());
            WvaOrdersDataGrid.Items.Refresh();
        }

        private void SetUpDataGrid(List<Order> orders)
        {
            WvaOrdersDataGrid.Items.Clear();

            foreach (Order order in orders)
                WvaOrdersDataGrid.Items.Add(order);

            WvaOrdersDataGrid.Items.Refresh();
        }

        private void SearchOrdersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            var tempOrders = new List<Order>();
            tempOrders.AddRange(Orders);

            switch (SearchOrdersComboBox.SelectedIndex)
            {
                case 0:
                    SetUpDataGrid(tempOrders.Where(x => x.WvaStoreID != null && x.WvaStoreID.StartsWith(SearchOrdersTextBox.Text)).ToList());
                    break;
                case 1:
                    SetUpDataGrid(tempOrders.Where(x => x.OrderName != null && x.OrderName.ToLower().Contains(SearchOrdersTextBox.Text)).ToList());
                    break;
                case 2:
                    SetUpDataGrid(tempOrders.Where(x => x.CreatedDate != null && x.CreatedDate.ToLower().Contains(SearchOrdersTextBox.Text)).ToList());
                    break;
                case 3:
                    SetUpDataGrid(tempOrders.Where(x => x.ShipToPatient != null && x.ShipToPatient.ToLower().StartsWith(SearchOrdersTextBox.Text)).ToList());
                    break;
                case 4:
                    SetUpDataGrid(tempOrders.Where(x => x.PoNumber != null && x.PoNumber.StartsWith(SearchOrdersTextBox.Text)).ToList());
                    break;
                case 5:
                    SetUpDataGrid(tempOrders.Where(x => x.OrderedBy != null && x.OrderedBy.ToLower().Contains(SearchOrdersTextBox.Text)).ToList());
                    break;
            }
        }

        private void OrderTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WvaOrdersDataGrid?.Items != null)
                SetUpDataGrid();
        }
    }
}
