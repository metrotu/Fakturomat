using ArchOp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ArchOp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public string Email { get; set; }

        public RegisterViewModel() { }

        public async Task<int> Register(string password, string rePass)
        {
            List<string> massage = [];
            int sum = 0;
            if (Email == null)
            {
                massage.Add("Please enter an email address. \n");
                //MessageBox.Show("Please enter an email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                sum += 1;
            }
            else if (!RegexUtilities.IsValidEmail(Email))
            {
                massage.Add("Please enter a valid email address. \n");
                //MessageBox.Show("Please enter a valid email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                sum += 1;

            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePass))
            {
                massage.Add("Please enter both password and re-password. \n");
                //MessageBox.Show("Please enter both password and re-password.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                sum += 5;
            }
            else if (password != rePass)
            {
                massage.Add("Passwords do not match. \n");
                //MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                sum += 5;
            }
            else if (RegexUtilities.CheckPasswordStrength(password) == RegexUtilities.PasswordStrength.VeryWeak)
            {
                massage.Add("Password is too weak. \n");
                //MessageBox.Show("Password is too weak.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                sum += 5;

            }
            if (sum > 0) {
                string s = "";
                massage.ForEach(x => s += x);
                s += sum;
                MessageBox.Show(s, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return sum;
            }
            await App.SupabaseClient.Auth.SignUp(Email, password);
            MessageBox.Show("Registration in progress, an email has been sent.", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }




    }
}
