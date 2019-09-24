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
    class SuperAdminRoleTests
    {
        [TestMethod]
        public void SuperAdmin_CanViewOrders_ReturnsTrue()
        {
            var superAdmin = new SuperAdminRole(3, "stark");

            Assert.IsTrue(superAdmin.CanViewOrders);
        }

        [TestMethod]
        public void SuperAdmin_CanViewUsers_ReturnsTrue()
        {
            var superAdmin = new SuperAdminRole(3, "stark");

            Assert.IsTrue(superAdmin.CanViewUsers);
        }

        [TestMethod]
        public void SuperAdmin_RoleIdSet_EqualsThree()
        {
            var superAdmin = new SuperAdminRole(3, "stark");

            Assert.AreEqual(superAdmin.RoleId, 3);
        }

        [TestMethod]
        public void SuperAdmin_UserNameSet_EqualsStark()
        {
            var superAdmin = new SuperAdminRole(3, "stark");

            Assert.AreEqual(superAdmin.UserName, "stark");
        }
    }
}
