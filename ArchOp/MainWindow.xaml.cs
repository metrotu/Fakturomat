using System;
using System.Windows;


namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavToLogin(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new();
            loginPage.Show();

            Close();
        }
        private void NavToRegister(object sender, RoutedEventArgs e)
        {
            RegisterPage registerPage = new();
            registerPage.Show();
            
            Close();
        }
    }
}
