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


        //
        // UPDATE
        //


        //
        // DESTROY
        //


        //
        // GET DATABASE CONNECTION STRING
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }


    }
}
