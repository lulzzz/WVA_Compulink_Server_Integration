using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Responses
{
    interface IResponse
    {
        string Status { get; set; }
        string Message { get; set; }
    }
}
