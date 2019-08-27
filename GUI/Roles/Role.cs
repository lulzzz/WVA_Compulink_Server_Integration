using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    public class Role : IRole
    {
        public int RoleId { get; set; }
        public string UserName { get; set; }

        public bool CanViewOrders { get; set; }
        public bool CanViewUsers { get; set; }

        public Role(int roleId, string userName)
        { 
            RoleId = roleId;
            UserName = userName;
        }

        public Role DetermineRole()
        {
            if (RoleId == 3)
                return new SuperAdminRole(RoleId, UserName);
            else if (RoleId == 2)
                return new ITAdminRole(RoleId, UserName);
            else if (RoleId == 1)
                return new ManagerRole(RoleId, UserName);
            else 
                return new UserRole(RoleId, UserName);
        }
    }
}
