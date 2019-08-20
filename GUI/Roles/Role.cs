using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    class Role : IRole
    {
        public int RoleId { get; set; }

        public bool CanViewOrders { get; set; }
        public bool CanViewUsers { get; set; }

        public Role(int roleId)
        { 
            RoleId = roleId;
        }

        public Role DetermineRole()
        {
            if (RoleId == 3)
                return new SuperAdminRole(RoleId);
            else if (RoleId == 2)
                return new ITAdminRole(RoleId);
            else if (RoleId == 1)
                return new ManagerRole(RoleId);
            else 
                return new UserRole(RoleId);
        }
    }
}
