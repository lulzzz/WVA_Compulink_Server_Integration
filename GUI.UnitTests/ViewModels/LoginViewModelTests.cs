using Microsoft.VisualStudio.TestTools.UnitTesting;
using WVA_Connect_CSI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;

namespace GUI.UnitTests.ViewModels
{
    [TestClass()]
    public class LoginViewModelTests
    {
        [TestMethod()]
        public void GetLoginRoleTest()
        {
            LoginViewModel loginViewModel = new LoginViewModel();

            int actual = loginViewModel.GetLoginRole("stark", "462090fee12886df35b0dd449f4a996e24f288c6e4b2bade80c0fc7971c66631");
            int expected = 3;

            Assert.AreEqual(actual, expected);
        }
    }
}