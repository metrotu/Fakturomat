using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace ArchOp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public string Email { get; set; }
        private readonly string password;


        public LoginViewModel() { }
        public LoginViewModel(string password)
        {
            this.password = password;
        }

        public async Task<bool> Login(string password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            try
            {
                var session = await App.SupabaseClient.Auth.SignIn(Email, password);
                if (session != null && session.User != null)
                {
                    MessageBox.Show("LoginSucc");
                }
            }
            catch (Exception _)
            {
                MessageBox.Show("Password and username don't match an existing user.");
                return false;
            }
            return true;
        }


    }
}
