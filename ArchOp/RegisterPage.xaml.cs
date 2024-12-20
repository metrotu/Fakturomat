using ArchOp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using ArchOp.Utils;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window, LoginWindow
    {
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }
        public async void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var email = ((RegisterViewModel)DataContext).Email;
            var password = PasswordBox.Password;
            var rePass = RePasswordBox.Password;
            if (email == null)
            {
                MessageBox.Show("Please enter an email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!RegexUtilities.IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePass))
            {
                MessageBox.Show("Please enter both password and re-password.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != rePass)
            {
                MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RegexUtilities.CheckPasswordStrength(password) == RegexUtilities.PasswordStrength.VeryWeak)
            {
                MessageBox.Show("Password is too weak.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var supabse = await App.SupabaseClient.Auth.SignUp(email, password);
            MessageBox.Show("Registration in progress, an email has been sent.", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
        
        }
        public void LoadHome()
        {
            HomePage home = new();
            home.Show();
            Close();
        }

    }
}
