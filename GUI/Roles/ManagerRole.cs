using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    public class ManagerRole : Role
    {
        public ManagerRole(int roleId, string userName) : base(roleId, userName)
        {
            RoleId = roleId;
            UserName = userName;
            SetPermissions();
        }

        private void SetPermissions()
        {
            CanViewOrders = true;
            CanViewUsers = false;
        }

    }
}
