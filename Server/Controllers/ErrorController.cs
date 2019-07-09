using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.EasyApi;
using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Models.Responses;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Compulink_Server_Integration.Controllers
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
