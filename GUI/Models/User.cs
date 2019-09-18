using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models
{
    public class User
    {
        public int ID { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Account { get; set; }
        public string ApiKey { get; set; }
        public string DSN { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int RequiresPasswordChange { get; set; }


        public string RoleName
        {
            get { return GetRoleName(RoleId); }
            set { RoleName = value; }
        }

        private string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 0:
                    return "User";
                case 1:
                    return "Manager";
                case 2:
                    return "IT Admin";
                case 3:
                    return "Super Admin";
                default:
                    return "User";
            }
        }

        public string Str_RequiresPasswordChange
        {
            get { return GetRequiresPasswordChangeString(RequiresPasswordChange); }
            set { Str_RequiresPasswordChange = value; }
        }

        private string GetRequiresPasswordChangeString(int requiresPassChange)
        {
            if (requiresPassChange == 1)
                return "Yes";
            else
                return "No";
        }

    }
}
