using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Utilities.Files;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Diagnostics;
using WVA_Connect_CSI.Services;
using System.ServiceProcess;

namespace WVA_Connect_CSI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            { 
                // Set up database
                if (!DatabaseExists())
                    SetupDatabase();

                // Update config file properties
                UpdateConfig();

                // Setup Config in Memory
                new Memory.Storage();

                if (Debugger.IsAttached || args.Contains("--debug"))
                {
                    // Spin up server
                    var configuration = new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build();

                    var hostUrl = configuration["hosturl"];

                    if (string.IsNullOrEmpty(hostUrl))
                        hostUrl = "http://0.0.0.0:44354";

                    var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls(hostUrl)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .Build();

                    host.Run();
                }
                else // Runs this app as a windows service
                {
                    // Spin up server
                    var configuration = new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build();

                    var hostUrl = configuration["hosturl"];

                    if (string.IsNullOrEmpty(hostUrl))
                        hostUrl = "http://0.0.0.0:44354";

                    var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls(hostUrl)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .Build();

                    host.RunAsCustomService();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private static bool DatabaseExists()
        {
            if (File.Exists(Paths.DatabaseFile))
                return true;
            else
                return false;
        }

        private static void SetupDatabase()
        {
            new Database().CreateDatabaseFile();
            new Database().CreateTables();
        }

        private static void UpdateConfig()
        {

        }

    }

    public static class CustomWebHostWindowsServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
