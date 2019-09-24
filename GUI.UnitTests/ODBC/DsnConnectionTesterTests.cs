using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.UnitTests.ODBC
{
    [TestClass]
    class DsnConnectionTesterTests
    {
        [TestMethod]
        public void IsGoodConnection_ReturnsTrue()
        {
            bool goodConnection = WVA_Connect_CSI.ODBC.DsnConnectionTester.IsGoodConnection();

            Assert.IsTrue(goodConnection);
        }
    }
}
