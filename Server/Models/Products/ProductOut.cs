using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Products
{
    public class ProductOut
    {
        [JsonProperty("api_key")]
        public string Api_key { get; set; }

        [JsonProperty("account_id")]
        public string AccountID { get; set; }
    }
}
