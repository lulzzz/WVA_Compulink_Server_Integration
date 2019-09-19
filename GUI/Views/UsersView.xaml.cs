using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WVA_Connect_CSI.Data;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.Utility.ActionLogging;
using WVA_Connect_CSI.ViewModels;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        Database database;
        UsersViewModel usersViewModel;
        Role userRole;

        public UsersView()
        {
            database = new Database();
        }

        public UsersView(Role role)
        {
            InitializeComponent();
            usersViewModel = new UsersViewModel();
            userRole = role;
            database = new Database();
            SetUpDataGrid();
            SetUpContextMenu();
        }

        //
        // Set up
        //

        private void SetUpDataGrid()
        {
            var users = database.GetUsers();

            foreach (User user in users)
                UsersDataGrid.Items.Add(user);
        }

        private void SetUpContextMenu()
        {
            MenuItem menuItem = new MenuItem() { Header =  "Delete"};
            menuItem.Click += new RoutedEventHandler(UsersDataGridContextMenu_Click);
            UsersDataGridContextMenu.Items.Add(menuItem);
        }

        //
        // Import users from a CSV file
        //

        private void ImportUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActionLogger.Log(GetType().FullName + nameof(ImportUsersButton_Click), userRole, "<Import_Users_Start>");
                usersViewModel.ImportUsers(userRole);
                RefreshGrid();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Cannot find path to file '{ex.Message}'", "File Not Found", MessageBoxButton.OK);
            }
            catch (FileFormatException)
            {
               
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ActionLogger.Log(GetType().FullName + nameof(ImportUsersButton_Click), userRole, "<Import_Users_End>");
            }
        }

        //
        // Create a new user in the database
        //

        private void TryCreateUser()
        {
            try
            {
                ActionLogger.Log(GetType().FullName + nameof(TryCreateUser), userRole, $"<Creating_User UserName={UserNameTextBox.Text}, Email={EmailTextBox.Text}, RoleId={RoleTextBox.Text}>");

                if (usersViewModel.UserNameExists(UserNameTextBox.Text))
                {
                    MessageBox.Show("This username exists already!", "", MessageBoxButton.OK);
                }
                else if (usersViewModel.EmailExists(EmailTextBox.Text))
                {
                    MessageBox.Show("This email exists already!", "", MessageBoxButton.OK);
                }
                else
                {
                    bool created = usersViewModel.CreateUser(UserNameTextBox.Text, Crypto.ConvertToHash(PasswordTextBox.Text), EmailTextBox.Text, RoleTextBox.SelectedIndex, 0);

                    if (created)
                    {
                        MessageBox.Show("User Created!", "", MessageBoxButton.OK);
                        UserNameTextBox.Text = "";
                        PasswordTextBox.Text = "";
                        EmailTextBox.Text = "";
                        RoleTextBox.SelectedIndex = 0;
                    }
                    else
                        MessageBox.Show("Error Creating User!", "", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {

            }
        }

        private void RefreshGrid()
        {
            // Remove all items in grid
            UsersDataGrid.Items.Clear();

            // Get update list of users
            var users = database.GetUsers();

            // Add users to grid
            foreach (User user in users)
                UsersDataGrid.Items.Add(user);

            // Refresh grid
            UsersDataGrid.Items.Refresh();
        }

        private bool FormsCompleted()
        {
            if (UserNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Username cannot be blank!", "", MessageBoxButton.OK);
                return false;
            }
            else if (EmailTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Email cannot be blank!", "", MessageBoxButton.OK);
                return false;
            }
            else if (PasswordTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Password cannot be blank!", "", MessageBoxButton.OK);
                return false;
            }
            else
                return true;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (FormsCompleted())
            {
                TryCreateUser();
                RefreshGrid();
            }
            else
                return;
        }

        private void UserNameTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UserNameTextBox.Focus();
            UserNameTextBox.SelectAll();
        }

        private void PasswordTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordTextBox.Focus();
            PasswordTextBox.SelectAll();
        }

        private void EmailTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            EmailTextBox.Focus();
            EmailTextBox.SelectAll();
        }

        private void SearchUsersTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchUsersTextBox.Text = "";
        }

        private void SearchUsersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchUsersTextBox.Text == "")
                SearchUsersTextBox.Text = "Search Users... ";
        }

        private void UsersDataGridContextMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Keep a copy of the selected items collection so we can safely remove the items from the grid
                List<User> usersToDelete = new List<User>();

                foreach (User user in UsersDataGrid.SelectedItems)
                    usersToDelete.Add(user);

                // confirm with user that they want to delete the selected users 
                var result = MessageBox.Show("Are you sure you want to delete the selected users? They will not be able to be recovered.", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // delete user from data base and remove it from the grid
                        foreach (User user in usersToDelete)
                        {
                            ActionLogger.Log(GetType().FullName + nameof(UsersDataGridContextMenu_Click), userRole, $"<Deleting_User> UserName={user.UserName}, Email={user.Email}, Role={user.RoleId}");
                            database.DeleteUser(user.UserName);
                            UsersDataGrid.Items.Remove(user);
                        }
                    }
                    finally
                    {
                        UsersDataGrid.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

    }
}
