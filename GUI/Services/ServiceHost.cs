using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Utility.Files;

namespace WVA_Compulink_Server_Integration.Services
{
    public class ServiceHost
    {
        private static readonly string AppName = Assembly.GetCallingAssembly().GetName().Name.ToString();

        public static void Install()
        {
            string pathToService = Paths.ServiceExePath;
            string installCommand = $"sc create {AppName} binPath=\"{pathToService}\"";

            IssueCommand(installCommand);
        }

        public static void Uninstall()
        {
            string uninstallCommand = $"sc delete {AppName}";

            IssueCommand(uninstallCommand);
        }

        public static void Start()
        {
            string startCommand = $"net start {AppName}";

            IssueCommand(startCommand);
        }

        public static void Stop()
        {
            string stopCommand = $"net stop {AppName}";

            IssueCommand(stopCommand);
        }

        public static bool IsRunning()
        {
            return new ServiceController(AppName).Status == ServiceControllerStatus.Running ? true : false;
        }

        public static bool IsInstalled()
        {
            return ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == $"{AppName}") != null ? true : false;
        }

        private static void IssueCommand(string command)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                Verb = "runas"
            };
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
