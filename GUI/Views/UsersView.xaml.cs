using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Security;
using WVA_Connect_CSI.ViewModels;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        UsersViewModel usersViewModel;
        Role userRole;

        public UsersView()
        {

        }

        public UsersView(Role role)
        {
            InitializeComponent();
            usersViewModel = new UsersViewModel();
            userRole = role;
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

        //
        // Import users from a CSV file
        //

        private void ImportUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                usersViewModel.ImportUsers();
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
        }

        //
        // Remove a single user from the database
        //

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (usersViewModel.UserNameExists(DeleteUserTextBox.Text))
            {
                bool deleted = usersViewModel.DeleteUser(DeleteUserTextBox.Text);

                if (deleted)
                {
                    MessageBox.Show("User Deleted!", "", MessageBoxButton.OK);
                    DeleteUserTextBox.Text = "";
                }
                else
                    MessageBox.Show("Error Deleting User!", "", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("User Not Found!", "", MessageBoxButton.OK);
            }
        }

        //
        // Create a new user in the database
        //

        private void TryCreateUser()
        {
            if (usersViewModel.UserNameExists(UserNameTextBox.Text))
            {
                MessageBox.Show("This username exists already!", "", MessageBoxButton.OK);
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
                TryCreateUser();
            else
                return;
        }


    }
}
