using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Data;
using WVA_Compulink_Server_Integration.WebTools;
using WVA_Compulink_Server_Integration.Models.Orders;
using WVA_Compulink_Server_Integration.Models.Prescriptions;
using WVA_Compulink_Server_Integration.Models.Responses;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Compulink_Server_Integration.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private Database sqliteDatabase;

        public OrderController()
        {
            sqliteDatabase = new Database();
        }


        [HttpGet("get-names/{act_num}")]
        public Dictionary<string, string> GetOrdersNames(string act_num)
        {
            return sqliteDatabase.GetOrderNames(act_num);
        }

        [HttpGet("get-orders/{act_num}")]
        public IEnumerable<Order> GetOrders(string act_num)
        {
            return sqliteDatabase.GetWVAOrders(act_num);
        }

        [HttpPost("exists")]
        public Order CheckIfOrderExists([FromBody]string orderName)
        {
            return sqliteDatabase.CheckIfOrderExists(orderName);
        }

        [HttpPost("submit")]
        public Response SubmitOrder([FromBody]OutOrderWrapper orderWrapper)
        {
            try
            {
                // Attempt to create the order in WisVis system
                string strResponse = API.Post($@"{Paths.WisVisOrders}", orderWrapper);
                var response = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

                // Update this order's WvaStoreID using the OrderResponse object
                orderWrapper.OutOrder.PatientOrder.WvaStoreID = response?.Data?.Wva_order_id;

                // Submit order in server database if we got a good response
                if (response?.Status == "SUCCESS")
                {
                    // If order exists, mark as submit. If not, create new order and mark as submitted
                    bool orderSubmitted = sqliteDatabase.SubmitOrder(orderWrapper?.OutOrder?.PatientOrder);

                    if (orderSubmitted)
                    {
                        var listLensRxes = sqliteDatabase.GetLensRxByWvaOrderId(response?.Data?.Wva_order_id);
                        var compulinkOdbcWriter = new CompulinkOdbcWriter();

                        compulinkOdbcWriter.UpdateLensRx(listLensRxes, response.Data?.Wva_order_id);
                    }
                    else
                    {
                        throw new Exception("Order submission to database failed.");
                    }
                }

                return response;
            }
            catch (Exception x)
            {
                // If order creation failed, set order back to 'open' status
                sqliteDatabase.UnsubmitOrder(orderWrapper?.OutOrder?.PatientOrder?.OrderName);

                // Return fail response
                return new Response()
                {
                    Status = "FAIL",
                    Message = $"{x.Message}"
                };
            }
        }

        [HttpPost("save")]
        public Response SaveOrder([FromBody]OutOrderWrapper outOrderWrapper)
        {
            Response response = new Response();

            try
            {
                bool didSave = sqliteDatabase.SaveOrder(outOrderWrapper.OutOrder.PatientOrder);

                if (didSave)
                {
                    response.Status = "SUCCESS";
                    response.Message = "Order saved.";
                }
                else
                    throw new Exception("Failed to save order.");
            }
            catch (Exception x)
            {
                response.Status = "FAIL";
                response.Message = $"{x.Message}";
            }

            return response;
        }

        [HttpPost("delete")]
        public Response DeleteOrder([FromBody]string orderName)
        {
            bool orderDeleted = sqliteDatabase.DeleteOrder(orderName);
            Response response = new Response();

            if (orderDeleted)
                response.Status = "SUCCESS";
            else
                response.Status = "FAIL";

            return response;
        }

    }
}
