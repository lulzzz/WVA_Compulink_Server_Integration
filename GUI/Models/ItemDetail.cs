using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    public class ItemDetail
    {
        public string ID { get; set; }
        public string WvaOrderId { get; set; }
        public string Price { get; set; }
        public string CustomerID { get; set; }
        public string Name{ get; set; }
        public string SKU { get; set; }
        public string ProductKey { get; set; }
        public string UPC { get; set; }
        public string BaseCurve { get; set; }
        public string Diameter { get; set; }
        public string Sphere { get; set; }
        public string Cylinder { get; set; }
        public string Axis { get; set; }
        public string Add { get; set; }
        public string Color { get; set; }
        public string Multifocal { get; set; }
        public string LensRx { get; set; }

        [JsonProperty("patient_firstname")]
        public string FirstName { get; set; }

        [JsonProperty("patient_lastname")]
        public string LastName { get; set; }

        [JsonProperty("patient_id")]
        public string PatientID { get; set; }

        [JsonProperty("eye")]
        public string Eye { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("item_retail_price")]
        public string ItemRetailPrice { get; set; }

        [JsonProperty("deleted_flag")]
        public string DeletedFlag { get; set; }

        [JsonProperty("qty_backordered")]
        public int QuantityBackordered { get; set; }

        [JsonProperty("qty_cancelled")]
        public int QuantityCancelled { get; set; }

        [JsonProperty("qty_shipped")]
        public int QuantityShipped { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("item_status")]
        public string ItemStatus { get; set; }

        [JsonProperty("shipping_date")]
        public string ShippingDate { get; set; }

        [JsonProperty("shipping_carrier")]
        public string ShippingCarrier { get; set; }

        [JsonProperty("tracking_url")]
        public string TrackingUrl { get; set; }

        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
    }
}
