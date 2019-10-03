using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.ViewModels;

namespace GUI.UnitTests.ViewModels
{
    [TestClass]
    public class UsersViewModelTests
    {

        // <NOTE> 
        //
        //          ImportUsers tests will go here after it has been refactored and it testable
        //
        // </NOTE>

        [TestMethod]
        public void UserViewModel_UserNameExists_True()
        {
            var user = new UsersViewModel();
            string userName = "testuser";

            // Make sure the test user is reset every time test is run
            user.DeleteUser(userName);
            user.CreateUser(userName, "testpassword", "testemail@email.com", 0, 0);

            bool actual = user.UserNameExists(userName);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UserViewModel_UserNameExists_False()
        {
            var user = new UsersViewModel();
            string userName = "testuser";

            // Make sure the test user does not exist 
            user.DeleteUser(userName);

            bool actual = user.UserNameExists(userName);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void UserViewModel_EmailExists_True()
        {
            var user = new UsersViewModel();
            string email = "testemail@email.com";
            string userName = "testuser";

            // Make sure the test user is reset every time test is run
            user.DeleteUser(userName);
            user.CreateUser(userName, "testpassword", "testemail@email.com", 0, 0);

            bool actual = user.EmailExists(email);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UserViewModel_EmailExists_False()
        {
            var user = new UsersViewModel();
            string email = "testemail@email.com";
            string userName = "testuser";

            // Make sure the test user does not exist 
            user.DeleteUser(userName);

            bool actual = user.EmailExists(email);

            Assert.IsFalse(actual);
        }

    }
}
