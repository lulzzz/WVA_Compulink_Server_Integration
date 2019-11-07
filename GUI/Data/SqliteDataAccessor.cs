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
        public void CreateUsersTable()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("CREATE TABLE IF NOT EXISTS Users (" +
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
                cnn.Execute("CREATE TABLE IF NOT EXISTS WvaOrders (" +
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
                cnn.Execute("CREATE TABLE IF NOT EXISTS OrderDetails (" +
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

        public List<User> GetUsers()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<User>($"SELECT UserName, Email, RoleId, RequiresPasswordChange FROM Users").AsEnumerable().ToList();
            }
        }

        //
        // Orders
        //

        public List<Order> GetAllOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<Order>($"SELECT * FROM WvaOrders").AsEnumerable().ToList();
            }
        }

        public List<Order> GetSubmittedOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE Status='submitted'").AsEnumerable().ToList();
            }
        }

        public List<Order> GetUnsubmittedOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<Order>($"SELECT * FROM WvaOrders WHERE Status='open'").AsEnumerable().ToList();
            }
        }

        //
        // OrderDetails
        //

        public List<ItemDetail> GetItemDetail(int orderId)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<ItemDetail>($"SELECT * FROM OrderDetails WHERE WvaOrderId='{orderId}'").AsEnumerable().ToList();
            }
        }

        //
        // Get database connection string
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }

    }
}
