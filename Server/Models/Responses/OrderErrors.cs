using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Responses
{
    public class OrderErrors
    {
        [JsonProperty("errors")]
        public List<string> ErrorMessages { get; set; }
    }
}
