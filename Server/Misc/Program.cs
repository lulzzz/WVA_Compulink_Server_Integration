using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Data;
using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WVA_Compulink_Server_Integration
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

                Database.UpdateChangedColumns();


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
            Database.CreateDatabaseFile();
            Database.CreateTables();
        }

    }
}
