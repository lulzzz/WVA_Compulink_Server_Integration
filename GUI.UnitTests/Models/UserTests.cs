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

            string actual = user.RoleName;
            string expected = "User";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_RoleName_EqualsManager()
        {
            var user = new User()
            {
                RoleId = 1
            };

            string actual = user.RoleName;
            string expected = "Manager";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_RoleName_EqualsItAdmin()
        {
            var user = new User()
            {
                RoleId = 2
            };

            string actual  = user.RoleName;
            string expected = "IT Admin";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_RoleName_EqualsSuperAdmin()
        {
            var user = new User()
            {
                RoleId = 3
            };

            string actual = user.RoleName;
            string expected = "Super Admin";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_InvalidRoleId_EqualsUser()
        {
            var user = new User()
            {
                RoleId = 4
            };

            string actual = user.RoleName;
            string expected = "User";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_StrRequiresPasswordChange_EqualsYes()
        {
            var user = new User()
            {
                RequiresPasswordChange = 1
            };

            string actual = user.StrRequiresPasswordChange;
            string expected = "Yes";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void User_StrRequiresPasswordChange_EqualsNo()
        {
            var user = new User()
            {
                RequiresPasswordChange = 0
            };

            string actual = user.StrRequiresPasswordChange;
            string expected = "No";

            Assert.AreEqual(expected, actual);
        }
    }
}
