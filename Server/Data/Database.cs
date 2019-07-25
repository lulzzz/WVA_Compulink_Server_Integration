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

        private SqliteDataAccessor DataAccessor;

        public Database()
        {
            DataAccessor = new SqliteDataAccessor();
        }

        // Creates a SQLite database file
        public void CreateDatabaseFile()
        {
            if (!Directory.Exists(Paths.DataDir))
                Directory.CreateDirectory(Paths.DataDir);

            if (!File.Exists(Paths.DatabaseFile))
                SQLiteConnection.CreateFile(Paths.DatabaseFile);
        }

        // Creates a table in the given SQLite file
        public void CreateTables()
        {
            try
            {
                DataAccessor.CreateUsersTable();
                DataAccessor.CreateWvaOrdersTable();
                DataAccessor.CreateOrderDetailsTable();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Create a single user to user table
        public User CreateUser(User user)
        {
            try
            {
                DataAccessor.CreateUser(user);
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
        public bool CreateOrder(Order order, bool submit = false)
        {
            try
            {
                // If true, mark this order as 'submitted'
                string submitStatus = submit ? "submitted" : "open";
                string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

                DataAccessor.CreateOrder(order, createdDate, submitStatus);

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
        public List<User> GetAllUsers()
        {
            try
            {
                return DataAccessor.GetAllUsers();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Get the order names of an account
        public Dictionary<string, string> GetOrderNames(string actNum)
        {
            try
            {
                return DataAccessor.GetOrderNames(actNum);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Get an accounts orders
        public List<Order> GetWVAOrders(string actNum)
        {
            try
            {
                return DataAccessor.GetWvaOrders(actNum);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }


        // Get email from username
        public string GetEmail(string userName)
        {
            try
            {
                return DataAccessor.GetEmail(userName);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // Check if order exits. If it does, return it
        public Order CheckIfOrderExists(string orderName)
        {
            try
            {
                var order = DataAccessor.OrderExists(orderName);

                if (order?.ID == null)
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
        public bool CheckIfEmailExists(string email)
        {
            try
            {
                return DataAccessor.EmailExists(email);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // Check if email exits
        public bool CheckIfUserNameExists(string userName)
        {
            try
            {
                return DataAccessor.UsernameExists(userName);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        // Check a user's login credentials
        public User CheckCredentials(User user)
        {
            try
            {
                var responseUser = DataAccessor.CheckCredentials(user);

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

        public List<string> GetLensRxByWvaOrderId(string wvaOrderId)
        {
            List<string> lensrxes = new List<string>();
            try
            {
                SQLiteConnection dbConnection = GetSQLiteConnection();
                dbConnection.Open();

                string getLensRxes = $"SELECT DISTINCT(LensRx) FROM OrderDetails JOIN WvaOrders ON OrderDetails.WvaOrderId = WvaOrders.Id WHERE WvaStoreId = '{wvaOrderId}'";

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

        public bool ChangePassword(string userName, string password)
        {
            try
            {
                DataAccessor.ChangePassword(userName, password); 

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        public bool SubmitOrder(Order order)
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
                    return true;
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }

        public bool UnsubmitOrder(string orderName)
        {
            try
            {
                DataAccessor.UnsumbmitOrder(orderName);

                return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
            }
        }


        public bool SaveOrder(Order order)
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

                DataAccessor.SaveOrder(order, checkOrder);

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
        public  void DeleteTable(string tableName)
        {
            try
            {
                DataAccessor.DeleteTable(tableName);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Delete a single user from user table
        public void DeleteUser(int id, string userName)
        {
            try
            {
                DataAccessor.DeleteUser(id, userName);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Delete a single user from user table
        public bool DeleteOrder(string orderName)
        {
            try
            {
                DataAccessor.DeleteOrder(orderName);

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
