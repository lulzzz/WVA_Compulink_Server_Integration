using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Validations
{
    public class EmailValidationCode : EmailValidation
    {
        [JsonProperty("email_code")]
        public string EmailCode { get; set; }
    }
}
