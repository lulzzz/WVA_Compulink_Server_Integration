﻿using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Utility.Files;
using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WVA_Compulink_Server_Integration.Services;
using System.Threading;

namespace WVA_Compulink_Server_Integration.Updates
{
    class Updater
    {
        public static async Task NotifyUpdatesAvailable()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Server_Integration").Result)
                {
                    var updateInfo = mgr.CheckForUpdate().Result;

                    if (updateInfo.ReleasesToApply.Any())
                    {
                        MessageBoxResult result = MessageBox.Show("An update for your application is available. Would you like to install it?", "Update Available", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                            ForceUpdate();
                    }
                }
            }
            catch (Exception e)
            {
                Error.ReportOrLog(e);
            }
        }

        private static async Task Update()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Server_Integration").Result)
                {
                    var updateInfo = mgr.CheckForUpdate().Result;

                    if (updateInfo.ReleasesToApply.Any())
                    {
                        await mgr.UpdateApp();
                    }
                }
            }
            catch (Exception e)
            {
                Error.ReportOrLog(e);
            }
        }

        public static bool UpdatesAvailable()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Server_Integration").Result)
            {
                var updateInfo = mgr.CheckForUpdate().Result;

                if (updateInfo.ReleasesToApply.Any())
                    return true;
                else
                    return false;
            }
        }

        public static async Task ForceUpdate()
        {
            ServiceHost.Stop();
            ServiceHost.Uninstall();
            await Task.Run(() => Update());
            Thread.Sleep(1000); // Wait a second for the for the update files to be placed before calling the new application
            ServiceHost.Install();
            ServiceHost.Start();
            RestartApplication();
        }

        private static void RestartApplication()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
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
