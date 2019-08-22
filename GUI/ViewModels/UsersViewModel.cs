using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.Utility.Files;

namespace WVA_Connect_CSI.ViewModels
{
    class UsersViewModel
    {
        Database database;

        public UsersViewModel()
        {
            database = new Database();
        }

        private List<User> GetUsers(string[] csvFile)
        {
            var users = new List<User>();

            foreach (string line in csvFile)
            {
                string[] lineItems = line.Split(',');

                var user = new User()
                {
                    UserName = lineItems[0].Trim(),
                    Password = Crypto.ConvertToHash("wisvis123"),
                    Email = lineItems[1].Trim(),
                    RequiresPasswordChange = 1
                };
               
                try
                {
                    if (lineItems[2].Trim() == "" && lineItems[2].Trim() == "0")
                        user.RoleId = 0;
                    else
                        user.RoleId = Convert.ToInt32(lineItems[2]);
                }
                catch
                {
                    user.RoleId = 0;
                }

                users.Add(user);
            }

            return users;
        }

        private void AppendToLogFile(string text)
        {
            File.AppendAllText(Paths.ImportCsvUsersLog ,text);
        }


        private string GetCsvPath()
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            choofdlog.ShowDialog();

            return choofdlog.FileName;
        }

        public void ImportUsers()
        {
            string csvFile = GetCsvPath();

            if (csvFile == "")
                throw new FileFormatException();
            if (!File.Exists(csvFile))
                throw new FileNotFoundException($"{csvFile}");
            else
            {
                string[] csvLines = File.ReadAllLines(csvFile);

                foreach (User user in GetUsers(csvLines))
                {
                    try
                    {
                        if (UserNameExists(user.UserName))
                        {
                            AppendToLogFile($"Failed to create user. Username '{user.UserName}' is already in use!\n");
                        }
                        else if (EmailExists(user.Email))
                        {
                            AppendToLogFile($"Failed to create user. Email '{user.Email}' is already in use!\n");
                        }
                        else
                        {
                            database.CreateUser(user);
                            AppendToLogFile($"User '{user.UserName}' created!\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendToLogFile($"Error creating user '{user.UserName}'. Error={ex.Message}\n");
                    }
                }
            }
            MessageBox.Show("User import complete! An import results file has been generated and placed on your desktop.","",MessageBoxButton.OK);
        }

        public bool UserNameExists(string username)
        {
            if (database.UserNameExists(username))
                return true;
            else
                return false;
        }

        public bool EmailExists(string email)
        {
            if (database.EmailExists(email))
                return true;
            else
                return false;
        }

        public bool CreateUser(string username, string password, string email, int roleId, int requiresPasswordChange)
        {
            return database.CreateUser(username, password, email, roleId, requiresPasswordChange);
        }

        public bool DeleteUser(string username)
        {
            return database.DeleteUser(username);  
        }

   
    }
}
