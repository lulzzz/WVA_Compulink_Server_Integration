using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Errors
{
    public class JsonError
    {
        public string ActNum { get; set; }
        public string Error { get; set; }
        public string Application { get; set; }
        public string AppVersion { get; set; }
    }
}
