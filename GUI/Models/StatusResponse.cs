﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    public class StatusResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("deleted_flag")]
        public string DeletedFlag { get; set; }

        [JsonProperty("processed_flag")]
        public string ProcessedFlag { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
