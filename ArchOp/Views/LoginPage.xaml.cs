using ArchOp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace ArchOp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            //Close();
        }

        public LoginPage()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Password = PasswordBox.Password;
        }
    }
}