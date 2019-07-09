using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Models.Configurations;
using WVA_Compulink_Server_Integration.Models.Parameters.Derived;
using WVA_Compulink_Server_Integration.Models.Prescriptions;
using WVA_Compulink_Server_Integration.Models.QueryFormats;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Compulink_Server_Integration.Controllers
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
                        ApiKey = "",
                        Products = compulinkOdbcReader.GetOpenOrders(new string[] {
                            "labsent is null",
                            $"{Startup.config?.WvaInvoiceColumn} is null",
                            $"lab = '{Startup.config?.Location[location]}'",
                            $"lrx.date > {{d '{Startup.config?.OrdersAfterDate ?? DateTime.Today.AddMonths(-1).ToString("d")}'}}"
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
