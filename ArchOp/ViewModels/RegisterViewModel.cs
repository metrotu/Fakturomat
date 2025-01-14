using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArchOp.Utils;
using CommunityToolkit.Mvvm.Input;

namespace ArchOp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        public Brush Color1 { get; set; }
        public Brush Color2 { get; set; }
        public Brush Color3 { get; set; }
        public string Email { get; set; }
        private string password;
        public string Password { get => password; set => password = value; }

        private string rePass;
        public string RePass { get => rePass; set => rePass = value; }

        private RelayCommand registerCommand;
        public ICommand RegisterCommand => registerCommand ??= new RelayCommand(RegisterButton);


        private RelayCommand backCommand;
        public ICommand BackCommand => backCommand ??= new RelayCommand(BackButton);



        public RegisterViewModel(NavStore navStore)
        {
            this.navStore = navStore;
        }

        public async Task<int> Register()
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
            if (sum > 0)
            {
                string s = "";
                massage.ForEach(x => s += x);
                MessageBox.Show(s, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);


                switch (sum)
                {
                    case 1:
                        Color1 = Brushes.Salmon;
                        Color2 = Brushes.White;
                        Color3 = Brushes.White;
                        break;
                    case 5:
                        Color1 = Brushes.White;
                        Color2 = Brushes.Salmon;
                        Color3 = Brushes.Salmon;
                        break;
                    case 6:
                        Color1 = Brushes.Salmon;
                        Color2 = Brushes.Salmon;
                        Color3 = Brushes.Salmon;
                        break;

                }
                OnPropertyChanged(nameof(Color1));
                OnPropertyChanged(nameof(Color2));
                OnPropertyChanged(nameof(Color3));
                return sum;
            }
            await App.SupabaseClient.Auth.SignUp(Email, password);
            MessageBox.Show("Registration in progress, an email has been sent.", "Registration Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }



        private async void RegisterButton()
        {
            var registerResult = await Register();
            if (registerResult == 0)
            {
                //nav to dashboard upon registration submition
                navStore.CurrentViewModel = new DashboardViewModel(navStore);
            }

        }

        private async void BackButton()
        {
            navStore.CurrentViewModel = new DashboardViewModel(navStore);
        }



    }
}
