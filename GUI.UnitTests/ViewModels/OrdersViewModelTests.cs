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
    public class OrdersViewModelTests
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

            // Leave test if no results returned
            if (orders.Count < 1)
                return;

            string actual = orders[0].Status;
            string expected = "submitted";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUnSubmittedOrders_ReturnsUnsubmittedOrders()
        {
            var ordersViewModel = new OrdersViewModel();

            List<Order> orders = ordersViewModel.GetUnsubmittedOrders();

            // Leave test if no results returned
            if (orders.Count < 1)
                return;

            string actual = orders[0].Status;
            string expected = "open";

            Assert.AreEqual(expected, actual);
        }

    }
}
