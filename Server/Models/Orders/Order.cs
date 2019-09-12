using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Orders
{
    public class Order
    {
        [JsonProperty("customer_id")]
        public string CustomerID { get; set; }

        [JsonProperty("order_name")]
        public string OrderName { get; set; }

        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }

        [JsonProperty("wva_store_id")]
        public string WvaStoreID { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("dob")]
        public string DoB { get; set; }

        [JsonProperty("name_1")]
        public string Name1 { get; set; }

        [JsonProperty("name_2")]
        public string Name2 { get; set; }

        [JsonProperty("street_address_1")]
        public string StreetAddr1 { get; set; }

        [JsonProperty("street_address_2")]
        public string StreetAddr2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("ordered_by")]
        public string OrderedBy { get; set; }

        [JsonProperty("po_number")]
        public string PoNumber { get; set; }

        [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; }

        [JsonProperty("ship_to_patient")]
        public string ShipToPatient { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
