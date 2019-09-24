﻿using WVA_Connect_CSI.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;

namespace WVA_Connect_CSI.ODBC
{
    public class DsnConnectionTester
    {
        public static bool IsGoodConnection()
        {
            try
            {
                var config = JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText(Paths.WvaConfigFile));

                using (OdbcConnection conn = new OdbcConnection())
                {
                    conn.ConnectionString = $"dsn={config.Dsn}";
                    conn.Open();
                    conn.Close();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
