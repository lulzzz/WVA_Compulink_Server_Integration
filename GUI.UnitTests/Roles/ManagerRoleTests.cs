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
    class ManagerRoleTests
    {

        [TestMethod]
        public void ManagerRole_CanViewOrders_ReturnsTrue()
        {
            var manager = new ManagerRole(1, "stark");

            Assert.IsTrue(manager.CanViewOrders);
        }

        [TestMethod]
        public void ManagerRole_CanViewUsers_ReturnsTrue()
        {
            var manager = new ManagerRole(1, "stark");

            Assert.IsFalse(manager.CanViewUsers);
        }

        [TestMethod]
        public void ManagerRole_RoleIdSet_EqualsOne()
        {
            var manager = new ManagerRole(1, "stark");

            Assert.AreEqual(manager.RoleId, 1);
        }

        [TestMethod]
        public void ManagerRole_UserNameSet_EqualsStark()
        {
            var manager = new ManagerRole(1, "stark");

            Assert.AreEqual(manager.UserName, "stark");
        }
    }
}
