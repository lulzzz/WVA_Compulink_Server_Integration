using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    class Role : IRole
    {
        public string Name { get; set; }
        public int Value { get; set; }

        protected bool CanViewOrders { get; set; }
        protected bool CanViewUsers { get; set; }

        public Role(string name, int value)
        {

        }

        public Role DetermineRole()
        {
            if (Value == 3)
                return new SuperAdminRole(Name, Value);
            else if (Value == 2)
                return new ITAdminRole(Name, Value);
            else if (Value == 1)
                return new ManagerRole(Name, Value);
            else 
                return new UserRole(Name, Value);
        }
    }
}
