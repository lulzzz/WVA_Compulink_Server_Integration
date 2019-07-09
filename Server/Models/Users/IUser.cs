using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Users
{
    interface IUser
    {
        int ID { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Email { get; set; }
    }
}
