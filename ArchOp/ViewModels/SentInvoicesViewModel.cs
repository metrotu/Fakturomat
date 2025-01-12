using ArchOp.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ArchOp.ViewModels
{
    internal class SentInvoicesViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public ObservableCollection<Invoice> Invoices { get; set; }

        private RelayCommand<Invoice> downloadInvoiceCommand;
        public ICommand DownloadInvoiceCommand => downloadInvoiceCommand ??= new RelayCommand<Invoice>(DownloadInvoice);


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

        private async void DownloadInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                MessageBox.Show("Invalid invoice.");
                return;
            }

            await DownloadInvoiceImpl(invoice);


        }

        private static async Task DownloadInvoiceImpl(Invoice invoice)
        {
            if (invoice == null)
            {
                MessageBox.Show("Invalid invoice.");
                return;
            }

            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Folder",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (folderDialog.ShowDialog() == true)
            {
                string selectedPath = folderDialog.FolderName;
                var s = invoice.InvoiceDisplayName.Split("/");
                string fileName = $"{s[0].Trim()}_{s[1].Trim()}.pdf";
                string filePath = Path.Combine(selectedPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    try
                    {
                        var bytes = await App.SupabaseClient.Storage.From("invoices").DownloadPublicFile($"{invoice.InvoiceId}_{invoice.InvoiceYear}.pdf");
                        await fileStream.WriteAsync(bytes, 0, bytes.Length);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Failed to download invoice: {e.Message}");
                    }
                    finally
                    {
                        fileStream.Close();

                    }
                    MessageBox.Show("Invoice downloaded successfully.");

                }
            }
            else
            {
                MessageBox.Show("No folder selected. Download canceled.");
            }
        }

    }
}