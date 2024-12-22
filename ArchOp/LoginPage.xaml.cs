using ArchOp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace ArchOp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {

        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
        
        public async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;

            if (await ((LoginViewModel)DataContext).Login(password))
            {
                HomePage home = new();
                home.Show();
                Close();
            }
        }
       
    }
}