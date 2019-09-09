using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Orders;
using WVA_Connect_CSI.Models.Users;
using WVA_Connect_CSI.Utilities.Files;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Dapper;

namespace WVA_Connect_CSI.Data
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
                string status = submit ? "submitted" : "open";
                string createdDate = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");

                DataAccessor.CreateOrder(order, createdDate, status);

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

        public bool OrderExists(string orderName)
        {
            try
            {
                var order = DataAccessor.OrderExists(orderName);

                if (order == null)
                    return false;
                else
                    return true;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return false;
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

                if (responseUser == null)
                {
                    return new User()
                    {
                        UserName = "FAIL",
                        Password = "FAIL",
                        Status = "FAIL",
                        Message = "Invalid Username/Password"
                    };
                }
                if (responseUser.RoleId == 2)
                {
                    responseUser.Status = "FAIL";
                    responseUser.Message = "Access Not Authorized!";
                }
                else if (responseUser.UserName == "FAIL" || responseUser.Password == "FAIL")
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

        public List<string> GetLensRxByWvaOrderId(string wvaStoreId)
        {
            List<string> lensrxes = new List<string>();

            SQLiteConnection dbConnection = GetSQLiteConnection();
            dbConnection.Open();

            try
            {
                //int id;
                //using (IDbConnection cnn = new SQLiteConnection($"Data Source={Paths.DatabaseFile};Version=3;"))
                //{
                //    id = cnn.Query<int>($"SELECT Id FROM WvaOrders WHERE WvaStoreId = '{wvaStoreId}'").FirstOrDefault();
                //    lensrxes.AddRange(cnn.Query<string>($"SELECT LensRx FROM OrderDetails WHERE WvaOrderId = '{id}'"));
                //}

                string getLensRxes = $"SELECT DISTINCT(LensRx) FROM OrderDetails JOIN WvaOrders ON OrderDetails.WvaOrderId = WvaOrders.ID WHERE WvaStoreId = '{wvaStoreId}'";

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
            finally
            {
                dbConnection.Close();
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
                if (OrderExists(order.OrderName))
                    return SaveOrder(order, submit:true) ? true : false;
                else
                    return CreateOrder(order, submit: true) ? true : false;

                //order.Status = "submitted";

                //Order checkOrder = CheckIfOrderExists(order.OrderName);

                //if (checkOrder == null)
                //{
                //    if (!CreateOrder(order, true))
                //        return false;
                //    else
                //        return true;
                //}
                //else
                //{
                //    return true;
                //}
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


        public bool SaveOrder(Order order, bool submit = false)
        {
            try
            {
                // Update the order status to follow the 'submit' action
                order.Status = submit ? "submitted" : "open";

                // Check if order exits. If not, create order, else save order
                if (CheckIfOrderExists(order.OrderName) == null ? true : false)
                {
                    return CreateOrder(order, submit);
                }
                else
                {
                    DataAccessor.SaveOrder(order);
                    return true; // returns true as long as save order doesn't throw an error
                }
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
