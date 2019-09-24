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
    class ITAdminRoleTests
    {
        [TestMethod]
        public void ITAdmin_CanViewOrders_ReturnsTrue()
        {
            var itAdmin = new ITAdminRole(2, "stark");

            Assert.IsFalse(itAdmin.CanViewOrders);
        }

        [TestMethod]
        public void ITAdmin_CanViewUsers_ReturnsTrue()
        {
            var itAdmin = new ITAdminRole(2, "stark");

            Assert.IsTrue(itAdmin.CanViewUsers);
        }

        [TestMethod]
        public void ITAdmin_RoleIdSet_EqualsTwo()
        {
            var itAdmin = new ITAdminRole(2, "stark");

            Assert.AreEqual(itAdmin.RoleId, 2);
        }

        [TestMethod]
        public void ITAdmin_UserNameSet_EqualsStark()
        {
            var itAdmin = new ITAdminRole(2, "stark");

            Assert.AreEqual(itAdmin.UserName, "stark");
        }
    }
}
