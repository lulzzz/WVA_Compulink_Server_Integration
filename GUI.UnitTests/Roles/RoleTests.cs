using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Security;

namespace GUI.UnitTests.Roles
{
    [TestClass]
    public class RoleTests
    {
        // Determine SuperUser Role

        [TestMethod]
        public void Role_DetermineSuperUserRole_RoleIsOfTypeSuperAdmin()
        {
            var newRole = new Role(3, "stark");

            var checkRole = newRole.DetermineRole();

            Assert.IsInstanceOfType(checkRole, typeof(SuperAdminRole));
        }

        // Determine ITAdmin Role

        [TestMethod]
        public void Role_DetermineITAdminRole_RoleIsOfTypeITAdmin()
        {
            var newRole = new Role(2, "stark");

            var checkRole = newRole.DetermineRole();

            Assert.IsInstanceOfType(checkRole, typeof(ITAdminRole));
        }

        // Determine Manager Role

        [TestMethod]
        public void Role_DetermineManagerRole_RoleIsOfTypeManager()
        {
            var newRole = new Role(1, "stark");

            var checkRole = newRole.DetermineRole();

            Assert.IsInstanceOfType(checkRole, typeof(ManagerRole));
        }

        // Determine User Role

        [TestMethod]
        public void Role_DetermineUserRole_RoleIsOfTypeUser()
        {
            var newRole = new Role(0, "stark");

            var checkRole = newRole.DetermineRole();

            Assert.IsInstanceOfType(checkRole, typeof(UserRole));
        }
    }
}
