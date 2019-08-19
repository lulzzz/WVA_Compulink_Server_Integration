﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Roles
{
    class UserRole : Role
    {
        public UserRole(string name, int value) : base(name, value)
        {
            Name = name;
            Value = value;
            SetPermissions();
        }

        private void SetPermissions()
        {
            CanViewOrders = false;
            CanViewUsers = false;
        }
    }
}
