﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Configurations
{
    public class WvaConfig
    {
        public string Dsn { get; set; }
        public string ApiKey { get; set; }
        public Dictionary<string, string> Location { get; set; }
        public string WvaInvoiceColumn { get; set; }
        public string LabSentColumn { get; set; }
        public string LabColumn { get; set; }
        public string DateColumn { get; set; }
        public string OrdersAfterDate { get; set; }
        public bool OverRideDefaultQueries { get; set; } = false;
    }
}
