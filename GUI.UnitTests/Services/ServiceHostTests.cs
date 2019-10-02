using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Services;
using WVA_Connect_CSI.Utility.Files;

namespace GUI.UnitTests.Services
{
    [TestClass]
    class ServiceHostTests
    {
        [TestMethod]
        public  void ServiceHost_Install_ServiceExistsTrue()
        {
            ServiceHost.Install();

            bool actualValue = ServiceHost.IsRunning();
            bool expectedValue = true;

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void ServiceHost_Uninstall_ServiceExistFalse()
        {
            ServiceHost.Install();

            bool actualValue = ServiceHost.IsRunning();
            bool expectedValue = true;

            Assert.AreEqual(actualValue, expectedValue);
        }

    }
}
