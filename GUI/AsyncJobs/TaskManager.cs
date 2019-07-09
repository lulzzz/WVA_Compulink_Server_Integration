using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Updates;
using WVA_Compulink_Server_Integration.Utility.Files;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.AsyncJobs
{
    class TaskManager
    {
        private static DateTime LastTimeUpdated { get; set; }
        private static DateTime LastTimeCleaned { get; set; }

        private static readonly TimeSpan startWorkingHour = new TimeSpan(6, 0, 0);
        private static readonly TimeSpan endWorkingHour = new TimeSpan(21, 0, 0);

        public static void StartAllJobs()
        {
            Task RunJobLauncherTask = new Task(() => { RunJobLauncher(); });
            RunJobLauncherTask.Start();
        }

        private static void RunJobLauncher()
        {
            while (true)
            {
                DateTime now = DateTime.Now;

                if (now.TimeOfDay < startWorkingHour || now.TimeOfDay > endWorkingHour)
                {
                    // Clean out error logs 
                    if (LastTimeCleaned < DateTime.Now.AddDays(-1))
                    {
                        Task cleanErrLogTask = new Task(() => { CleanErrorDirectory(); });
                        cleanErrLogTask.Start();
                        LastTimeCleaned = now;
                    }

                    // Update the server app
                    if (LastTimeUpdated < DateTime.Now.AddDays(-1))
                    {
                        Task checkUpdatesTask = new Task(() => { CheckForServerUpdates(); });
                        checkUpdatesTask.Start();
                        checkUpdatesTask.Wait();
                        LastTimeUpdated = now;
                    }
                }

                Thread.Sleep(60000);
            }
        }

        private static void CleanErrorDirectory()
        {
            try
            {
                if (!Directory.Exists(Paths.ErrorLogDir))
                    Directory.CreateDirectory(Paths.ErrorLogDir);

                string[] files = Directory.GetFiles(Paths.ErrorLogDir);

                if (files.Length < 1)
                    return;

                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);

                    if (f.CreationTime < DateTime.Now.AddDays(-30))
                        f.Delete();
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private static async void CheckForServerUpdates()
        {
            await Updater.RunUpdates();
        }
    }
}
