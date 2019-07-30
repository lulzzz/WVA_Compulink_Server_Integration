using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.ODBC;
using WVA_Connect_CSI.Utility.Files;

namespace WVA_Connect_CSI.Memory
{
    class Storage
    {
        public static WvaConfig Config { get; set; }

        public Storage()
        {
            Config = JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText($@"{Paths.WvaConfigFile}"));
        }
    }
}
