﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVA_Compulink_Server_Integration.Data;
using WVA_Compulink_Server_Integration.WebTools;
using WVA_Compulink_Server_Integration.Models.Users;
using WVA_Compulink_Server_Integration.Models.Validations;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WVA_Compulink_Server_Integration.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        [HttpPost("register")]
        public User Register([FromBody]User user)
        {
            try
            {
                var userResponse = new User();

                if (Database.CheckIfEmailExists(user?.Email))
                {
                    userResponse.Status = "FAIL";
                    userResponse.Message = "Email already exists";
                }
                else if (Database.CheckIfUserNameExists(user?.UserName))
                {
                    userResponse.Status = "FAIL";
                    userResponse.Message = "UserName already exists";
                }
                else
                    userResponse = Database.CreateUser(user);

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

                userResponse = Database.CheckCredentials(user);

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
                return new User() { Email = Database.GetEmail(user.UserName) };
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
                bool passwordChanged = Database.ChangePassword(user.UserName, user.Password);

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

            foreach (KeyValuePair<string, string> pair in Startup.config?.Location)
                availableAccounts.Add(pair.Key);

            return availableAccounts;
        }

    }
}
