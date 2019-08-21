using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    class WvaOrder
    {
        public int CustomerId { get; set; }
        public string OrderName { get; set; }
        public string CreatedDate { get; set; }
        public string WvaStoreID { get; set; }
        public string DateOfBirth { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string StreetAddr1 { get; set; }
        public string StreetAddr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ShipToAccount { get; set; }
        public string OrderedBy { get; set; }
        public string PoNumber { get; set; }
        public string ShippingMethod { get; set; }
        public string ShipToPatient { get; set; }
        public string Freight { get; set; }
        public string Tax { get; set; }
        public string Discount { get; set; }
        public double InvoiceTotal { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
    }
}
