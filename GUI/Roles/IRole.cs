using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    interface IRole
    {
        int RoleId { get; set; }

        Role DetermineRole();
    }
}
