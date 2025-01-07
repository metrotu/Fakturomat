using ArchOp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using ArchOp.Utils;
using System.Drawing;
using System.Windows.Media;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

        //trzeba zrobic zeby przy rejestracji rowniez dodawalo do tablic uzytkownikow
        public async void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var email = ((RegisterViewModel)DataContext).Email;
            var password = PasswordBox.Password;
            var rePass = RePasswordBox.Password;
            int registrationResult = await ((RegisterViewModel)DataContext).Register(password, rePass);
            switch (registrationResult)
            {
                case 1:
                    EmailTextBox.Background = Brushes.Salmon;
                    EmailTextBox.Focus();
                    PasswordBox.Background = Brushes.White;
                    RePasswordBox.Background = Brushes.White;
                    break;
                case 4:
                    EmailTextBox.Background = Brushes.Salmon;
                    EmailTextBox.Focus();
                    PasswordBox.Background = Brushes.White;
                    RePasswordBox.Background = Brushes.White;
                    break;
                case 5:
                    EmailTextBox.Background = Brushes.White;
                    PasswordBox.Background = Brushes.Salmon;
                    RePasswordBox.Background = Brushes.Salmon;
                    EmailTextBox.Focus();
                    break;
                case 6:
                    EmailTextBox.Background = Brushes.Salmon;
                    PasswordBox.Background = Brushes.Salmon;
                    RePasswordBox.Background = Brushes.Salmon;
                    EmailTextBox.Focus();
                    break;
                case 0:
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                    break;
            }
            
           
        }
        

    }
}
