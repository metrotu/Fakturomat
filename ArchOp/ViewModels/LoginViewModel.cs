using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace ArchOp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        public string Email { get; set; }
        private string password;
        public string Password { get=>password; set=>password = value; }


        public LoginViewModel(NavStore navStore) 
        {
            this.navStore = navStore;
        }
        public LoginViewModel(NavStore navStore, string password) : this(navStore)
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

        private RelayCommand loginButtonCommand;
        public ICommand LoginButtonCommand => loginButtonCommand ??= new RelayCommand(LoginButton);

        private RelayCommand backButtonCommand;
        public ICommand BackButtonCommand => backButtonCommand ??= new RelayCommand(BackButton);

        private void BackButton()
        {
            navStore.CurrentViewModel = new DashboardViewModel(navStore);
        }
        private async void LoginButton()
        {
            if (await Login(password))
            {
                var mainFrame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
                navStore.CurrentViewModel = new HomePageViewModel(navStore);
            }

        }
    }
}
