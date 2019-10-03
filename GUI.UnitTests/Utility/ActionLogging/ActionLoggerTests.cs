using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Responses;
using WVA_Connect_CSI.Utility.ActionLogging;
using WVA_Connect_CSI.Utility.Files;
using WVA_Connect_CSI.WebTools;

namespace GUI.UnitTests.Utility.ActionLogging
{
    [TestClass]
    public class ActionLoggerTests
    {
        [TestMethod]
        public void ActionLogger_Log_TakesString_CreatesLogFile()
        {
            ActionLogger.Log("Test Location", "Test Username", 0, "Test Action Message");

            bool actual = File.Exists($"{Paths.TempDir}CSI_Action_Log_{DateTime.Today.ToString("MM-dd-yy")}.txt");
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ActionLogger_Log_TakesString_LogTextNotNull()
        {
            ActionLogger.Log("Test Location", "Test Username", 0, "Test Action Message");

            string actualResult = File.ReadAllText($"{Paths.TempDir}CSI_Action_Log_{DateTime.Today.ToString("MM-dd-yy")}.txt");

            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void ActionLogger_GetAllData_ReturnsNotNull()
        {
            // Log data to make sure there is always something there when this test is run
            ActionLogger.Log("Test Location", "Test Username", 0, "Test Action Message");

            var data = ActionLogger.GetAllData();

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ActionLogger_ReportData_ReturnsSUCCESS()
        {
            var jsonError = new JsonError()
            {
                ActNum = "426761f0-3e9d-4dfd-bdbf-0f35a232c285",
                Error = "test data",
                Application = "WVA_Connect_CSI_Tests",
                AppVersion = "1.2.3"
            };

            string strResponse = API.Post(Paths.WisVisErrors, jsonError);
            var response = JsonConvert.DeserializeObject<Response>(strResponse);

            string actual = response.Status;
            string expected = "SUCCESS";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ActionLogger_ReportData_ReturnsFAIL()
        {
            var jsonError = new JsonError();

            string strResponse = API.Post(Paths.WisVisErrors, jsonError);
            var response = JsonConvert.DeserializeObject<Response>(strResponse);

            string actual = response.Status;
            string expected = "FAIL";

            Assert.AreEqual(expected, actual);
        }
    }
}
