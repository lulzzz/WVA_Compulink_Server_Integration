using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Products
{
    public class IProduct
    {
        string Name { get; set; }
        string SKU { get; set; }
        string UPC { get; set; }
        string ProductCode { get; set; }
        string Basecurve { get; set; }
        string Diameter { get; set; }
        string Sphere { get; set; }
        string Cylinder { get; set; }
        string Axis { get; set; }
        string Add { get; set; }
        string Color { get; set; }
        string Multifocal { get; set; }
    }
}
