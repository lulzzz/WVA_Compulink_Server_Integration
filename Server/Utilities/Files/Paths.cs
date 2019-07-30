using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Utilities.Files
{
    public class Paths
    {
        /* ---------------------------------------- ROOT PATHS --------------------------------------------------------- */

        private static readonly string AppName              = Assembly.GetCallingAssembly().GetName().Name.ToString();
        private static readonly string PublicDocs           = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);


        /* ---------------------------------------- APP DATA --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string DataDir               = $@"{PublicDocs}\{AppName}\Data\";
        public static readonly string ErrorLogDir           = $@"{PublicDocs}\{AppName}\ErrorLog\";

        // FILES  
        public static readonly string DatabaseFile          = $@"{PublicDocs}\{AppName}\Data\SQLite_Database.sqlite";
        public static readonly string WvaConfigFile         = $@"{PublicDocs}\{AppName}\Config\wvaConfig.json";


        /* -------------------------------------------- WEB PATHS --------------------------------------------------------- */

        public static readonly string WisVisBase            = "https://orders.wisvis.com";
        public static readonly string WisVisOrders          = $@"{WisVisBase}/orders";
        public static readonly string WisVisProducts        = $@"{WisVisBase}/products";
        public static readonly string WisVisEmailReset      = $@"{WisVisBase}/mailers/reset";
        public static readonly string WisVisEmailResetCheck = $@"{WisVisBase}/mailers/reset_check";
        public static readonly string WisVisErrors          = $@"https://ws2.wisvis.com/aws/scanner/error_handler.rb";
    }
}
