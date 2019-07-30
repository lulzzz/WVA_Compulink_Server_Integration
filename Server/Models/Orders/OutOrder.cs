using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Orders
{
    public class OutOrder
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("order")]
        public Order PatientOrder { get; set; }
    }
}
