using WVA_Connect_CSI.WebTools;
using WVA_Connect_CSI.Models.Products;
using WVA_Connect_CSI.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Controllers
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
