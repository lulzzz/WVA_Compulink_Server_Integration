﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Validations
{
    public class EmailValidation
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }
}
