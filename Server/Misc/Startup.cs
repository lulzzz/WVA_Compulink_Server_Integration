using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WVA_Compulink_Server_Integration.Utilities.Files;
using System.Windows;

namespace WVA_Compulink_Server_Integration
{
    public class Startup
    {
        public static WvaConfig config = new WvaConfig();

        public Startup(IHostingEnvironment env)
        {
            config = JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText($@"{Paths.WvaConfigFile}"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
