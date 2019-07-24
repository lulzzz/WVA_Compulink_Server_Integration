using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Models.Orders;
using WVA_Compulink_Server_Integration.Models.Users;
using WVA_Compulink_Server_Integration.Utilities.Files;

namespace WVA_Compulink_Server_Integration.Data
{
    public class SqliteDataAccessor
    {
        //
        // CREATE TABLES
        //

        public void CreateUsersTable()
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

        public void CreateWvaOrdersTable()
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

        public void CreateOrderDetailsTable()
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

        //
        // CREATE
        //

        public void CreateUser(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"INSERT INTO Users (" +
                                                    $"Username, " +
                                                    $"Password, " +
                                                    $"Email) " +
                                        $"VALUES (" +
                                                $"'{user.UserName}', " +
                                                $"'{user.Password}, " +
                                                $"'{user.Email}')");
            }
        }

        public void CreateOrder(Order order, string createdDate, string submitStatus)
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
                                                        $"'{order.CreatedDate}'," +
                                                        $"'{order.WvaStoreID}'," +
                                                        $"'{order.DoB}'," +
                                                        $"'{order.Name_1}'," +
                                                        $"'{order.Name_2}'," +
                                                        $"'{order.StreetAddr_1}'," +
                                                        $"'{order.StreetAddr_2}'," +
                                                        $"'{order.City}'," +
                                                        $"'{order.State}'," +
                                                        $"'{order.Zip}'," +
                                                        $"'{order.OrderedBy}'," +
                                                        $"'{order.PoNumber}'," +
                                                        $"'{order.ShippingMethod}'," +
                                                        $"'{order.ShipToPatient}'," +
                                                        $"'{order.Phone}'," +
                                                        $"'{order.Email}'," +
                                                        $"'{submitStatus}'," +
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
                    cnn.Execute($"INSERT INTO OrderDetail (" +
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
                                                        $"'{item.FirstName}'," +
                                                        $"'{item.LastName}'," +
                                                        $"'{item.Eye}'," +
                                                        $"'{item.Quantity}'," +
                                                        $"'{item.ItemRetailPrice}'," +
                                                        $"'{item.PatientID}'," +
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
                                                        $"'{item.OrderDetail.Add}'," +
                                                        $"'{item.OrderDetail.Cylinder}'," +
                                                        $"'{item.OrderDetail.Multifocal}')");
                }
            }
        }

        //
        // READ
        //

        public List<User> GetAllUsers()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<User>($"SELECT * FROM Users").ToList();
            }
        }

        public Dictionary<string, string> GetOrderNames(string actNum)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                string sql = $"SELECT OrderName, Id FROM WvaOrders WHERE CustomerId = '{actNum}' AND Status = 'open'";
                return cnn.Query<string, string, KeyValuePair<string, string>>(sql, (s, i) => new KeyValuePair<string, string>(s, i))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            }
        }

        public List<Order> GetWvaOrders(string actNum)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                List<Order> orders = cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE CustomerId = '{actNum}'").ToList();

                foreach (Order order in orders)
                {
                    order.Items = cnn.Query<Item>($"SELECT * FROM OrderDetails WHERE WvaOrderId = '{order.ID}'").ToList();
                }

                return orders;
            }
        }

        public string GetEmail(string userName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
               return cnn.Query<string>($"SELECT email FROM users WHERE user_name = '{userName}'").FirstOrDefault();
            }
        }

        public Order OrderExists(string orderName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var order = cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE OrderName = '{orderName}' AND Status = 'open' LIMIT 1").FirstOrDefault();

                order.Items = cnn.Query<Item>($"SELECT * FROM OrderDetails WHERE WvaOrderId = '{order.ID}'").ToList();

                return order;
            }
        }

        public bool EmailExists(string email)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<string>($"SELECT * FROM Users WHERE Email='{email}'").FirstOrDefault() != null ? true : false;
            }
        }

        public bool UsernameExists(string userName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<User>($"SELECT * FROM users WHERE user_name = '{userName}'").FirstOrDefault() != null ? true : false;
            }
        }

        public User CheckCredentials(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<User>($"SELECT user_name, password, email FROM users WHERE user_name='{user.UserName}' AND password='{user.Password}'").FirstOrDefault();
            }
        }

        //
        // UPDATE
        //

        public void ChangePassword(string userName, string password)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE users SET Password ='{password}' WHERE UserName = '{userName}'");
            }
        }

        public void SumbitOrder(Order order)
        {
            string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE WvaOrders " +
                                            $"SET " +
                                                $"Status           =   'submitted', " +
                                                $"CreatedDate     =   '{createdDate}', " +
                                                $"WvaStoreId     =   '{order.WvaStoreID}' " +
                                            $"WHERE OrderName     =   '{order.OrderName}'");
            }
        }

        public void UnsumbmitOrder(string orderName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE WvaOrders " +
                                        $"SET " +
                                            $"Status           =   'open' " +
                                        $"WHERE OrderName     =   '{orderName}'");
            }
        }


        public void SaveOrder(Order order, Order checkOrder)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE WvaOrders " +
                                        $"SET " +
                                            $"WvaStoreId       =   '{checkOrder.WvaStoreID}', " +
                                            $"CreatedDate      =   '{checkOrder.CreatedDate}', " +
                                            $"DateOfBirth      =   '{order.DoB}', " +
                                            $"Name1            =   '{order.Name_1}', " +
                                            $"Name2            =   '{order.Name_2}', " +
                                            $"StreetAddr1      =   '{order.StreetAddr_1}', " +
                                            $"StreetAddr2      =   '{order.StreetAddr_2}', " +
                                            $"City             =   '{order.City}', " +
                                            $"State            =   '{order.State}', " +
                                            $"Zip              =   '{order.Zip}', " +
                                            $"OrderedBy        =   '{order.OrderedBy}', " +
                                            $"PoNumber         =   '{order.PoNumber}', " +
                                            $"ShippingMethod   =   '{order.ShippingMethod}', " +
                                            $"ShipToPatient    =   '{order.ShipToPatient}', " +
                                            $"Phone            =   '{order.Phone}', " +
                                            $"Email            =   '{order.Email}', " +
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
                                                   "Multifocal) " +
                                                   "values (" +
                                                       $"'{checkOrder.ID}', " +
                                                       $"'{item.FirstName}', " +
                                                       $"'{item.LastName}', " +
                                                       $"'{item.Eye}', " +
                                                       $"'{item.Quantity}', " +
                                                       $"'{item.ItemRetailPrice}', " +
                                                       $"'{item.PatientID}', " +
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
                                                       $"'{item.OrderDetail.Add}', " +
                                                       $"'{item.OrderDetail.Color}', " +
                                                       $"'{item.OrderDetail.Multifocal}'");
                }
            }
        }

        //
        // DESTROY
        //
        public void DeleteTable(string tableName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"DROP TABLE {tableName}");
            }
        }

        public void DeleteUser(int id, string userName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"DELETE FROM Users WHERE Id='{id}' AND UserName='{userName}'");
            }
        }

        public void DeleteOrder(string orderName)
        {
            int orderId;

            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                orderId  = cnn.Query<int>($"SELECT ID FROM WvaOrders WHERE OrderName = '{orderName}'").FirstOrDefault();
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

        //
        // GET DATABASE CONNECTION STRING
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }


    }
}
