using ArchOp.Models;
using ArchOp.ViewModels;
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
using System.Windows.Shapes;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy SentInvoicesWindow.xaml
    /// </summary>
    public partial class SentInvoicesWindow : UserControl
    {
        public SentInvoicesWindow()
        {
            InitializeComponent();
        }

        private async void DownloadClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var invoice = await DBRequests.GetInvoiceById(Convert.ToInt32(b.Content));
            SentInvoicesViewModel.DownloadInvoiceCommand(invoice);

        }
    }
}
