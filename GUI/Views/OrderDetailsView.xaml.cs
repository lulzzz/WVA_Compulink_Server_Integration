using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using WVA_Connect_CSI.Utility.Files;
using WVA_Connect_CSI.ViewModels;
using WVA_Connect_CSI.WebTools;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailsView.xaml
    /// </summary>
    public partial class OrderDetailsView : UserControl
    {
        Order order;
        List<ItemDetail> itemDetails;
        OrderDetailsViewModel orderDetailsViewModel;

        public OrderDetailsView()
        {

        }

        public OrderDetailsView(Order o)
        {
            orderDetailsViewModel = new OrderDetailsViewModel();
            order = o;
            InitializeComponent();
            SetItemDetails();
            SetUp();
        }

        private void SetItemDetails()
        {
            if (order == null)
                return;

            itemDetails = orderDetailsViewModel.GetItemDetail(order.Id);
        }

        private string GetShippingTypeID(string shipType)
        {
            switch (shipType)
            {
                case "1":
                    return "Standard";
                case "D":
                    return "UPS Ground";
                case "J":
                    return "UPS 2nd Day Air";
                case "P":
                    return "UPS Next Day Air";
                default:
                    return shipType;
            }
        }

        private void SetUp()
        {
            // Header
            OrderNameLabel.Content = order.OrderName;
            OrderedByLabel.Content = order.OrderedBy;
            AccountIDLabel.Content = order.CustomerId;
            OrderIDLabel.Content = $"WVA Order ID: {order.WvaStoreID}";

            // Sub-header (if value is not null or blank, add it to a stack panel column so the view scales smoothly)
            if (order.Name1 != null && order.Name1.Trim() != "")
                StackPanelAddLeftChild($"Addressee: {order.Name1}");

            if (order.StreetAddr1 != null && order.StreetAddr1.Trim() != "")
                StackPanelAddLeftChild($"Address: {order.StreetAddr1}");

            if (order.ShippingMethod != null && order.ShippingMethod.Trim() != "")
                StackPanelAddLeftChild($"Ship Type: {GetShippingTypeID(order.ShippingMethod)}");

            if (order.Phone != null && order.Phone.Trim() != "")
                StackPanelAddLeftChild($"Phone: {order.Phone}");

            if (order.City != null && order.City.Trim() != "")
                StackPanelAddRightChild($"City: {order.City}");

            if (order.State != null && order.State.Trim() != "")
                StackPanelAddRightChild($"State: {order.State}");

            if (order.Zip != null && order.Zip.Trim() != "")
                StackPanelAddRightChild($"Zip: {order.Zip}");

            if (order.StreetAddr2 != null && order.StreetAddr2.Trim() != "")
                StackPanelAddRightChild($"Suite/Apt: {order.StreetAddr2}");

            // Grid items
            foreach (ItemDetail item in itemDetails)
            {
                ReviewOrderDataGrid.Items.Add(new Prescription()
                {
                    FirstName = item?.FirstName,
                    LastName = item?.LastName,
                    Eye = item?.Eye,
                    Product = item?.Name,
                    Quantity = item?.Quantity,
                    BaseCurve = item?.BaseCurve,
                    Diameter = item?.Diameter,
                    Sphere = item?.Sphere,
                    Cylinder = item?.Cylinder,
                    Axis = item?.Axis,
                    Add = item?.Add,
                    Color = item?.Color,
                    Multifocal = item?.Multifocal
                });
            }
        }

        private void StackPanelAddLeftChild(string content)
        {
            LeftInnerStackPanel.Children.Add(new Label()
            {
                Content = content,
                FontFamily = new FontFamily("Sitka Text"),
                FontSize = 16
            });
        }

        private void StackPanelAddRightChild(string content)
        {
            RightInnerStackPanel.Children.Add(new Label()
            {
                Content = content,
                FontFamily = new FontFamily("Sitka Text"),
                FontSize = 16
            });
        }

        private string GetApiKey()
        {
            try
            {
                var config = JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText($@"{Paths.WvaConfigFile}"));

                return config.ApiKey;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        private Order UpdateOrderStatus(Order order)
        {
            if (order.WvaStoreID == null || order.WvaStoreID.Trim() == "" || order.Status == "open")
                return order;

            RequestWrapper request = new RequestWrapper()
            {
                Request = new StatusRequest()
                {
                    ApiKey = GetApiKey(),
                    CustomerId = order.CustomerId.ToString(),
                    WvaStoreNumber = order.WvaStoreID
                }
            };

            string statusEndpoint = Paths.WisVisOrderStatus;
            string strStatusResponse = API.Post(statusEndpoint, request);

            if (strStatusResponse == null || strStatusResponse.Trim() == "")
                return order;

            var statusResponse = JsonConvert.DeserializeObject<StatusResponse>(strStatusResponse);

            order.Message = statusResponse.Message;
            order.ProcessedFlag = statusResponse.ProcessedFlag;

            if (statusResponse.Items == null || statusResponse.Items?.Count < 1)
                return order;

            for (int i = 0; i < statusResponse.Items.Count; i++)
            {
                itemDetails[i].DeletedFlag = statusResponse.DeletedFlag;
                itemDetails[i].QuantityBackordered = statusResponse.Items[i].QuantityBackordered;
                itemDetails[i].QuantityCancelled = statusResponse.Items[i].QuantityCancelled;
                itemDetails[i].QuantityShipped = statusResponse.Items[i].QuantityShipped;
                itemDetails[i].Status = statusResponse.Items[i].Status;
                itemDetails[i].ItemStatus = statusResponse.Items[i].ItemStatus;
                itemDetails[i].ShippingDate = statusResponse.Items[i].ShippingDate;
                itemDetails[i].ShippingCarrier = statusResponse.Items[i].ShippingCarrier;
                itemDetails[i].TrackingUrl = statusResponse.Items[i].TrackingUrl;
                itemDetails[i].TrackingNumber = statusResponse.Items[i].TrackingNumber;
            }

            return order;
        }

        private void AddItemToGridContextMenu(string headerContent)
        {
            try
            {
                MenuItem item = new MenuItem() { Header = headerContent };
                item.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                WVA_OrdersContextMenu.Items.Add(item);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void WVA_OrdersContextMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = sender as MenuItem;
                string selectedItem = menuItem.Header.ToString();

                if (selectedItem.Contains("https"))
                {
                    Process.Start(selectedItem);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void SetUpContextMenu()
        {
            WVA_OrdersContextMenu.Items.Clear();

            int selectedRowIndex = ReviewOrderDataGrid.Items.IndexOf(ReviewOrderDataGrid.CurrentItem);
            order = UpdateOrderStatus(order);

            if (selectedRowIndex < 0)
                return;

            if (itemDetails[selectedRowIndex].DeletedFlag != null && itemDetails[selectedRowIndex].DeletedFlag.ToLower() == "y")
                AddItemToGridContextMenu("-- DELETED ORDER! --");

            if (itemDetails[selectedRowIndex].QuantityBackordered > 0)
                AddItemToGridContextMenu($"Quantity backordered: {itemDetails[selectedRowIndex].QuantityBackordered}");

            if (itemDetails[selectedRowIndex].QuantityCancelled > 0)
                AddItemToGridContextMenu($"Quantity cancelled: {itemDetails[selectedRowIndex].QuantityCancelled}");

            if (itemDetails[selectedRowIndex].QuantityShipped > 0)
                AddItemToGridContextMenu($"Quantity shipped: {itemDetails[selectedRowIndex].QuantityShipped}");

            if (itemDetails[selectedRowIndex].Status != null && itemDetails[selectedRowIndex].Status.Trim() != "")
                AddItemToGridContextMenu($"Status: {itemDetails[selectedRowIndex].Status}");

            if (itemDetails[selectedRowIndex].ItemStatus != null && itemDetails[selectedRowIndex].ItemStatus.Trim() != "")
                AddItemToGridContextMenu($"Item Status: {itemDetails[selectedRowIndex].ItemStatus}");

            if (itemDetails[selectedRowIndex].ShippingDate != null && itemDetails[selectedRowIndex].ShippingDate.Trim() != "")
                AddItemToGridContextMenu($"Shipping Date: {itemDetails[selectedRowIndex].ShippingDate}");

            if (itemDetails[selectedRowIndex].ShippingCarrier != null && itemDetails[selectedRowIndex].ShippingCarrier.Trim() != "")
                AddItemToGridContextMenu($"Shipping Carrier: {itemDetails[selectedRowIndex].ShippingCarrier}");

            if (itemDetails[selectedRowIndex].TrackingUrl != null && itemDetails[selectedRowIndex].TrackingUrl.Trim() != "")
                AddItemToGridContextMenu($"{itemDetails[selectedRowIndex].TrackingUrl}");

            if (itemDetails[selectedRowIndex].TrackingNumber != null && itemDetails[selectedRowIndex].TrackingNumber.Trim() != "")
                AddItemToGridContextMenu($"Tracking Number: {itemDetails[selectedRowIndex].TrackingNumber}");
        }

        private void ReviewOrderDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            SetUpContextMenu();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new AdminMainView(1);
        }
    }
}
