using ArchOp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ArchOp.ViewModels
{
    internal class SentInvoicesViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public ObservableCollection<Invoice> Invoices { get; set; }

        public SentInvoicesViewModel(NavStore navStore)
        {
            this.navStore = navStore;
            Invoices = new ObservableCollection<Invoice>();
            LoadInvoicesAsync();
        }

        private async void LoadInvoicesAsync()
        {

            var userId = App.SupabaseClient.Auth.CurrentUser.Id;
            var fetchedInvoices = await DBRequests.GetInvoicesAsync(userId);
            foreach (var invoice in fetchedInvoices)
            {
                Invoices.Add(invoice);
            }
        
        }

        public static async void DownloadInvoiceCommand(Invoice invoice)
        {
            if (invoice != null)
            {
                string path = $"{invoice.InvoiceId}.pdf";
                var f = File.Create(path);
                try
                {
                    var bytes = await App.SupabaseClient.Storage.From("invoices").DownloadPublicFile($"invoicespdf/{invoice.InvoiceId}.pdf");
                    f.Write(bytes, 0, bytes.Length);
                }
                catch(Exception e)
                {
                    throw e;
                }
                finally
                {
                    f.Close();   
                }
                MessageBox.Show("Invoice downloaded succesfully.");
            }
            
        }

    }
}
