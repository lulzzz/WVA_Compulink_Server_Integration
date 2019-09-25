using Microsoft.VisualStudio.TestTools.UnitTesting;
using WVA_Connect_CSI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Models;

namespace GUI.UnitTests.ViewModels
{
    [TestClass]
    public class OrderDetailsViewModelTests
    {
        [TestMethod]
        public void GetItemDetail_ReturnsNotNull()
        {
            var orderDetailsViewModel = new OrderDetailsViewModel();

            int orderId = 1557;
            List<ItemDetail> items = orderDetailsViewModel.GetItemDetail(orderId);

            Assert.IsNotNull(items);
        }
    }
}