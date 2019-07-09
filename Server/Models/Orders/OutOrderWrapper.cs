using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Orders
{
    public class OutOrderWrapper
    {
        [JsonProperty("request")]
        public OutOrder OutOrder { get; set; }
    }
}
