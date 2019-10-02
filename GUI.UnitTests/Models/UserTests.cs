using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WVA_Connect_CSI.Models;

namespace GUI.UnitTests.Models
{
    [TestClass]
    public class UserTests
    {

        [TestMethod]
        public void User_RoleName_EqualsUser()
        {
            var user = new User()
            {
                RoleId = 0
            };

            string actualValue = user.RoleName;
            string expectedValue = "User";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_RoleName_EqualsManager()
        {
            var user = new User()
            {
                RoleId = 1
            };

            string actualValue = user.RoleName;
            string expectedValue = "Manager";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_RoleName_EqualsItAdmin()
        {
            var user = new User()
            {
                RoleId = 2
            };

            string actualValue= user.RoleName;
            string expectedValue = "IT Admin";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_RoleName_EqualsSuperAdmin()
        {
            var user = new User()
            {
                RoleId = 3
            };

            string actualValue = user.RoleName;
            string expectedValue = "Super Admin";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_InvalidRoleId_EqualsUser()
        {
            var user = new User()
            {
                RoleId = 4
            };

            string actualValue = user.RoleName;
            string expectedValue = "User";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_StrRequiresPasswordChange_EqualsYes()
        {
            var user = new User()
            {
                RequiresPasswordChange = 1
            };

            string actualValue = user.StrRequiresPasswordChange;
            string expectedValue = "Yes";

            Assert.AreEqual(actualValue, expectedValue);
        }

        [TestMethod]
        public void User_StrRequiresPasswordChange_EqualsNo()
        {
            var user = new User()
            {
                RequiresPasswordChange = 0
            };

            string actualValue = user.StrRequiresPasswordChange;
            string expectedValue = "No";

            Assert.AreEqual(actualValue, expectedValue);
        }
    }
}
