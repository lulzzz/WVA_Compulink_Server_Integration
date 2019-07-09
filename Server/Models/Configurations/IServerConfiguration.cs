using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Configurations
{
    interface IServerConfiguration
    {
        string Dsn { get; set; }
        string ApiKey { get; set; }
    }
}
