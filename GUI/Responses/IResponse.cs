using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Responses
{
    interface IResponse
    {
        string Status { get; set; }
        string Message { get; set; }
    }
}
