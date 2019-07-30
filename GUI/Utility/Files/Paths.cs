using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Utility.Files
{
    class Paths
    {
        /* ---------------------------------------- ROOT PATHS --------------------------------------------------------- */

        public static readonly string AppDataLocal              = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string AppName                  = Assembly.GetCallingAssembly().GetName().Name.ToString();
        private static readonly string Desktop                  = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string Documents                = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string PublicDocs               = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);

        /* ---------------------------------------- APP DATA --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string ConfigDir                 = $@"{PublicDocs}\{AppName}\Config\";
        public static readonly string DataDir                   = $@"{PublicDocs}\{AppName}\Data\";
        public static readonly string ErrorLogDir               = $@"{PublicDocs}\{AppName}\ErrorLog\";
        private static readonly string AppHighestVer            = GetServerDirectoryName();


        // FILES     
        public static readonly string DatabaseFile              = $@"{PublicDocs}\{AppName}\Data\SQLite_Database.sqlite";
        public static readonly string AppExecFile               = $@"{AppDataLocal}\{AppName}\{AppName}.exe";
        public static readonly string WvaConfigFile             = $@"{PublicDocs}\{AppName}\Config\wvaConfig.json";
        public static readonly string ConfigDesktop             = $@"{Desktop}\wvaConfig.json";
        public static readonly string ConfigDocuments           = $@"{Documents}\WVA Remote Help\Files\wvaConfig.json";
        public static readonly string ConfigInstallDir          = $@"{Desktop}\WVA Compulink Integration\Applications\Server App\wvaConfig.json";
        public static readonly string ServiceExePath            = $@"{AppHighestVer}\Server\{AppName}.exe";

        /* -------------------------------------------- WEB PATHS --------------------------------------------------------- */

        public static readonly string WisVisBase                = "https://orders.wisvis.com";
        public static readonly string WisVisOrders              = $@"{WisVisBase}/orders";
        public static readonly string WisVisErrors              = $@"https://ws2.wisvis.com/aws/scanner/error_handler.rb";


        private static string GetServerDirectoryName()
        {
            string path = Paths.AppDataLocal + $@"\{Assembly.GetCallingAssembly().GetName().Name}";
            string[] dirs = Directory.GetDirectories(path, "app-*");

            if (dirs.Length > 1)
                return dirs[dirs.Length - 1]; // Returns the last item in the array (highest app version)
            else if (dirs.Length == 1)
                return dirs[0];
            else
                return null;
        }

        //private static string GetServerDirectoryName()
        //{
        //    string path = Paths.AppDataLocal + $@"\{Assembly.GetCallingAssembly().GetName().Name}\";
        //    string[] dirs = Directory.GetDirectories(path, "app-*");

        //    return dirs[dirs.Length - 1] + @"\Server\"; // Returns the last item in the array (highest app version)
        //}
    }
}
