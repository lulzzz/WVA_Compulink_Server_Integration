using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    public class Prescription
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public bool IsTrial { get; set; }
        public string LensRx { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        private string patient { get; set; }
        public string Patient
        {
            get { return $"{LastName}, {FirstName}"; }
            set { patient = value; }
        }
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
