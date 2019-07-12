using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.ODBC;
using WVA_Compulink_Server_Integration.Utility.Files;

namespace WVA_Compulink_Server_Integration.Memory
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
