using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ArchOp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        public string Email { get; set; }
        private string password;
        public string Password { get => password; set => password = value; }

        public bool IsLoginButtonEnabled { get; set; }



        private RelayCommand loginButtonCommand;
        public ICommand LoginButtonCommand => loginButtonCommand ??= new RelayCommand(LoginButton);

        private RelayCommand backButtonCommand;
        public ICommand BackButtonCommand => backButtonCommand ??= new RelayCommand(BackButton);

        public LoginViewModel(NavStore navStore)
        {
            IsLoginButtonEnabled = true;
            this.navStore = navStore;
        }

        public async Task<bool> Login(string password)
        {
            IsLoginButtonEnabled = false;
            OnPropertyChanged(nameof(IsLoginButtonEnabled));

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);

                IsLoginButtonEnabled = true;
                OnPropertyChanged(nameof(IsLoginButtonEnabled));

                return false;
            }
            try
            {
                var session = await App.SupabaseClient.Auth.SignIn(Email, password);
                /*if (session != null && session.User != null)
                {
                    MessageBox.Show("LoginSucc");
                }*/
            }
            catch (Exception _)
            {
                MessageBox.Show("Password and username don't match an existing user.");
                IsLoginButtonEnabled = true;
                OnPropertyChanged(nameof(IsLoginButtonEnabled));
                return false;
            }

            IsLoginButtonEnabled = true;
            OnPropertyChanged(nameof(IsLoginButtonEnabled));

            return true;

        }


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
