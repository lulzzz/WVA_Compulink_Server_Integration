using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Models;

namespace WVA_Connect_CSI.ViewModels
{
    class OrdersViewModel
    {
        Database database;

        public OrdersViewModel()
        {
            database = new Database();
        }

        private List<Order> GetAllOrders()
        {
            return database.GetAllOrders();
        }

        private List<Order> GetSubmittedOrders()
        {
            return database.GetSubmittedOrders();
        }

        private List<Order> GetUnsubmittedOrders()
        {
            return database.GetUnsubmittedOrders();
        }

        public List<Order> GetOrders(string selection)
        {

            switch (selection)
            {
                case "all":
                    return GetAllOrders();
                case "submitted":
                    return GetSubmittedOrders();
                case "open":
                    return GetUnsubmittedOrders();
                default:
                    return GetAllOrders();
            }
        }

    }
}
