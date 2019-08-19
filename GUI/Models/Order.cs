using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    class Order
    {
        public string WvaStoreID { get; set; }
        public string OrderName { get; set; }
        public string CreatedDate { get; set; }
        public string ShipToPatient { get; set; }
        public string PoNumber { get; set; }
        public string OrderedBy { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
