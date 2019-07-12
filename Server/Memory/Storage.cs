using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Models.Configurations;
using WVA_Compulink_Server_Integration.Utilities.Files;

namespace WVA_Compulink_Server_Integration.Memory
{
    public class Storage
    {
        public static WvaConfig Config { get; set; }

        public Storage()
        {
            Config = JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText($@"{Paths.WvaConfigFile}"));
        }

    }
}
