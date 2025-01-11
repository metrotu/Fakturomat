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
        
        public async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;

            if (await ((LoginViewModel)DataContext).Login(password))
            {
                HomePage home = new();
                home.Show();
                //Close();
            }
        }
       
    }
}