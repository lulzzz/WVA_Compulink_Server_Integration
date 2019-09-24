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
            var user = new UserRole(0, "stark");

            Assert.AreEqual(user.RoleId, 0);
        }

        [TestMethod]
        public void UserRole_UserNameSet_EqualsStark()
        {
            var user = new UserRole(0, "stark");

            Assert.AreEqual(user.UserName, "stark");
        }
    }
}
