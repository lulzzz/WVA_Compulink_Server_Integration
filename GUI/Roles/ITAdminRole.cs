using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    public class ITAdminRole : Role
    {
        public ITAdminRole(int roleId, string userName) : base(roleId, userName)
        {
            RoleId = roleId;
            UserName = userName;
            SetPermissions();
        }

        private void SetPermissions()
        {
            CanViewOrders = false;
            CanViewUsers = true;
        }
    }
}
