using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.ViewModels;

namespace GUI.UnitTests.ViewModels
{
    [TestClass]
    class OrdersViewModelTests
    {
        [TestMethod]
        public void GetAllOrders_ReturnsSubAndUnSubbedOrders()
        {
            var ordersViewModel = new OrdersViewModel();

            List<Order> orders = ordersViewModel.GetAllOrders();

            Assert.IsNotNull(orders);
        }

        [TestMethod]
        public void GetSubmittedOrders_ReturnsSubmittedOrders()
        {
            var ordersViewModel = new OrdersViewModel();

            List<Order> orders = ordersViewModel.GetSubmittedOrders();
            string actualStatus = orders[0].Status;
            string expectedStatus = "submitted";

            Assert.Equals(actualStatus, expectedStatus);
        }

        [TestMethod]
        public void GetUnSubmittedOrders_ReturnsUnsubmittedOrders()
        {
            var ordersViewModel = new OrdersViewModel();

            List<Order> orders = ordersViewModel.GetUnsubmittedOrders();
            string actualStatus = orders[0].Status;
            string expectedStatus = "open";

            Assert.Equals(actualStatus, expectedStatus);
        }

    }
}
