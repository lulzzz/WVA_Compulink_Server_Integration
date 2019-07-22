using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Services
{
    internal class CustomWebHostService : WebHostService
    {
        public CustomWebHostService(IWebHost host) : base(host)
        {

        }

        protected override void OnStarting(string[] args)
        {
            // TODO: Notify service starting
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            // TODO: Notify service started
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            // TODO: Notify service stopped
            base.OnStopping();
        }

        protected override void OnStopped()
        {
            // TODO: Notify service stopped
            base.OnStopped();
        }
    }
}
