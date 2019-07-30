using WVA_Connect_CSI.Models.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Orders
{
    public class OrderDetail : Product
    {
        [JsonProperty("product_reviewed")]
        public bool ProductReviewed { get; set; }

        [JsonProperty("product_key")]
        public string ProductKey { get; set; }

        [JsonProperty("lens_rx")]
        public string LensRx { get; set; }
    }
}
