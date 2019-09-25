using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Models;

namespace WVA_Connect_CSI.ViewModels
{
    public class OrdersViewModel
    {
        Database database;

        public OrdersViewModel()
        {
            database = new Database();
        }

        public List<Order> GetAllOrders()
        {
            return database.GetAllOrders();
        }

        public List<Order> GetSubmittedOrders()
        {
            return database.GetSubmittedOrders();
        }

        public List<Order> GetUnsubmittedOrders()
        {
            return database.GetUnsubmittedOrders();
        }
    }
}
