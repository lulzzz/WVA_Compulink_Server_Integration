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
                                    "OfficeName             TEXT, " +
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

        public void UsernameExists(string username)
        {

        }

        public void CheckCredentials(User user)
        {

        }

        public void GetLensRxByWvaOrderId(string wvaOrderId)
        {

        }



        //
        // UPDATE
        //

        public void ChangePassword(string username, string password)
        {

        }

        public void SumbitOrder(Order order)
        {

        }

        public void UnsumbmitOrder(string ordername)
        {

        }


        public void SaveOrder(Order order)
        {

        }

        //
        // DESTROY
        //
        public void DeleteTable(string tableName)
        {

        }

        public void DeleteUser(int id, string username)
        {

        }

        public void DeleteOrder(string ordername)
        {

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
