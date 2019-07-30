using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Prescriptions
{
    public class PrescriptionWrapper
    {
        [JsonProperty("request")]
        public PrescriptionRequest Request { get; set; }
    }
}
