using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Configurations
{
    public class WvaConfig
    {
        public string Dsn { get; set; }
        public string ApiKey { get; set; }
        public Dictionary<string, string> Location { get; set; }
        public string WvaInvoiceColumn { get; set; }
        public string LabSentColumn { get; set; }
        public string LabColumn { get; set; }
        public string FilterColumn { get; set; }
        public string FilterValue { get; set; }
        public bool OverRideDefaultQueries { get; set; } = false;
    }
}
