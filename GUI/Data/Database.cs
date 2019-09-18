using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Security;

namespace WVA_Connect_CSI.Data
{
    public class Database
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
                dataAccessor.AddChangePasswordColumn();
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
                dataAccessor.CreateUser(user.UserName, user.Password, user.Email, user.RoleId, user.RequiresPasswordChange);
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

        public bool EmailExists(string email)
        {
            if (dataAccessor.GetEmail(email) == null)
                return false;
            else
                return true;
        }

        public bool CreateUser(string username, string password, string email, int roleId, int requiresPasswordChange)
        {
            try
            {
                dataAccessor.CreateUser(username, password, email, roleId, requiresPasswordChange);
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

        public List<User> GetUsers()
        {
            try
            {
                var users = dataAccessor.GetUsers();

                // Filter out all the SuperAdmins 
                users.RemoveAll(x => x.RoleId == 3);

                return users;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        //
        // Orders
        //

        public List<Order> GetAllOrders()
        {
            try
            {
                return DecryptOrders(dataAccessor.GetAllOrders());
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
                return DecryptOrders(dataAccessor.GetSubmittedOrders());
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
                return DecryptOrders(dataAccessor.GetUnsubmittedOrders());
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        //
        // OrderDetails
        //

        public List<ItemDetail> GetItemDetail(int orderId)
        {
            try
            {
                return DecryptItemDetails(dataAccessor.GetItemDetail(orderId));
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        //
        // Order decryption
        //

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
