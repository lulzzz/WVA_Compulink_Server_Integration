using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Responses
{
    public class ResponseData
    {
        [JsonProperty("order")]
        public OrderErrors OrderErrors { get; set; }

        public string Wva_order_id { get; set; }
    }
}
