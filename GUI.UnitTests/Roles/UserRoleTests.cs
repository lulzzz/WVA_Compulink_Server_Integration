using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Roles;

namespace GUI.UnitTests.Roles
{
    [TestClass]
    class UserRoleTests
    {
        [TestMethod]
        public void UserRole_CanViewOrders_ReturnsTrue()
        {
            var user = new UserRole(0, "stark");

            Assert.IsFalse(user.CanViewOrders);
        }

        [TestMethod]
        public void UserRole_CanViewUsers_ReturnsTrue()
        {
            var user = new UserRole(0, "stark");

            Assert.IsFalse(user.CanViewUsers);
        }

        [TestMethod]
        public void UserRole_RoleIdSet_EqualsZero()
        {
            int actual = new UserRole(0, "stark").RoleId;
            int expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UserRole_UserNameSet_EqualsStark()
        {
            string actual = new UserRole(0, "stark").UserName;
            string expected = "stark";

            Assert.AreEqual(expected, actual);
        }
    }
}
