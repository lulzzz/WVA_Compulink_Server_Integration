using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Configurations;
using WVA_Connect_CSI.Models.Parameters.Derived;
using WVA_Connect_CSI.Models.Prescriptions;
using WVA_Connect_CSI.Models.QueryFormats;
using WVA_Connect_CSI.Utilities.Files;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Connect_CSI.Controllers
{
    [Route("api/openorder")]
    public class OpenOrderController : Controller
    {
        [HttpGet("{location}", Name = "GetOpenOrders")]
        public PrescriptionWrapper GetOpenOrders(string location)   
        {
            try
            {
                var compulinkOdbcReader = new CompulinkOdbcReader();
                
                if (string.IsNullOrEmpty(Startup.config.FilterValue))
                {
                    string JsonConfig = System.IO.File.ReadAllText(Paths.WvaConfigFile);
                    dynamic JsonConfigObj = JsonConvert.DeserializeObject(JsonConfig);
                    JsonConfigObj.FilterValue = compulinkOdbcReader.GetLastFilterValue();
                    Startup.config.FilterValue = JsonConfigObj.FilterValue;
                    string ConfigOutput = JsonConvert.SerializeObject(JsonConfigObj, Formatting.Indented);
                    System.IO.File.WriteAllText(Paths.WvaConfigFile, ConfigOutput);
                }
                return new PrescriptionWrapper()
                {
                    Request = new PrescriptionRequest()
                    {
                        ApiKey = Memory.Storage.Config.ApiKey,
                        Products = compulinkOdbcReader.GetOpenOrders(new string[] {
                            $"{Startup.config?.LabSentColumn} is null",
                            $"{Startup.config?.WvaInvoiceColumn} is null",
                            $"{(location == "99999" ? "lab" : Startup.config?.LabColumn)} = '{Startup.config?.Location?[location] ?? location}'", // if test account, use default way of pulling an order
                            $"{Startup.config?.FilterColumn} > {Startup.config?.FilterValue}"
                        })
                    }
                };
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }
    }
}
