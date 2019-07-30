using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Products
{
    public class ProdRequestOut
    {
        [JsonProperty("request")]
        public ProductOut Request { get; set; }
    }
}
