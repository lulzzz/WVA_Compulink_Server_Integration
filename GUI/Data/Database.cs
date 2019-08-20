using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public void SetUpRoles()
        {
            dataAccessor.CreateRolesTable();
            dataAccessor.AddRoles();
            dataAccessor.AddRoleIdColumn();
            dataAccessor.CreateSuperUser();
        }

        public int GetUserRole(string username, string password)
        {
            return dataAccessor.GetRoleFromCredentials(username, password);
        }

    }
}
