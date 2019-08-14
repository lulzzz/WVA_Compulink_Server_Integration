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
                string errorMessage = null;
                string wvaStoreId = null;
                OrderResponse orderResponse;
                bool orderSubmitted = false;

                // Create the order in the WVA_Connect_CSI database
                try
                {
                    // If order exists, mark as submit. If not, create new order and mark as submitted
                    orderSubmitted = sqliteDatabase.SubmitOrder(orderWrapper?.OutOrder?.PatientOrder);
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception("Order submission to server database failed.");
                }
                
                // Create the order in the WVA_Connect_CSI database
                try
                {
                    if (orderSubmitted)
                    {
                        string strResponse = API.Post($@"{Paths.WisVisOrders}", orderWrapper);
                        orderResponse = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

                        orderWrapper.OutOrder.PatientOrder.WvaStoreID = orderResponse?.Data?.Wva_order_id;
                        wvaStoreId = orderResponse?.Data?.Wva_order_id ?? throw new Exception("WvaStoreId not returned from API!");

                        if (orderResponse?.Status != "SUCCESS")
                            throw new Exception($"Status message from Orders API was not SUCCESS. Status:{orderResponse?.Status} Message:{orderResponse?.Message}");
                        else
                        {
                            // Update order with wvaStoreId
                            var order = orderWrapper?.OutOrder?.PatientOrder;
                            order.WvaStoreID = wvaStoreId;

                            // Save the order so the wvaStoreId shows up
                            sqliteDatabase.SaveOrder(order);
                        }

                    }
                    else
                    {
                        throw new Exception($"Order was not correctly submitted to the database!");
                    }
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception("Order submission to server database failed.");
                }

                // Find the LensRx for this order and submit it to Compulink
                try
                {
                    if (orderSubmitted)
                    {
                        var listLensRxes = sqliteDatabase.GetLensRxByWvaOrderId(wvaStoreId);
                        new CompulinkOdbcWriter().UpdateLensRx(listLensRxes, wvaStoreId);
                    }
                    else
                    {
                        throw new Exception("Order submission to database failed. Cannot get LensRx.");
                    }
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception("An issue was encountered while trying to update LensRx.");
                }


                /*
                // Create the order in the wva system
                try
                {
                    string strResponse = API.Post($@"{Paths.WisVisOrders}", orderWrapper);
                    orderResponse = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

                    orderWrapper.OutOrder.PatientOrder.WvaStoreID = orderResponse?.Data?.Wva_order_id;
                    wvaStoreId = orderResponse?.Data?.Wva_order_id ?? throw new Exception("WvaStoreId not returned from API!");
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception($"Order submission in WVA system failed. Error:{errorMessage ?? "null"}");
                }

                // Create the order in the WVA_Connect_CSI database
                try
                {
                    if (orderResponse?.Status == "SUCCESS")
                    {
                        // If order exists, mark as submit. If not, create new order and mark as submitted
                        orderSubmitted = sqliteDatabase.SubmitOrder(orderWrapper?.OutOrder?.PatientOrder);
                    }
                    else
                    {
                        throw new Exception($"Status message from Orders API was not SUCCESS. Status:{orderResponse?.Status} Message:{orderResponse?.Message}");
                    }
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception("Order submission to server database failed.");
                }

                // Find the LensRx for this order and submit it to Compulink
                try
                {
                    if (orderSubmitted)
                    {
                        var listLensRxes = sqliteDatabase.GetLensRxByWvaOrderId(wvaStoreId);
                        new CompulinkOdbcWriter().UpdateLensRx(listLensRxes, wvaStoreId);
                    }
                    else
                    {
                        throw new Exception("Order submission to database failed. Cannot get LensRx.");
                    }
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    throw new Exception("An issue was encountered while trying to update LensRx.");
                }
                */

                return orderResponse;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);

                var response = new Response()
                {
                    Status = "FAIL"
                };

                if (x.Message.Contains("Order submission in WVA system failed"))
                    response.Message = "An issue was encountered while trying to submit the order to WVA. Please contact IT if the problem persists.";
                else if ( x.Message.Contains("Order submission to server database failed"))
                    response.Message = "An issue was encountered while trying to submit the order to the server's database. Please contact IT.";
                else if (x.Message.Contains("An issue was encountered while trying to update LensRx"))
                    response.Message = "An issue was encountered while trying to sumbit the LensRx to Compulink. WVA order was still created.";
                else
                    response.Message = "An issue was encoutered while creating this order. Please contact IT.";

                return response;
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
