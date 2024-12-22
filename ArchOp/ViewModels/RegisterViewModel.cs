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
    public class RegisterViewModel
    {
        public string Email { get; set; }

        public RegisterViewModel() { }

        public async Task<bool> Register(string password, string rePass)
        {
            if (Email == null)
            {
                MessageBox.Show("Please enter an email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!RegexUtilities.IsValidEmail(Email))
            {
                MessageBox.Show("Please enter a valid email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePass))
            {
                MessageBox.Show("Please enter both password and re-password.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (password != rePass)
            {
                MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (RegexUtilities.CheckPasswordStrength(password) == RegexUtilities.PasswordStrength.VeryWeak)
            {
                MessageBox.Show("Password is too weak.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var supabase = await App.SupabaseClient.Auth.SignUp(Email, password);
            MessageBox.Show("Registration in progress, an email has been sent.", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }


    }
}
