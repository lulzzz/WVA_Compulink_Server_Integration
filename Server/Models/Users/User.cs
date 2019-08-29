﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Users
{
    public class User : IUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Account { get; set; }
        public string ApiKey { get; set; }
        public string DSN { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int RoleId { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public string[] AvailableActs { get; set; }
    }
}
