using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Models;

namespace WVA_Connect_CSI.ViewModels
{
    class OrderDetailsViewModel
    {
        Database database;

        public OrderDetailsViewModel()
        {
            database = new Database();
        }

        public List<ItemDetail> GetItemDetail(int orderId)
        {
            return database.GetItemDetail(orderId);
        }

    }
}
