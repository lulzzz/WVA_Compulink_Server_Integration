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
using WVA_Connect_CSI.Utility.Files;

namespace WVA_Connect_CSI.Data
{
    class SqliteDataAccessor
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
        // UsersView
        //

        public void CreateUser(string username, string password, string email, int roleId)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"INSERT INTO Users (UserName, " +
                                                $"Password, " +
                                                $"Email, " +
                                                $"RoleId) " +
                                        $"VALUES ('{username}', " +
                                                $" '{password}', " +
                                                $" '{email}', " +
                                                $" '{roleId}')");
            }
        }

        public string GetUserName(string username)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                return cnn.Query<string>($"SELECT UserName FROM Users WHERE UserName='{username}'").FirstOrDefault();
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
        // Get database connection string
        //

        private string GetDbConnectionString()
        {
            return $"Data Source={Paths.DatabaseFile};Version=3;";
        }

    }
}
