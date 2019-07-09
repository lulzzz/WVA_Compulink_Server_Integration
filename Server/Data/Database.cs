using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Models.Orders;
using WVA_Compulink_Server_Integration.Models.Users;
using WVA_Compulink_Server_Integration.Utilities.Files;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace WVA_Compulink_Server_Integration.Data
{
    public class Database
    {
        // =========================================================================================================
        // CREATE
        // =========================================================================================================

        // Creates a SQLite database file
        public static void CreateDatabaseFile()
        {
            if (!Directory.Exists(Paths.DataDir))
                Directory.CreateDirectory(Paths.DataDir);


            SQLiteConnection.CreateFile(Paths.DatabaseFile);
        }

        // Creates a table in the given SQLite file
        public static void CreateTables()
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = "CREATE TABLE users (" +
                                    "ID                 INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "user_name          TEXT, " +
                                    "password           TEXT, " +
                                    "email              TEXT); " +

                             "CREATE TABLE wva_orders (" +
                                    "ID                 INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "customer_id        TEXT, " +
                                    "order_name         TEXT, " +
                                    "created_date       TEXT, " +
                                    "wva_store_id       TEXT, " +
                                    "date_of_birth      TEXT, " +
                                    "name_1             TEXT, " +
                                    "name_2             TEXT, " +
                                    "street_addr_1      TEXT, " +
                                    "street_addr_2      TEXT, " +
                                    "city               TEXT, " +
                                    "state              TEXT, " +
                                    "zip                TEXT, " +
                                    "ship_to_account    TEXT, " +
                                    "office_name        TEXT, " +
                                    "ordered_by         TEXT, " +
                                    "po_number          TEXT, " +
                                    "shipping_method    TEXT, " +
                                    "ship_to_patient    TEXT, " +
                                    "freight            TEXT, " +
                                    "tax                TEXT, " +
                                    "discount           TEXT, " +
                                    "invoice_total      TEXT, " +
                                    "email              TEXT, " +
                                    "phone              TEXT, " +
                                    "status             TEXT);" +

                            "CREATE TABLE order_details (" +
                                   "ID                 INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                   "wva_order_id       INT, " +
                                   "lens_rx            INT, " +
                                   "first_name         TEXT, " +
                                   "last_name          TEXT, " +
                                   "eye                TEXT, " +
                                   "quantity           TEXT, " +
                                   "price              TEXT, " +
                                   "patient_id         TEXT, " +
                                   "name               TEXT, " +
                                   "product_reviewed   INT, " +
                                   "sku                TEXT, " +
                                   "product_key        TEXT, " +
                                   "upc                TEXT, " +
                                   "basecurve          TEXT, " +
                                   "diameter           TEXT, " +
                                   "sphere             TEXT, " +
                                   "cylinder           TEXT, " +
                                   "axis               TEXT, " +
                                   "ad                 TEXT, " +
                                   "color              TEXT, " +
                                   "multifocal         TEXT); ";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        public static void UpdateChangedColumns()
        {
            SQLiteConnection dbConnection = GetSQLiteConnection();

            try
            {
                dbConnection.Open();

                string sql =
                    "ALTER TABLE order_details " +
                    "ADD lens_rx INT;";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
            catch
            {
                // Duplicate table name, just ignore it
            }
            finally
            {
                dbConnection.Close();
            }

        }

        // Create a single user to user table
        public static User CreateUser(User user)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"INSERT into users (user_name, password, email) values ('{user.UserName}', '{user.Password}', '{user.Email}')";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                user.Status = "OK";

                return user;
            }
            catch (Exception x)
            {
                user.Status = "FAIL";
                user.Message = $"{x.Message}";
                Error.ReportOrLog(x);
                return user;
            }
        }

        // If you want to mark an order as 'submitted', just pass in a 'true' boolean. If you don't the order will be marked as 'open'
        public static bool CreateOrder(Order order, bool submit = false)
        {
            try
            {
                // If true, mark this order as 'submitted'
                string submitStatus = submit ? "submitted" : "open";
                string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

                // <1>
                // Create the order
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string insertWvaOrder = "INSERT OR IGNORE into wva_orders (" +
                                            "customer_id, " +
                                            "order_name, " +
                                            "created_date, " +
                                            "wva_store_id," +
                                            "date_of_birth, " +
                                            "name_1, " +
                                            "name_2, " +
                                            "street_addr_1, " +
                                            "street_addr_2, " +
                                            "city, " +
                                            "state, " +
                                            "zip, " +
                                            "ordered_by, " +
                                            "po_number, " +
                                            "shipping_method, " +
                                            "ship_to_patient, " +
                                            "phone, " +
                                            "email, " +
                                            "status) " +
                                            "values (" +
                                                $"'{order.CustomerID}', " +
                                                $"'{order.OrderName}', " +
                                                $"'{createdDate}', " +
                                                $"'{order.WvaStoreID}', " +
                                                $"'{order.DoB}', " +
                                                $"'{order.Name_1}', " +
                                                $"'{order.Name_2}', " +
                                                $"'{order.StreetAddr_1}', " +
                                                $"'{order.StreetAddr_2}', " +
                                                $"'{order.City}', " +
                                                $"'{order.State}', " +
                                                $"'{order.Zip}', " +
                                                $"'{order.OrderedBy}', " +
                                                $"'{order.PoNumber}', " +
                                                $"'{order.ShippingMethod}', " +
                                                $"'{order.ShipToPatient}', " +
                                                $"'{order.Phone}', " +
                                                $"'{order.Email}', " +
                                                $"'{submitStatus}'" +
                                                ")";

                SQLiteCommand command_1 = new SQLiteCommand(insertWvaOrder, dbConnection);
                command_1.ExecuteNonQuery();

                // <2>
                // Get this order's order ID that will link 'order_details' table to 'wva_order'
                string findID = $"SELECT ID FROM wva_orders WHERE order_name = '{order.OrderName}'";

                SQLiteCommand command_2 = new SQLiteCommand(findID, dbConnection);
                SQLiteDataReader reader = command_2.ExecuteReader();

                Int64 orderID = 0;
                while (reader.Read())
                {
                    orderID = (Int64)reader["ID"];
                }

                // <3>
                // Create the order_details objects associated with the order
                foreach (Item item in order.Items)
                {
                    string insertOrderDetail = "INSERT into order_details (" +
                                           "wva_order_id, " +
                                           "lens_rx, " +
                                           "first_name, " +
                                           "last_name, " +
                                           "eye, " +
                                           "quantity, " +
                                           "price, " +
                                           "patient_id, " +
                                           "name, " +
                                           "product_reviewed, " +
                                           "sku, " +
                                           "product_key, " +
                                           "upc, " +
                                           "basecurve, " +
                                           "diameter, " +
                                           "sphere, " +
                                           "cylinder, " +
                                           "axis, " +
                                           "ad, " +
                                           "color, " +
                                           "multifocal) " +
                                           "values (" +
                                               $"'{orderID}', " +
                                               $"'{item.OrderDetail.LensRx}', " +
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
                                               $"'{item.OrderDetail.Multifocal}'" +
                                               ")";

                    SQLiteCommand command_3 = new SQLiteCommand(insertOrderDetail, dbConnection);
                    command_3.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // =========================================================================================================
        // READ
        // =========================================================================================================

        // Read from database file
        public static List<User> GetAllUsers()
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = "select * from users";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                List<User> listUsers = new List<User>();

                while (reader.Read())
                {
                    listUsers.Add(new User()
                    {
                        UserName = (string)reader["user_name"],
                        Password = (string)reader["password"],
                        Email = (string)reader["email"],
                    });
                }

                return listUsers;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Get the order names of an account
        public static Dictionary<string, string> GetOrderNames(string actNum)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"select order_name, ID from wva_orders where customer_id = '{actNum}' AND status = 'open'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                Dictionary<string, string> dictNames = new Dictionary<string, string>();

                while (reader.Read())
                {
                    string orderName = (string)reader["order_name"];
                    Int64 orderID = (Int64)reader["ID"];

                    dictNames.Add(orderID.ToString(), orderName);
                }

                return dictNames;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Get an accounts orders
        public static List<Order> GetWVAOrders(string actNum)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string strGetOrders = $"Select * FROM wva_orders WHERE customer_id = '{actNum}'";

                SQLiteCommand command_1 = new SQLiteCommand(strGetOrders, dbConnection);
                SQLiteDataReader reader = command_1.ExecuteReader();

                List<Order> listOrders = new List<Order>();

                while (reader.Read())
                {
                    listOrders.Add(new Order()
                    {
                        ID = ((Int64)reader["ID"]).ToString(),
                        CustomerID = (string)reader["customer_id"],
                        OrderName = (string)reader["order_name"],
                        CreatedDate = (string)reader["created_date"],
                        WvaStoreID = (string)reader["wva_store_id"],
                        DoB = (string)reader["date_of_birth"],
                        Name_1 = (string)reader["name_1"],
                        Name_2 = (string)reader["name_2"],
                        StreetAddr_1 = (string)reader["street_addr_1"],
                        StreetAddr_2 = (string)reader["street_addr_2"],
                        City = (string)reader["city"],
                        State = (string)reader["state"],
                        Zip = (string)reader["zip"],
                        OrderedBy = (string)reader["ordered_by"],
                        PoNumber = (string)reader["po_number"],
                        ShippingMethod = (string)reader["shipping_method"],
                        ShipToPatient = (string)reader["ship_to_patient"],
                        Phone = (string)reader["phone"],
                        Email = (string)reader["email"],
                        Status = (string)reader["status"]
                    });
                }

                foreach (Order order in listOrders)
                {
                    string strGetOrderDetail = $"Select * FROM order_details WHERE wva_order_id = '{order.ID}'";

                    SQLiteCommand command_2 = new SQLiteCommand(strGetOrderDetail, dbConnection);
                    SQLiteDataReader reader_2 = command_2.ExecuteReader();

                    List<Item> listItems = new List<Item>();

                    while (reader_2.Read())
                    {
                        Item item = new Item()
                        {
                            ID = ((Int64)reader_2["ID"]).ToString(),
                            FirstName = (string)reader_2["first_name"],
                            LastName = (string)reader_2["last_name"],
                            PatientID = (string)reader_2["patient_id"],
                            Eye = (string)reader_2["eye"],
                            Quantity = (string)reader_2["quantity"],
                            ItemRetailPrice = (string)reader_2["price"],
                            OrderDetail = new OrderDetail()
                            {
                                Name = (string)reader_2["name"],
                                ProductReviewed = (int)reader_2["product_reviewed"] == 1 ? true : false,
                                SKU = (string)reader_2["sku"],
                                ProductKey = (string)reader_2["product_key"],
                                UPC = (string)reader_2["upc"],
                                Basecurve = (string)reader_2["basecurve"],
                                Diameter = (string)reader_2["diameter"],
                                Sphere = (string)reader_2["sphere"],
                                Cylinder = (string)reader_2["cylinder"],
                                Axis = (string)reader_2["axis"],
                                Add = (string)reader_2["ad"],
                                Color = (string)reader_2["color"],
                                Multifocal = (string)reader_2["multifocal"],
                            },
                        };

                        listItems.Add(item);
                    }

                    order.Items = listItems;
                }

                return listOrders;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }


        // Get email from username
        public static string GetEmail(string userName)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"SELECT email FROM users WHERE user_name = '{userName}'";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                string email = null;
                while (reader.Read())
                {
                    email = (string)reader["email"];
                }

                return email;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Check if order exits. If it does, return it
        public static Order CheckIfOrderExists(string orderName)
        {
            try
            {
                Order order = new Order();

                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                // Get the main order object from 'wva_orders' table
                string getOrder = $"SELECT * FROM wva_orders WHERE order_name = '{orderName}' AND status = 'open' LIMIT 1";
                SQLiteCommand command_1 = new SQLiteCommand(getOrder, dbConnection);
                SQLiteDataReader reader_1 = command_1.ExecuteReader();

                while (reader_1.Read())
                {
                    order.ID = ((Int64)reader_1["ID"]).ToString();
                    order.CustomerID = (string)reader_1["customer_id"];
                    order.OrderName = (string)reader_1["order_name"];
                    order.CreatedDate = (string)reader_1["created_date"];
                    order.WvaStoreID = (string)reader_1["wva_store_id"];
                    order.DoB = (string)reader_1["date_of_birth"];
                    order.Name_1 = (string)reader_1["name_1"];
                    order.Name_2 = (string)reader_1["name_2"];
                    order.StreetAddr_1 = (string)reader_1["street_addr_1"];
                    order.StreetAddr_2 = (string)reader_1["street_addr_2"];
                    order.City = (string)reader_1["city"];
                    order.State = (string)reader_1["state"];
                    order.Zip = (string)reader_1["zip"];
                    order.OrderedBy = (string)reader_1["ordered_by"];
                    order.PoNumber = (string)reader_1["po_number"];
                    order.ShippingMethod = (string)reader_1["shipping_method"];
                    order.ShipToPatient = (string)reader_1["ship_to_patient"];
                    order.Phone = (string)reader_1["phone"];
                    order.Email = (string)reader_1["email"];
                    order.Status = (string)reader_1["status"];
                }

                // Get the order items from 'order_details' table
                string getOrderItems = $"SELECT * FROM order_details WHERE wva_order_id = '{order.ID}'";
                SQLiteCommand command_2 = new SQLiteCommand(getOrderItems, dbConnection);
                SQLiteDataReader reader_2 = command_2.ExecuteReader();

                List<Item> items = new List<Item>();

                while (reader_2.Read())
                {
                    Item item = new Item()
                    {
                        ID = ((Int64)reader_2["ID"]).ToString(),
                        FirstName = (string)reader_2["first_name"],
                        LastName = (string)reader_2["last_name"],
                        PatientID = (string)reader_2["patient_id"],
                        Eye = (string)reader_2["eye"],
                        Quantity = (string)reader_2["quantity"],
                        ItemRetailPrice = (string)reader_2["price"],
                        OrderDetail = new OrderDetail()
                        {
                            Name = (string)reader_2["name"],
                            ProductReviewed = (Int32)reader_2["product_reviewed"] == 1 ? true : false,
                            SKU = (string)reader_2["sku"],
                            ProductKey = (string)reader_2["product_key"],
                            UPC = (string)reader_2["upc"],
                            Basecurve = (string)reader_2["basecurve"],
                            Diameter = (string)reader_2["diameter"],
                            Sphere = (string)reader_2["sphere"],
                            Cylinder = (string)reader_2["cylinder"],
                            Axis = (string)reader_2["axis"],
                            Add = (string)reader_2["ad"],
                            Color = (string)reader_2["color"],
                            Multifocal = (string)reader_2["multifocal"],
                        }
                    };
                    items.Add(item);
                }

                // Add items to the list
                order.Items = items;

                if (order.ID == null)
                    return null;
                else
                    return order;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Check if email exits
        public static bool CheckIfEmailExists(string email)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"SELECT * FROM users WHERE email='{email}'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                // Read response of executed command
                while (reader.Read())
                {
                    // If account number in database matches account number parameter, return true;
                    if ((string)reader["email"] == email)
                    {
                        return true;
                    }
                }
                // If no account numbers match, return false
                return false;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // Check if email exits
        public static bool CheckIfUserNameExists(string userName)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"SELECT * FROM users WHERE user_name = '{userName}'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                // Read response of executed command
                while (reader.Read())
                {
                    // If account number in database matches account number parameter, return true;
                    if ((string)reader["user_name"] == userName)
                    {
                        return true;
                    }
                }
                // If no account numbers match, return false
                return false;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // Check a user's login credentials
        public static User CheckCredentials(User user)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"SELECT user_name, password, email FROM users WHERE user_name='{user.UserName}' AND password='{user.Password}'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                User responseUser = new User()
                {
                    UserName = "FAIL",
                    Password = "FAIL",
                    Email = null,
                };

                while (reader.Read())
                {
                    responseUser.UserName = (string)reader["user_name"];
                    responseUser.Password = (string)reader["password"];
                    responseUser.Email = (string)reader["email"];
                };

                if (responseUser.UserName == "FAIL" || responseUser.Password == "FAIL")
                {
                    responseUser.Status = "FAIL";
                    responseUser.Message = "Invalid Username/Password";
                }
                else
                {
                    responseUser.Status = "OK";
                }

                return responseUser;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        public static List<string> GetLensRxByWvaOrderId(string wvaOrderId)
        {
            List<string> lensrxes = new List<string>();
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string getLensRxes = $"SELECT DISTINCT(lens_rx) FROM order_details JOIN wva_orders ON order_details.wva_order_id = wva_orders.ID WHERE wva_store_id = '{wvaOrderId}'";

                SQLiteCommand command = new SQLiteCommand(getLensRxes, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lensrxes.Add(reader[0].ToString());
                }

                return lensrxes;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // =========================================================================================================
        // UPDATE
        // =========================================================================================================

        public static bool ChangePassword(string userName, string password)
        {
            try
            {
                // Update order status to 'submitted'
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string updateOrder = $"UPDATE users " +
                                        $"SET " +
                                            $"password           =   '{password}' " +
                                        $"WHERE user_name     =   '{userName}'";

                SQLiteCommand command_1 = new SQLiteCommand(updateOrder, dbConnection);
                command_1.ExecuteNonQuery();

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        public static bool SubmitOrder(Order order)
        {
            try
            {
                Order checkOrder = CheckIfOrderExists(order.OrderName);

                if (checkOrder == null)
                {
                    order.Status = "submitted";
                    if (!CreateOrder(order, true))
                        return false;
                    else
                        return true;
                }
                else
                {
                    // Update order status to 'submitted'
                    SQLiteConnection dbConnection = GetSQLiteConnection();
                    dbConnection.Open();

                    string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

                    string updateOrder = $"UPDATE wva_orders " +
                                            $"SET " +
                                                $"status           =   'submitted', " +
                                                $"created_date     =   '{createdDate}', " +
                                                $"wva_store_id     =   '{order.WvaStoreID}' " +
                                            $"WHERE order_name     =   '{order.OrderName}'";

                    SQLiteCommand command_1 = new SQLiteCommand(updateOrder, dbConnection);
                    command_1.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        public static bool UnsubmitOrder(string orderName)
        {
            try
            {
                // Update order status to 'open'
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string updateOrder = $"UPDATE wva_orders " +
                                        $"SET " +
                                            $"status           =   'open' " +
                                        $"WHERE order_name     =   '{orderName}'";

                SQLiteCommand command_1 = new SQLiteCommand(updateOrder, dbConnection);
                command_1.ExecuteNonQuery();

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }


        public static bool SaveOrder(Order order)
        {
            try
            {
                // Check if order exits. If not, create order, else save order
                Order checkOrder = CheckIfOrderExists(order.OrderName);

                if (checkOrder == null)
                {
                    bool orderCreated = CreateOrder(order);

                    if (orderCreated)
                        return true;
                    else
                        return false;
                }

                // Save order
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string updateOrder = $"UPDATE wva_orders " +
                                        $"SET " +
                                            $"wva_store_id     =   '{checkOrder.WvaStoreID}', " +
                                            $"created_date     =   '{checkOrder.CreatedDate}', " +
                                            $"date_of_birth    =   '{order.DoB}', " +
                                            $"name_1           =   '{order.Name_1}', " +
                                            $"name_2           =   '{order.Name_2}', " +
                                            $"street_addr_1    =   '{order.StreetAddr_1}', " +
                                            $"street_addr_2    =   '{order.StreetAddr_2}', " +
                                            $"city             =   '{order.City}', " +
                                            $"state            =   '{order.State}', " +
                                            $"zip              =   '{order.Zip}', " +
                                            $"ordered_by       =   '{order.OrderedBy}', " +
                                            $"po_number        =   '{order.PoNumber}', " +
                                            $"shipping_method  =   '{order.ShippingMethod}', " +
                                            $"ship_to_patient  =   '{order.ShipToPatient}', " +
                                            $"phone            =   '{order.Phone}', " +
                                            $"email            =   '{order.Email}', " +
                                            $"status           =   'open' " +
                                        $"WHERE order_name     =   '{order.OrderName}'";

                SQLiteCommand command_1 = new SQLiteCommand(updateOrder, dbConnection);
                command_1.ExecuteNonQuery();


                // Delete old order details
                string deleteOrderDetail = $"DELETE FROM order_details WHERE wva_order_id = '{checkOrder.ID}'";

                SQLiteCommand command_2 = new SQLiteCommand(deleteOrderDetail, dbConnection);
                command_2.ExecuteNonQuery();

                // Add new order details
                foreach (Item item in order.Items)
                {
                    string insertOrderDetail = "INSERT into order_details (" +
                                                   "wva_order_id, " +
                                                   "first_name, " +
                                                   "last_name, " +
                                                   "eye, " +
                                                   "quantity, " +
                                                   "price, " +
                                                   "patient_id, " +
                                                   "name, " +
                                                   "product_reviewed, " +
                                                   "sku, " +
                                                   "product_key, " +
                                                   "upc, " +
                                                   "basecurve, " +
                                                   "diameter, " +
                                                   "sphere, " +
                                                   "cylinder, " +
                                                   "axis, " +
                                                   "ad, " +
                                                   "color, " +
                                                   "multifocal) " +
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
                                                       $"'{item.OrderDetail.Multifocal}'" +
                                                       ")";

                    SQLiteCommand command_3 = new SQLiteCommand(insertOrderDetail, dbConnection);
                    command_3.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }


        // =========================================================================================================
        // DELETE
        // =========================================================================================================

        // Deletes a table in the given SQLite file 
        public static void DeleteTable(string tableName)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"DROP TABLE {tableName}";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Delete a single user from user table
        public static void DeleteUser(int id, string userName)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string sql = $"DELETE FROM users WHERE id='{id}' AND user_name='{userName}'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Delete a single user from user table
        public static bool DeleteOrder(string orderName)
        {
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string getId = $"SELECT ID FROM wva_orders WHERE order_name = '{orderName}'";
                SQLiteCommand command_1 = new SQLiteCommand(getId, dbConnection);
                SQLiteDataReader reader_1 = command_1.ExecuteReader();

                Int64 orderID = 0;
                while (reader_1.Read())
                {
                    orderID = (Int64)reader_1["ID"];
                }

                string deleteOrder = $"DELETE FROM wva_orders WHERE order_name = '{orderName}'";
                SQLiteCommand command_2 = new SQLiteCommand(deleteOrder, dbConnection);
                command_2.ExecuteNonQuery();

                string deleteItemDetails = $"DELETE FROM order_details WHERE wva_order_id = '{orderID}'";
                SQLiteCommand command_3 = new SQLiteCommand(deleteItemDetails, dbConnection);
                command_3.ExecuteNonQuery();

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // =========================================================================================================
        // SUPPORTING METHODS
        // =========================================================================================================

        // Get the database connection
        private static SQLiteConnection GetSQLiteConnection()
        {
            try
            {
                SQLiteConnection dbConnection = new SQLiteConnection($"Data Source={Paths.DatabaseFile};Version=3;");

                return dbConnection;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }
    }
}
