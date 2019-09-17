using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.Utility.Files;

namespace WVA_Connect_CSI.Data
{
    public class SqliteDataAccessor
    {

        //
        // Database setup
        //

        public void CreateRolesTable()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("CREATE TABLE IF NOT EXISTS Roles (" +
                                                        "Id                     INTEGER PRIMARY KEY, " +
                                                        "Role                   TEXT); ");
            }
        }

        public void AddRoles()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("INSERT OR IGNORE INTO Roles (Id, Role) VALUES ('0', 'USER');");
                cnn.Execute("INSERT OR IGNORE INTO Roles (Id, Role) VALUES ('1', 'MANAGER');");
                cnn.Execute("INSERT OR IGNORE INTO Roles (Id, Role) VALUES ('2', 'ITADMIN');");
                cnn.Execute("INSERT OR IGNORE INTO Roles (Id, Role) VALUES ('3', 'SUPERADMIN');");
            }
        }

        public void AddRoleIdColumn()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("ALTER TABLE Users ADD COLUMN RoleId INTEGER");
                }
            }
            catch
            {

            }
        }

        public void AddChangePasswordColumn()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("ALTER TABLE Users ADD COLUMN RequiresPasswordChange INT");
                }
            }
            catch
            {

            }
        }

        public void CreateSuperUser()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                string superuser = cnn.Query<string>($"SELECT UserName FROM Users WHERE UserName='stark' AND Password='462090fee12886df35b0dd449f4a996e24f288c6e4b2bade80c0fc7971c66631'").FirstOrDefault();

                if (superuser == null)
                {
                    using (IDbConnection cnn1 = new SQLiteConnection(GetDbConnectionString()))
                    {
                        cnn1.Execute("INSERT INTO Users (UserName, Password, Email, RoleId) VALUES ('stark','462090fee12886df35b0dd449f4a996e24f288c6e4b2bade80c0fc7971c66631', 'evan@wisvis.com', '3');");
                    }
                }
            }
        }

        //
        // LoginView
        //

        public int GetRoleFromCredentials(string username, string password)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<int>($"SELECT RoleId FROM Users WHERE UserName='{username}' AND Password='{password}'").FirstOrDefault();
            }
        }

        //
        // Users
        //

        public void CreateUser(string username, string password, string email, int roleId, int requiresPasswordChange)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"INSERT INTO Users (UserName, " +
                                                $"Password, " +
                                                $"Email, " +
                                                $"RoleId," +
                                                $"RequiresPasswordChange) " +
                                         $"VALUES ('{username}', " +
                                                $" '{password}', " +
                                                $" '{email}', " +
                                                $" '{roleId}', " +
                                                $" '{requiresPasswordChange}');");
            }
        }

        public string GetUserName(string username)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<string>($"SELECT UserName FROM Users WHERE UserName='{username}'").FirstOrDefault();
            }
        }

        public string GetEmail(string email)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<string>($"SELECT Email FROM Users WHERE Email='{email}'").FirstOrDefault();
            }
        }

        public void DeleteUser(string username)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"DELETE FROM Users WHERE UserName='{username}'");
            }
        }

        //
        // Orders
        //

        public List<Order> GetAllOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return DecryptOrders(cnn.Query<Order>($"SELECT * FROM WvaOrders").AsEnumerable().ToList());
            }
        }

        public List<Order> GetSubmittedOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return DecryptOrders(cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE Status='submitted'").AsEnumerable().ToList());
            }
        }

        public List<Order> GetUnsubmittedOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return DecryptOrders(cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE Status='open'").AsEnumerable().ToList());
            }
        }

        //
        // OrderDetails
        //

        public List<ItemDetail> GetItemDetail(int orderId)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return DecryptItemDetails(cnn.Query<ItemDetail>($"SELECT * FROM OrderDetails WHERE WvaOrderId='{orderId}'").AsEnumerable().ToList());
            }
        }

        //
        // Get database connection string
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }

        // Order decryption

        private List<Order> DecryptOrders(List<Order> orders)
        {
            for (int i = 0; i < orders.Count; i++)
                orders[i] = DecryptOrder(orders[i]);

            return orders;
        }

        private Order DecryptOrder(Order order)
        {
            order.Name1 = Crypto.Decrypt(order?.Name1);
            order.Name2 = Crypto.Decrypt(order?.Name2);
            order.StreetAddr1 = Crypto.Decrypt(order?.StreetAddr1);
            order.StreetAddr2 = Crypto.Decrypt(order?.StreetAddr2);
            order.City = Crypto.Decrypt(order?.City);
            order.State = Crypto.Decrypt(order?.State);
            order.Zip = Crypto.Decrypt(order?.Zip);
            order.OrderedBy = Crypto.Decrypt(order?.OrderedBy);
            order.PoNumber = Crypto.Decrypt(order?.PoNumber);
            order.ShippingMethod = Crypto.Decrypt(order?.ShippingMethod);
            order.Phone = Crypto.Decrypt(order?.Phone);
            order.Email = Crypto.Decrypt(order?.Email);

            return order;
        }

        private List<ItemDetail> DecryptItemDetails(List<ItemDetail> itemDetails)
        {
            for (int i = 0; i < itemDetails.Count; i++)
                itemDetails[i] = DecryptItemDetail(itemDetails[i]);

            return itemDetails;
        }

        private ItemDetail DecryptItemDetail(ItemDetail itemDetail)
        {
            itemDetail.FirstName = Crypto.Decrypt(itemDetail.FirstName);
            itemDetail.LastName = Crypto.Decrypt(itemDetail.LastName);
            itemDetail.PatientID = Crypto.Decrypt(itemDetail.PatientID);

            return itemDetail;
        }

    }
}
