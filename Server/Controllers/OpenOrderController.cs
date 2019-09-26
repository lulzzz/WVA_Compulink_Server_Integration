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
using WVA_Connect_CSI.Memory;

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
                
                // Set filter value if it hasn't been set already 
                if (string.IsNullOrEmpty(Startup.config.FilterValue))
                {
                    // Read in the jsonConfig from the file location as a string
                    string strJsonConfig = System.IO.File.ReadAllText(Paths.WvaConfigFile);
                    var jsonConfig = (WvaConfig)JsonConvert.DeserializeObject(strJsonConfig);

                    // Update the FilterValue in memory and in the file system
                    jsonConfig.FilterValue = Startup.config.FilterValue = compulinkOdbcReader.GetLastFilterValue();
                    string configOutput = JsonConvert.SerializeObject(jsonConfig, Formatting.Indented);
                    System.IO.File.WriteAllText(Paths.WvaConfigFile, configOutput);
                }

                return new PrescriptionWrapper()
                {
                    Request = new PrescriptionRequest()
                    {
                        ApiKey = Storage.Config.ApiKey,
                        // Use config settings to get prescriptions (Compulink orders) for this location
                        Products = compulinkOdbcReader.GetOpenOrders(new string[] {
                            $"{Startup.config?.LabSentColumn} is null",
                            $"{Startup.config?.WvaInvoiceColumn} is null",
                            // if using test api key, use default way of pulling orders
                            $"{(Storage.Config.ApiKey == "da1c9295-56af-48bd-aed5-3421dd4d4aaf" ? "lab" : Startup.config?.LabColumn)} = '{Startup.config?.Location?[location] ?? location}'", 
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
