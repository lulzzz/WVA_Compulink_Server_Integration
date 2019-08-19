using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool IsGoodLogin(string username, string password)
        {
            return dataAccessor.LoginCredentialsFound(username, password);
        }

        public DatabaseRole GetUserRole(string username)
        {
            return dataAccessor.GetDatabaseRole(username);
        }

    }
}
