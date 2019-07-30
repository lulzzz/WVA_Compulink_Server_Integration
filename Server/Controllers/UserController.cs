using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.WebTools;
using WVA_Connect_CSI.Models.Users;
using WVA_Connect_CSI.Models.Validations;
using WVA_Connect_CSI.Utilities.Files;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Connect_CSI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private Database sqliteDatabase;

        public UserController()
        {
            sqliteDatabase = new Database();
        }

        [HttpPost("register")]
        public User Register([FromBody]User user)
        {
            try
            {
                var userResponse = new User();

                if (sqliteDatabase.CheckIfEmailExists(user?.Email))
                {
                    userResponse.Status = "FAIL";
                    userResponse.Message = "Email already exists";
                }
                else if (sqliteDatabase.CheckIfUserNameExists(user?.UserName))
                {
                    userResponse.Status = "FAIL";
                    userResponse.Message = "UserName already exists";
                }
                else
                    userResponse = sqliteDatabase.CreateUser(user);

                return userResponse;
            }
            catch (Exception x)
            {
                return new User() { Status = "ERROR", Message = $"{x.Message}" };
            }
        }

        [HttpPost("login")]
        public User Login([FromBody]User user)
        {
            try
            {
                var userResponse = new User();

                userResponse = sqliteDatabase.CheckCredentials(user);

                if (userResponse == null)
                {
                    return new User()
                    {
                        Status = "FAIL",
                        Message = "Invalid login credentials"
                    };
                }

                return userResponse;
            }
            catch (Exception x)
            {
                return new User() { Status = "ERROR", Message = $"{x.Message}" };
            }
        }

        [HttpPost("GetEmail")]
        public User GetEmail([FromBody]User user)
        {
            try
            {
                return new User() { Email = sqliteDatabase.GetEmail(user.UserName) };
            }
            catch (Exception x)
            {
                return new User() { Status = "ERROR", Message = $"{x.Message}" };
            }
        }

        [HttpPost("changePass")]
        public User ChangePassword([FromBody]User user)
        {
            try
            {
                bool passwordChanged = sqliteDatabase.ChangePassword(user.UserName, user.Password);

                if (passwordChanged)
                    return new User() { Status = "SUCCESS", Message = $"Password changed!" };
                else
                    return new User() { Status = "FAIL", Message = $"Failed to update password!" };
            }
            catch (Exception x)
            {
                return new User() { Status = "ERROR", Message = $"{x.Message}" };
            }
        }

        [HttpPost("reset-email")]
        public string ResetEmail([FromBody]EmailValidation validationSend)
        {
            string endpoint = $"{Paths.WisVisEmailReset}";

            return API.Post(endpoint, validationSend);
        }

        [HttpPost("reset-email-check")]
        public string ResetEmailCheck([FromBody]EmailValidationCode validationCode)
        {
            string endpoint = $"{Paths.WisVisEmailResetCheck}";

            return API.Post(endpoint, validationCode);
        }

        [HttpGet("get-acts")]
        public List<string> GetAvailableAccounts()
        {
            List<string> availableAccounts = new List<string>();

            if (Startup.config?.Location == null)
                return null;

            foreach (KeyValuePair<string, string> pair in Startup.config?.Location)
                availableAccounts.Add(pair.Key);

            return availableAccounts;
        }

    }
}
