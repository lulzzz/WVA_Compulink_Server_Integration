using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Models;

namespace WVA_Connect_CSI.ViewModels
{
    public class LoginViewModel
    {
        private Database _Database;

        public LoginViewModel()
        {
            _Database = new Database();
        }

        public int GetLoginRole(string username, string password)
        {
            return _Database.GetUserRole(username, password);
        }

    }
}
