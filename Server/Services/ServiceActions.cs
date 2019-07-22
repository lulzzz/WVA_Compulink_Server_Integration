using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Services
{
    public class ServiceActions
    {
        private readonly string AppName = Assembly.GetCallingAssembly().GetName().Name.ToString();

        public void Install()
        {
            string pathToService = "";
            string installCommand = $"sc create {AppName} binPath=\"{pathToService}\"";

            IssueCommand(installCommand);
        }

        public void Uninstall()
        {
            string uninstallCommand = $"sc delete {AppName}";

            IssueCommand(uninstallCommand);
        }

        public void Start()
        {
            string startCommand = $"net start {AppName}";

            IssueCommand(startCommand);
        }

        public void Stop()
        {
            string stopCommand = $"net stop {AppName}";

            IssueCommand(stopCommand);
        }

        private void IssueCommand(string command)
        {
            var cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;

                cmd.Start();
                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
        }
    }
}
