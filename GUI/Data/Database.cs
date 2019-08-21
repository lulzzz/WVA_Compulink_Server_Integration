using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models;

namespace WVA_Connect_CSI.Data
{
    class Database
    {
        SqliteDataAccessor dataAccessor;

        public Database()
        {
            dataAccessor = new SqliteDataAccessor();
        }

        //
        // User Roles
        //

        public void SetUpRoles()
        {
            try
            {
                dataAccessor.CreateRolesTable();
                dataAccessor.AddRoles();
                dataAccessor.AddRoleIdColumn();
                dataAccessor.CreateSuperUser();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public int GetUserRole(string username, string password)
        {
            return dataAccessor.GetRoleFromCredentials(username, password);
        }

        //
        // Users
        //

        public void CreateUser(User user)
        {
            try
            {
                dataAccessor.CreateUser(user.UserName, user.Password, user.Email, user.RoleId);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public bool UserNameExists(string username)
        {
            if (dataAccessor.GetUserName(username) == null)
                return false;
            else
                return true;
        }

        public bool CreateUser(string username, string password, string email, int roleId)
        {
            try
            {
                dataAccessor.CreateUser(username, password, email, roleId);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public bool DeleteUser(string username)
        {
            try
            {
                dataAccessor.DeleteUser(username);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        //
        // Orders
        //

        public List<Order> GetAllOrders()
        {
            try
            {
                return dataAccessor.GetAllOrders();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        public List<Order> GetSubmittedOrders()
        {
            try
            { 
                return dataAccessor.GetSubmittedOrders();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        public List<Order> GetUnsubmittedOrders()
        {
            try
            {
                return dataAccessor.GetUnsubmittedOrders();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

    }
}
