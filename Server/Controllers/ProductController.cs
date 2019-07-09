using WVA_Compulink_Server_Integration.EasyApi;
using WVA_Compulink_Server_Integration.Models.Products;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        [HttpPost]
        public string Post([FromBody]ProdRequestOut requestOut)
        {
            // Gets all WVA products that are accessible by a given account
            string endpoint = $"{Paths.WisVisProducts}";

            return API.Post(endpoint, requestOut);
        }

    }
}
