using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Configurations;
using WVA_Connect_CSI.Models.Parameters.Derived;
using WVA_Connect_CSI.Models.Prescriptions;
using WVA_Connect_CSI.Models.QueryFormats;
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

                return new PrescriptionWrapper()
                {
                    Request = new PrescriptionRequest()
                    {
                        ApiKey = Memory.Storage.Config.ApiKey,
                        Products = compulinkOdbcReader.GetOpenOrders(new string[] {
                            $"{Startup.config?.LabSentColumn} is null",
                            $"{Startup.config?.WvaInvoiceColumn} is null",
                            $"{Startup.config?.LabColumn} = '{Startup.config?.Location?[location] ?? location}'",
                            $"{Startup.config?.DateColumn} > {{d '{Startup.config?.OrdersAfterDate ?? DateTime.Today.AddMonths(-1).ToString("d")}'}}"
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
