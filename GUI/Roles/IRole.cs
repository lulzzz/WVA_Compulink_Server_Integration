using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    interface IRole
    {
        string Name { get; set; }
        int Value { get; set; }

        Role DetermineRole();
    }
}
