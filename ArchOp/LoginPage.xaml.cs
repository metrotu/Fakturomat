using ArchOp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace ArchOp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window, LoginWindow
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
        
        public async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var email = ((LoginViewModel)DataContext).Email;
            var password = PasswordBox.Password;



            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var session = await App.SupabaseClient.Auth.SignIn(email, password);
                if (session != null && session.User != null)
                    MessageBox.Show("LoginSucc");
            }
            catch (Exception _)
            {
                MessageBox.Show("Password and username don't match an existing user.");
            }

        }
        public void LoadHome()
        {
            HomePage home = new();
            home.Show();
            Close();
        }
    }
}