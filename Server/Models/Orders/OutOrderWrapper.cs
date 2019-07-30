using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Orders
{
    public class OutOrderWrapper
    {
        [JsonProperty("request")]
        public OutOrder OutOrder { get; set; }
    }
}
