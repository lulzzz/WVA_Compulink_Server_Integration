using WVA_Compulink_Server_Integration.Models.Parameters.Derived;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Prescriptions
{
    public class Prescription
    {
        public CustomerID _CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LensRx { get; set; }
        public string ProductCode { get; set; }
        public string SKU { get; set; }
        public string Vendor { get; set; }
        public string UPC { get; set; }
        public string Price { get; set; }
        public bool IsShipToPat { get; set; }
        public bool IsTrial { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }
        public string Date { get; set; }
        public string Eye { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string BaseCurve { get; set; }
        public string Diameter { get; set; }
        public string Sphere { get; set; }
        public string Cylinder { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Color { get; set; }
        public string Multifocal { get; set; }
    }
}
