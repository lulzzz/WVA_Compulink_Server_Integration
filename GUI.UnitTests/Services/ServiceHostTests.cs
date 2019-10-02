using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WVA_Connect_CSI.Services;
using WVA_Connect_CSI.Utility.Files;

namespace GUI.UnitTests.Services
{
    [TestClass]
    class ServiceHostTests
    {
        [TestMethod]
        public  void ServiceHost_Install_IsInstalled_True()
        {
            ServiceHost.Install();

            Thread.Sleep(2000); // Wait for system to install service

            bool actualValue = ServiceHost.IsInstalled();
            bool expectedValue = true;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_Uninstall_IsInstalled_False()
        {
            ServiceHost.Uninstall();

            Thread.Sleep(2000); // Wait for system to remove service

            bool actualValue = ServiceHost.IsInstalled();
            bool expectedValue = false;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_Start_IsRunning_True()
        {
            ServiceHost.Start();

            Thread.Sleep(2000); // Wait for system to spin up service

            bool actualValue = ServiceHost.IsRunning();
            bool expectedValue = true;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_Stop_IsRunning_False()
        {
            ServiceHost.Stop();

            Thread.Sleep(2000); // Wait for system to terminate service

            bool actualValue = ServiceHost.IsRunning();
            bool expectedValue = false;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_IsRunning_True()
        {
            ServiceHost.Start();

            Thread.Sleep(2000); // Wait for system to start the service

            bool actualValue = new ServiceController("WVA_Connect_CSI_Service").Status == ServiceControllerStatus.Running ? true : false;
            bool expectedValue = true;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_IsRunning_False()
        {
            ServiceHost.Stop();

            Thread.Sleep(2000); // Wait for system to start the service

            bool actualValue = new ServiceController("WVA_Connect_CSI_Service").Status == ServiceControllerStatus.Running ? true : false;
            bool expectedValue = false;

            Assert.AreEqual(actualValue, expectedValue);
        }

    }
}
