using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Models.Configurations;
using WVA_Connect_CSI.Utilities.Files;

namespace WVA_Connect_CSI.Memory
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
