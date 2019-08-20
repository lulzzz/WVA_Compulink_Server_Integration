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

        private List<User> GetUsers()
        {
            var users = new List<User>();



            return users;
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
            string file = GetCsvPath();

            if (file == "")
                throw new FileFormatException();
            if (!File.Exists(file))
                throw new FileNotFoundException($"{file}");
            else
            {
                // TODO
                foreach (User user in GetUsers())
                {
                    database.CreateUser(user);
                }
            }

        }

        public bool UserNameExists(string username)
        {
            if (database.UserNameExists(username))
                return true;
            else
                return false;
        }

        public bool CreateUser(string username, string password, string email, int roleId)
        {
            return database.CreateUser(username, password, email, roleId);
        }

        public bool DeleteUser(string username)
        {
            return database.DeleteUser(username);  
        }

   
    }
}
