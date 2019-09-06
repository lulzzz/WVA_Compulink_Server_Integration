using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Orders;
using WVA_Connect_CSI.Models.Users;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.Utilities.Files;

namespace WVA_Connect_CSI.Data
{
    public class SqliteDataAccessor
    {
        //
        // CREATE TABLES
        //

        public void CreateUsersTable()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("CREATE TABLE Users (" +
                                        "Id                     INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                        "UserName               TEXT, " +
                                        "Password               TEXT, " +
                                        "Email                  TEXT); ");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void CreateWvaOrdersTable()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("CREATE TABLE WvaOrders (" +
                                        "Id                     INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                        "CustomerId             TEXT, " +
                                        "OrderName              TEXT, " +
                                        "CreatedDate            TEXT, " +
                                        "WvaStoreId             TEXT, " +
                                        "DateOfBirth            TEXT, " +
                                        "Name1                  TEXT, " +
                                        "Name2                  TEXT, " +
                                        "StreetAddr1            TEXT, " +
                                        "StreetAddr2            TEXT, " +
                                        "City                   TEXT, " +
                                        "State                  TEXT, " +
                                        "Zip                    TEXT, " +
                                        "ShipToAccount          TEXT, " +
                                        "OrderedBy              TEXT, " +
                                        "PoNumber               TEXT, " +
                                        "ShippingMethod         TEXT, " +
                                        "ShipToPatient          TEXT, " +
                                        "Freight                TEXT, " +
                                        "Tax                    TEXT, " +
                                        "Discount               TEXT, " +
                                        "InvoiceTotal           TEXT, " +
                                        "Email                  TEXT, " +
                                        "Phone                  TEXT, " +
                                        "Status                 TEXT); ");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void CreateOrderDetailsTable()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("CREATE TABLE OrderDetails (" +
                                       "ID                      INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                       "WvaOrderId              INT, " +
                                       "LensRx                  INT, " +
                                       "FirstName               TEXT, " +
                                       "LastName                TEXT, " +
                                       "Eye                     TEXT, " +
                                       "Quantity                TEXT, " +
                                       "Price                   TEXT, " +
                                       "PatientId               TEXT, " +
                                       "Name                    TEXT, " +
                                       "ProductReviewed         INT, " +
                                       "Sku                     TEXT, " +
                                       "ProductKey              TEXT, " +
                                       "Upc                     TEXT, " +
                                       "Basecurve               TEXT, " +
                                       "Diameter                TEXT, " +
                                       "Sphere                  TEXT, " +
                                       "Cylinder                TEXT, " +
                                       "Axis                    TEXT, " +
                                       "Ad                      TEXT, " +
                                       "Color                   TEXT, " +
                                       "Multifocal              TEXT); ");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        //
        // CREATE
        //

        public void CreateUser(User user)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"INSERT INTO Users (" +
                                                        $"UserName, " +
                                                        $"Password, " +
                                                        $"Email) " +
                                            $"VALUES (" +
                                                    $"'{user.UserName}', " +
                                                    $"'{user.Password}', " +
                                                    $"'{user.Email}')");
                }
            }
            catch(Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void CreateOrder(Order order, string createdDate, string submitStatus)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"INSERT INTO WvaOrders (" +
                                                            $"CustomerId," +
                                                            $"OrderName," +
                                                            $"CreatedDate," +
                                                            $"WvaStoreId," +
                                                            $"DateOfBirth," +
                                                            $"Name1," +
                                                            $"Name2," +
                                                            $"StreetAddr1," +
                                                            $"StreetAddr2," +
                                                            $"City," +
                                                            $"State," +
                                                            $"Zip," +
                                                            $"OrderedBy," +
                                                            $"PoNumber," +
                                                            $"ShippingMethod," +
                                                            $"ShipToPatient," +
                                                            $"Phone," +
                                                            $"Email," +
                                                            $"Status) " +
                                                $"VALUES (" +
                                                            $"'{order.CustomerID}'," +
                                                            $"'{order.OrderName}'," +
                                                            $"'{createdDate}'," +
                                                            $"'{order.WvaStoreID}'," +
                                                            $"'{Crypto.Encrypt(order.DoB)}'," +
                                                            $"'{Crypto.Encrypt(order.Name_1)}'," +
                                                            $"'{Crypto.Encrypt(order.Name_2)}'," +
                                                            $"'{Crypto.Encrypt(order.StreetAddr_1)}'," +
                                                            $"'{Crypto.Encrypt(order.StreetAddr_2)}'," +
                                                            $"'{Crypto.Encrypt(order.City)}'," +
                                                            $"'{Crypto.Encrypt(order.State)}'," +
                                                            $"'{Crypto.Encrypt(order.Zip)}'," +
                                                            $"'{Crypto.Encrypt(order.OrderedBy)}'," +
                                                            $"'{Crypto.Encrypt(order.PoNumber)}'," +
                                                            $"'{Crypto.Encrypt(order.ShippingMethod)}'," +
                                                            $"'{Crypto.Encrypt(order.ShipToPatient)}'," +
                                                            $"'{Crypto.Encrypt(order.Phone)}'," +
                                                            $"'{Crypto.Encrypt(order.Email)}'," +
                                                            $"'{submitStatus}'" +
                                                            $")");
                }

                int orderId;
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    orderId = cnn.Query<int>($"SELECT ID FROM WvaOrders WHERE OrderName = '{order.OrderName}'").FirstOrDefault();
                }

                foreach (Item item in order.Items)
                {
                    using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                    {
                        cnn.Execute($"INSERT INTO OrderDetails (" +
                                                            $"WvaOrderId," +
                                                            $"LensRx," +
                                                            $"FirstName," +
                                                            $"LastName," +
                                                            $"Eye," +
                                                            $"Quantity," +
                                                            $"Price," +
                                                            $"PatientId," +
                                                            $"Name," +
                                                            $"ProductReviewed," +
                                                            $"Sku," +
                                                            $"ProductKey," +
                                                            $"Upc," +
                                                            $"Basecurve," +
                                                            $"Diameter," +
                                                            $"Sphere," +
                                                            $"Cylinder," +
                                                            $"Axis," +
                                                            $"Ad," +
                                                            $"Color," +
                                                            $"Multifocal) " +
                                                    $"VALUES (" +
                                                            $"'{orderId}'," +
                                                            $"'{item.OrderDetail.LensRx}'," +
                                                            $"'{Crypto.Encrypt(item.FirstName)}'," +
                                                            $"'{Crypto.Encrypt(item.LastName)}'," +
                                                            $"'{item.Eye}'," +
                                                            $"'{item.Quantity}'," +
                                                            $"'{item.ItemRetailPrice}'," +
                                                            $"'{Crypto.Encrypt(item.PatientID)}'," +
                                                            $"'{item.OrderDetail.Name}'," +
                                                            $"'{(item.OrderDetail.ProductReviewed == true ? 1 : 0)}'," +
                                                            $"'{item.OrderDetail.SKU}'," +
                                                            $"'{item.OrderDetail.ProductKey}'," +
                                                            $"'{item.OrderDetail.UPC}'," +
                                                            $"'{item.OrderDetail.Basecurve}'," +
                                                            $"'{item.OrderDetail.Diameter}'," +
                                                            $"'{item.OrderDetail.Sphere}'," +
                                                            $"'{item.OrderDetail.Cylinder}'," +
                                                            $"'{item.OrderDetail.Axis}'," +
                                                            $"'{item.OrderDetail.Ad}'," +
                                                            $"'{item.OrderDetail.Cylinder}'," +
                                                            $"'{item.OrderDetail.Multifocal}')");
                    }
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        //
        // READ
        //

        public List<User> GetAllUsers()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    return cnn.Query<User>($"SELECT * FROM Users").ToList();
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        public Dictionary<string, string> GetOrderNames(string actNum)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    string sql = $"SELECT OrderName, Id FROM WvaOrders WHERE CustomerId = '{actNum}' AND Status = 'open'";
                    return cnn.Query<string, Int64, KeyValuePair<string, Int64>>(sql, (s, i) => new KeyValuePair<string, Int64>(s, i))
                        .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        public List<Order> GetWvaOrders(string actNum)
        {
            try
            { 
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    List<Order> orders = cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE CustomerId = '{actNum}'").ToList();

                    foreach (Order order in orders)
                    {
                       List<ItemOrderDetail> details = cnn.Query<ItemOrderDetail>($"SELECT * FROM OrderDetails WHERE WvaOrderId = '{order.ID}'").ToList();

                        order.Items = GetNestedItems(details);
                    }

                    return DecryptOrders(orders);
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        private Order DecryptOrder(Order order)
        {
            order.DoB = Crypto.Decrypt(order?.DoB);
            order.Name_1 = Crypto.Decrypt(order?.Name_1);
            order.Name_2 = Crypto.Decrypt(order?.Name_2);
            order.StreetAddr_1 = Crypto.Decrypt(order?.StreetAddr_1);
            order.StreetAddr_2 = Crypto.Decrypt(order?.StreetAddr_2);
            order.City = Crypto.Decrypt(order?.City);
            order.State = Crypto.Decrypt(order?.State);
            order.Zip = Crypto.Decrypt(order?.Zip);
            order.OrderedBy = Crypto.Decrypt(order?.OrderedBy);
            order.PoNumber = Crypto.Decrypt(order?.PoNumber);
            order.ShippingMethod = Crypto.Decrypt(order?.ShippingMethod);
            order.ShipToPatient = Crypto.Decrypt(order?.ShipToPatient);
            order.Phone = Crypto.Decrypt(order?.Phone);
            order.Email = Crypto.Decrypt(order?.Email);

            for (int i = 0; i < order.Items.Count; i++)
            {
                order.Items[i].FirstName = Crypto.Decrypt(order?.Items[i]?.FirstName);
                order.Items[i].LastName = Crypto.Decrypt(order?.Items[i]?.LastName);
                order.Items[i].PatientID = Crypto.Decrypt(order?.Items[i]?.PatientID);
            }

            return order;
        }

        private List<Order> DecryptOrders(List<Order> orders)
        {
            for (int i = 0; i < orders.Count; i++)
                orders[i] = DecryptOrder(orders[i]);

            return orders;
        }

        public string GetEmail(string userName)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();


                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                   var q = cnn.Query<string>($"SELECT Email FROM Users WHERE UserName = '{userName}'").FirstOrDefault();

                    watch.Stop();
                    Trace.WriteLine($"=============== {watch.ElapsedMilliseconds}");
                    return q;
                }

               
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        public Order OrderExists(string orderName)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    var order = cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE OrderName = '{orderName}' AND Status = 'open' LIMIT 1").FirstOrDefault();

                    if (order == null)
                        return null;

                    // Purpose of this weird conversion is so we can map our nested order object to a single object that represents the OrderDetails table
                    List<ItemOrderDetail> details = cnn.Query<ItemOrderDetail>($"SELECT * FROM OrderDetails WHERE WvaOrderId = '{order.ID}'").ToList();

                    order.Items = GetNestedItems(details);

                    return DecryptOrder(order);
                }
            } 
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        public bool EmailExists(string email)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    return cnn.Query<string>($"SELECT * FROM Users WHERE Email='{email}'").FirstOrDefault() != null ? true : false;
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return false;
            }
        }

        public bool UsernameExists(string userName)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    return cnn.Query<User>($"SELECT * FROM Users WHERE UserName = '{userName}'").FirstOrDefault() != null ? true : false;
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return false;
            }
        }

        public User CheckCredentials(User user)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    return cnn.Query<User>($"SELECT UserName, Password, Email, RoleId, RequiresPasswordChange FROM Users WHERE UserName='{user.UserName}' AND Password='{user.Password}'").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
                return null;
            }
        }

        //
        // UPDATE
        //

        public void ChangePassword(string userName, string password)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"UPDATE users SET Password ='{password}', RequiresPasswordChange='0' WHERE UserName = '{userName}'");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void SumbitOrder(Order order)
        {
            try
            {
                string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"UPDATE WvaOrders " +
                                                $"SET " +
                                                    $"Status          =   'submitted', " +
                                                    $"CreatedDate     =   '{createdDate}', " +
                                                    $"WvaStoreId      =   '{order.WvaStoreID}' " +
                                                $"WHERE OrderName     =   '{order.OrderName}'");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void UnsumbmitOrder(string orderName)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"UPDATE WvaOrders " +
                                            $"SET " +
                                                $"Status           =   'open' " +
                                            $"WHERE OrderName     =   '{orderName}'");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void SaveOrder(Order order, Order checkOrder)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"UPDATE WvaOrders " +
                                            $"SET " +
                                                $"WvaStoreId       =   '{checkOrder.WvaStoreID}', " +
                                                $"CreatedDate      =   '{checkOrder.CreatedDate}', " +
                                                $"DateOfBirth      =   '{Crypto.Encrypt(order.DoB)}', " +
                                                $"Name1            =   '{Crypto.Encrypt(order.Name_1)}', " +
                                                $"Name2            =   '{Crypto.Encrypt(order.Name_2)}', " +
                                                $"StreetAddr1      =   '{Crypto.Encrypt(order.StreetAddr_1)}', " +
                                                $"StreetAddr2      =   '{Crypto.Encrypt(order.StreetAddr_2)}', " +
                                                $"City             =   '{Crypto.Encrypt(order.City)}', " +
                                                $"State            =   '{Crypto.Encrypt(order.State)}', " +
                                                $"Zip              =   '{Crypto.Encrypt(order.Zip)}', " +
                                                $"OrderedBy        =   '{Crypto.Encrypt(order.OrderedBy)}', " +
                                                $"PoNumber         =   '{Crypto.Encrypt(order.PoNumber)}', " +
                                                $"ShippingMethod   =   '{Crypto.Encrypt(order.ShippingMethod)}', " +
                                                $"ShipToPatient    =   '{Crypto.Encrypt(order.ShipToPatient)}', " +
                                                $"Phone            =   '{Crypto.Encrypt(order.Phone)}', " +
                                                $"Email            =   '{Crypto.Encrypt(order.Email)}', " +
                                                $"Status           =   'open' " +
                                            $"WHERE OrderName      =   '{order.OrderName}';");
                }

                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DELETE FROM OrderDetails WHERE WvaOrderId = '{checkOrder.ID}'");
                }

                foreach (Item item in order.Items)
                {
                    using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                    {
                        cnn.Execute($"INSERT into OrderDetails (" +
                                                       "WvaOrderId, " +
                                                       "FirstName, " +
                                                       "LastName, " +
                                                       "Eye, " +
                                                       "Quantity, " +
                                                       "Price, " +
                                                       "PatientId, " +
                                                       "Name, " +
                                                       "ProductReviewed, " +
                                                       "Sku, " +
                                                       "ProductKey, " +
                                                       "Upc, " +
                                                       "Basecurve, " +
                                                       "Diameter, " +
                                                       "Sphere, " +
                                                       "Cylinder, " +
                                                       "Axis, " +
                                                       "Ad, " +
                                                       "Color, " +
                                                       "Multifocal, " +
                                                       "LensRx)" +
                                                       "values (" +
                                                           $"'{checkOrder.ID}', " +
                                                           $"'{Crypto.Encrypt(item.FirstName)}', " +
                                                           $"'{Crypto.Encrypt(item.LastName)}', " +
                                                           $"'{item.Eye}', " +
                                                           $"'{item.Quantity}', " +
                                                           $"'{item.ItemRetailPrice}', " +
                                                           $"'{Crypto.Encrypt(item.PatientID)}', " +
                                                           $"'{item.OrderDetail.Name}', " +
                                                           $"'{(item.OrderDetail.ProductReviewed == true ? 1 : 0)}', " +
                                                           $"'{item.OrderDetail.SKU}', " +
                                                           $"'{item.OrderDetail.ProductKey}', " +
                                                           $"'{item.OrderDetail.UPC}', " +
                                                           $"'{item.OrderDetail.Basecurve}', " +
                                                           $"'{item.OrderDetail.Diameter}', " +
                                                           $"'{item.OrderDetail.Sphere}', " +
                                                           $"'{item.OrderDetail.Cylinder}', " +
                                                           $"'{item.OrderDetail.Axis}', " +
                                                           $"'{item.OrderDetail.Ad}', " +
                                                           $"'{item.OrderDetail.Color}', " +
                                                           $"'{item.OrderDetail.Multifocal}', " +
                                                           $"'{item.OrderDetail.LensRx}')");
                    }
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        //
        // DESTROY
        //
        public void DeleteTable(string tableName)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DROP TABLE {tableName}");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void DeleteUser(int id, string userName)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DELETE FROM Users WHERE Id='{id}' AND UserName='{userName}'");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        public void DeleteOrder(string orderName)
        {
            try
            {
                int orderId;

                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    orderId = cnn.Query<int>($"SELECT ID FROM WvaOrders WHERE OrderName = '{orderName}'").FirstOrDefault();
                }

                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DELETE FROM WvaOrders WHERE OrderName = '{orderName}'");
                }

                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DELETE FROM OrderDetails WHERE WvaOrderId = '{orderId}'");
                }
            }
            catch (Exception ex)
            {
                Error.WriteError(ex);
            }
        }

        //
        // GET DATABASE CONNECTION STRING
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }

        //
        // Convert a ItemOrderDetail object (ORM style) to an Item object (nested class style) to follow Dapper ORM protocol
        //

        private List<Item> GetNestedItems(List<ItemOrderDetail> details)
        {
            var items = new List<Item>();

            foreach (ItemOrderDetail detail in details)
            {
                var item = new Item()
                {
                    ID = detail.ID,
                    FirstName = detail.FirstName,
                    LastName = detail.LastName,
                    PatientID = detail.PatientID,
                    Eye = detail.Eye,
                    Quantity = detail.Quantity,
                    ItemRetailPrice = detail.ItemRetailPrice,
                    OrderDetail = new OrderDetail()
                    {
                        Name = detail.Name,
                        SKU = detail.SKU,
                        UPC = detail.UPC,
                        Basecurve = detail.Basecurve,
                        Diameter = detail.Diameter,
                        Sphere = detail.Sphere,
                        Cylinder = detail.Cylinder,
                        Axis = detail.Axis,
                        Ad = detail.Ad,
                        Color = detail.Color,
                        Multifocal = detail.Multifocal,
                        ProductCode = detail.ProductCode,
                        ProductReviewed = detail.ProductReviewed,
                        ProductKey = detail.ProductKey,
                        LensRx = detail.LensRx
                    }
                };

                items.Add(item);
            };

            return items;
        }

    }
}
