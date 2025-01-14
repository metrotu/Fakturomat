using System.Windows.Controls;
using ArchOp.ViewModels;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy SentInvoicesWindow.xaml
    /// </summary>
    public partial class SentInvoicesWindow : Page
    {
        public SentInvoicesWindow(NavStore navStore)
        {
            InitializeComponent();
            DataContext = new SentInvoicesViewModel(navStore);
        }

        /*private async void DownloadClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var invoice = await DBRequests.GetInvoiceById(Convert.ToInt32(b.Content));
            SentInvoicesViewModel.DownloadInvoiceCommand(invoice);
        }*/
    }
}
