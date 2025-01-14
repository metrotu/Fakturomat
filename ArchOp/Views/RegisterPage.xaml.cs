using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArchOp.ViewModels;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : UserControl
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void ShowcaseWrongInputs(int registrationRes)
        {
            switch (registrationRes)
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
                    //Close();
                    break;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((RegisterViewModel)DataContext).Password = PasswordBox.Password;
        }

        private void RePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((RegisterViewModel)DataContext).RePass = RePasswordBox.Password;
        }
    }
}
