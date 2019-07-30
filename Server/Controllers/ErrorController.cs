using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.WebTools;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Responses;
using WVA_Connect_CSI.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Connect_CSI.Controllers
{
    [Route("api/[controller]")]
    public class ErrorController : Controller
    {
        // POST api/<controller>
        [HttpPost]
        public bool Post([FromBody]JsonError error)
        {
            try
            {
                string endpoint = $"{Paths.WisVisErrors}";
                string strResponse = API.Post(endpoint, error);
                var response = JsonConvert.DeserializeObject<Response>(strResponse);

                return response.Status.Contains("SUCCESS") ? true : false;
            }
            catch
            {
                return false;
            }
        }
    }
}
