using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.WebTools;
using WVA_Connect_CSI.Models.Orders;
using WVA_Connect_CSI.Models.Prescriptions;
using WVA_Connect_CSI.Models.Responses;
using WVA_Connect_CSI.Utilities.Files;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WVA_Connect_CSI.Errors;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Connect_CSI.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private Database sqliteDatabase;

        public OrderController()
        {
            sqliteDatabase = new Database();
        }

        //
        // Get Order(s)
        //

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

        [HttpGet("get-orders/")]
        public IEnumerable<Order> GetOrders()
        {
            return sqliteDatabase.GetWVAOrders();
        }

        [HttpPost("exists")]
        public Order CheckIfOrderExists([FromBody]string orderName)
        {
            return sqliteDatabase.CheckIfOrderExists(orderName);
        }

        //
        // Create Order
        //

        [HttpPost("submit")]
        public Response SubmitOrder([FromBody]OutOrderWrapper orderWrapper)
        {
            try
            {
                // Create the order in the wva system
                bool didSubmitOrdersToApi = SubmitToOrdersApi(orderWrapper, out orderWrapper, out OrderResponse ordersApiResponse);

                if (didSubmitOrdersToApi)
                {
                    // Create or update this order in the local SQLite database 
                    bool didSubmitToLocalDb = SubmitToLocalDb(orderWrapper);

                    // Find the LensRx for this order and mark it as ordered in Compulink
                    bool didSubmitToCompulink = SubmitToCompulink(orderWrapper);
                }
                else
                {
                    throw new Exception($"Failed to create order. Status={ordersApiResponse.Status}, " +
                                                                $"Message={ordersApiResponse.Message}, " +
                                                                $"Data={ordersApiResponse.Data.ToString()}");
                }

                return ordersApiResponse;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);

                var response = new Response()
                {
                    Status = "FAIL",
                    Message = "Failed to create order. Contact IT if the problem persists."
                };

                return response;
            }
        }

        private bool SubmitToOrdersApi(OutOrderWrapper orderWrapper, out OutOrderWrapper order, out OrderResponse ordersApiResponse)
        {
            OrderResponse orderResponse = null; 

            try
            {
                // Send post request to orders api
                string strResponse = API.Post($@"{Paths.WisVisOrders}", orderWrapper);
                orderResponse = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

                // Update the order object with the WvaStoreId
                orderWrapper.OutOrder.PatientOrder.WvaStoreID = orderResponse?.Data?.Wva_order_id;

                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
            finally
            {
                order = orderWrapper;
                ordersApiResponse = orderResponse;
            }
        }

        private bool SubmitToLocalDb(OutOrderWrapper orderWrapper)
        {
            try
            {
                sqliteDatabase.SubmitOrder(orderWrapper?.OutOrder?.PatientOrder);

                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        private bool SubmitToCompulink(OutOrderWrapper orderWrapper)
        {
            try
            {
                if (orderWrapper?.OutOrder?.ApiKey != "da1c9295-56af-48bd-aed5-3421dd4d4aaf")
                {
                    var listLensRxes = sqliteDatabase.GetLensRxByWvaOrderId(orderWrapper.OutOrder.PatientOrder.WvaStoreID);
                    new CompulinkOdbcWriter().UpdateLensRx(listLensRxes, orderWrapper.OutOrder.PatientOrder.WvaStoreID);
                }

                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex); // Will attempt to send email notification
                Error.WriteError(ex.Message + JsonConvert.SerializeObject(orderWrapper)); // Appends order object data to error 
                return false;
            }
        }

        //
        // Save Order
        //

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

        //
        // Delete Order
        //

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
