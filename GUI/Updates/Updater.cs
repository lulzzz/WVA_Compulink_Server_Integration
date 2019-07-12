using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Utility.Files;
using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace WVA_Compulink_Server_Integration.Updates
{
    class Updater
    {
        public static async Task RunUpdates()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Server_Integration").Result)
                {
                    var updateInfo = mgr.CheckForUpdate().Result;
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        await mgr.UpdateApp();
                        MessageBox.Show("An update for your application is available. Please stop the server, then close and reopen the application.", "Update Available");
                    }
                }
            }
            catch (Exception e)
            {
                Error.ReportOrLog(e);
            }
        }

        // This may be usefull in the future
        //private static string GetServerDirectoryName()
        //{
        //    string path = Paths.AppDataLocal + $@"\{Assembly.GetCallingAssembly().GetName().Name}\";
        //    string[] dirs = Directory.GetDirectories(path, "app-*");

        //    if (dirs.Length > 1)
        //        return dirs[dirs.Length - 1] + @"\Server\"; // Returns the last item in the array (highest app version)
        //    else if (dirs.Length == 1)
        //        return dirs[0];
        //    else
        //        return null;
        //}
    }
}
