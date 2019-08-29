using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Security;

namespace GUI.UnitTests
{
    [TestClass]
    public class TestClass
    {
        // Testing Format

        [TestMethod]
        public void MethodToTest_Scenario_ExpectedResult()
        {
            // - ARRANGE
            // - Initialize objects

            // - ACT
            // - Act on the initialized object (call a method in the object)

            // - ASSERT
            // - Check expected result to actual result
        }

        // Example Test Method

        [TestMethod]
        public void StartCar_CarStarted_ReturnsTrue()
        {
            // var car = new Car();

            // car.Start();

            // Assert.IsTrue(car.IsStarted);
        }
    }


    [TestClass]
    public class RoleUnitTests
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

        // SuperAdmin

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

        // ITAdminRole

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

        // ManagerRole

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

        // UserRole

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

    [TestClass]
    public class CryptoTests
    {
        [TestMethod]
        public void ConvertToHash_GivesNormalString_ReturnsValue()
        {
            string result = Crypto.ConvertToHash("password123");
            string expected = "2ba33ac8f9f9aad8336b71b4e7f851fe1628709672802ce56c107157a569a5b9";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void ConvertToHash_GivesTextBoxMaxLength_ReturnsValue()
        {
            string result = Crypto.ConvertToHash("aaaaaaaaaaaaaaaaaaaa");
            string expected = "0594ad161e0e5658d3ec5ea839c59e6e9dd0d3fe0621dc65be72ccac304bf1b1";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesBlankString_ReturnsNull()
        {
            Crypto.ConvertToHash("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesNullValue_ReturnsNull()
        {
            Crypto.ConvertToHash(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must be at least 6 characters")]
        public void ConvertToHash_GivesShortString_ThrowsException()
        {
            Crypto.ConvertToHash("aaaaa");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "'inputString' must not exceed 63 characters")]
        public void ConvertToHash_GivesHugeString_ThrowsException()
        {
            Crypto.ConvertToHash("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }

    [TestClass]
    public class DatabaseTests
    {
       


    }


}
