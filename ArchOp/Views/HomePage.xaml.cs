using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }
    
        private void CreateInvoiceClick(object sender, RoutedEventArgs e)
        {
            //???? czy tak sie powinno robic
            //((Button)sender).IsEnabled = false;
            //((Button)sender).Visibility = Visibility.Hidden;
            //???

            MainFrame.Navigate(new InvoiceMakerPage());
        }

        private void ViewSentInvoicesClick(object sender, RoutedEventArgs e)
        {
            SentInvoicesWindow sentInvoicesWindow = new();
            sentInvoicesWindow.Show();

        }

        private void SetCompanyClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SetCompanyPage());
        }
        private void AddCompanyClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddCompanyPage());
        }

    }
}
