using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Users
{
    interface IUser
    {
        int ID { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Email { get; set; }
    }
}
