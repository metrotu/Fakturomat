using System.Windows;
using System.Windows.Controls;
using ArchOp.Models;
using ArchOp.ViewModels;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;


namespace ArchOp
{
    /// <summary>
    /// Interaction logic for InvoiceMakerPage.xaml
    /// </summary>
    public partial class InvoiceMakerPage : UserControl
    {
        public InvoiceMakerPage()
        {
            InitializeComponent();
           
        }

        private async void SendInvoiceClick(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            ((Button)sender).IsEnabled = await ((InvoiceMakerViewModel)DataContext).SendInvoice();

        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            ((InvoiceMakerViewModel)DataContext).AddItem();
        }

        

    }
}
