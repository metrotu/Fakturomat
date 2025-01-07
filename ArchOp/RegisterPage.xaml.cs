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
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        //trzeba zrobic zeby przy rejestracji rowniez dodawalo do tablic uzytkownikow
        public async void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var email = ((RegisterViewModel)DataContext).Email;
            var password = PasswordBox.Password;
            var rePass = RePasswordBox.Password;
            if (await ((RegisterViewModel)DataContext).Register(password, rePass))
            {

                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();

            }
        }
        

    }
}
