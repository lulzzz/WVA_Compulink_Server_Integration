using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Orders
{
    public class ItemOrderDetail
    {
        [JsonProperty("id")]
        public string ID { get; set; }

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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("wva_sku")]
        public string SKU { get; set; }

        [JsonProperty("upc")]
        public string UPC { get; set; }

        [JsonProperty("bc")]
        public string Basecurve { get; set; }

        [JsonProperty("dia")]
        public string Diameter { get; set; }

        [JsonProperty("sph")]
        public string Sphere { get; set; }

        [JsonProperty("cyl")]
        public string Cylinder { get; set; }

        [JsonProperty("ax")]
        public string Axis { get; set; }

        [JsonProperty("add")]
        public string Add { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("multifocal")]
        public string Multifocal { get; set; }

        public string ProductCode { get; set; }

        [JsonProperty("product_reviewed")]
        public bool ProductReviewed { get; set; }

        [JsonProperty("product_key")]
        public string ProductKey { get; set; }

        [JsonProperty("lens_rx")]
        public string LensRx { get; set; }
    }
}
