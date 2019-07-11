using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Utilities.Files
{
    public class Paths
    {
        /* ---------------------------------------- ROOT PATHS --------------------------------------------------------- */

        public static readonly string AppDataLocal          = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string AppName              = Assembly.GetCallingAssembly().GetName().Name.ToString();


        /* ---------------------------------------- APP DATA --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string DataDir               = $@"{AppDataLocal}\{AppName}\Data\";
        public static readonly string ApiKeyDir             = $@"{AppDataLocal}\{AppName}\Data\ApiKey\";
        public static readonly string ErrorLogDir           = $@"{AppDataLocal}\{AppName}\ErrorLog\";

        // FILES  
        public static readonly string AppExecFile           = $@"{AppDataLocal}\{AppName}\{AppName}GUI.exe";
        public static readonly string DatabaseFile          = $@"{AppDataLocal}\{AppName}\Data\SQLite_Database.sqlite";
        public static readonly string ApiKeyFile            = $@"{AppDataLocal}\{AppName}\ApiKey\ApiKey.txt";
        public static readonly string WvaConfigFile         = $@"{AppDataLocal}\{AppName}\Data\wvaConfig.json";


        /* -------------------------------------------- WEB PATHS --------------------------------------------------------- */

        public static readonly string WisVisBase            = "https://orders.wisvis.com";
        public static readonly string WisVisOrders          = $@"{WisVisBase}/orders";
        public static readonly string WisVisProducts        = $@"{WisVisBase}/products";
        public static readonly string WisVisEmailReset      = $@"{WisVisBase}/mailers/reset";
        public static readonly string WisVisEmailResetCheck = $@"{WisVisBase}/mailers/reset_check";
        public static readonly string WisVisErrors          = $@"https://ws2.wisvis.com/aws/scanner/error_handler.rb";
    }
}
