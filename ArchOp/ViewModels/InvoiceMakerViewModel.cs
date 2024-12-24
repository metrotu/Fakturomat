using ArchOp.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Windows;

namespace ArchOp.ViewModels
{
    internal class InvoiceMakerViewModel
    {

        //text tu idzie trzeba obmyslec w jaki sposob ma robic fakture
        public string CompanyNameAddressCustomer { get; set; }
        public string OwnCompanyNameAddress { get; set; }
        public string SummaryDescription { get; set; }
        public DateTime DueDate { get; set; }
        public double Total { get; set; }

        private DateTime invoiceDate { get => DateTime.Now; }

        public List<Item> InvoiceItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; }


        public string TTotal { get => InvoiceItems.Count != 0 ? Item.Total(InvoiceItems).ToString():"0.0"; }

        public InvoiceMakerViewModel()
        {
            InvoiceItems = [];
        }



        public async void SendInvoice()
        {
            string pdfPath = $"{await Invoice.GetNewInvoiceId()}.pdf";

            var pdfData = CreatePDF();

            await App.SupabaseClient.Storage.From("invoices").Upload(pdfData, $"invoicespdf/{pdfPath}");
            await App.SupabaseClient.From<Invoice>().Insert(new Invoice
            {
                InvoiceId = Convert.ToInt32(pdfPath.Split(".")[0]),
                UserId = App.SupabaseClient.Auth.CurrentUser.Id
            });
            MessageBox.Show("Invoice has been sent.");
        }

        private byte[] CreatePDF()
        {
            ByteArrayOutputStream byteArrayOutputStream = new();
            PdfWriter writer = new(byteArrayOutputStream);
            PdfDocument pdfDocument = new(writer);
            Document document = new(pdfDocument);

            List<string> data = [CompanyNameAddressCustomer, OwnCompanyNameAddress, SummaryDescription, invoiceDate.Date.ToString(), DueDate.Date.ToString()];

            //Write the file content
            foreach (var text in data)
            {
                document.Add(new iText.Layout.Element.Paragraph(text));
            }

            document.Close();
            return byteArrayOutputStream.ToArray();
        }

        public void AddItem()
        {
            try
            {
                InvoiceItems.Add(new(Name, Description, Convert.ToDouble(Price), Convert.ToInt32(Quantity)));
                
            
            }
            catch 
            {
                MessageBox.Show("Invalid data types for item.", "Warning");
            }
        }
    }
}

