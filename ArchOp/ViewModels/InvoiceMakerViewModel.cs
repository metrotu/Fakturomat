using ArchOp.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Windows;
using System.Windows.Controls;

namespace ArchOp.ViewModels
{
    internal class InvoiceMakerViewModel
    {

        //text tu idzie trzeba obmyslec w jaki sposob ma robic fakture
        public string CompanyNameAddressCustomer { get; set; }
        public string OwnCompanyNameAddress { get; set; }
        public string SummaryDescription { get; set; }
        public string DateOfSupply { get; set; }


        public InvoiceMakerViewModel()
        {
        }



        public async void SendInvoice()
        {
            string pdfPath = $"{await Invoice.GetNewInvoiceId()}.pdf";
            
            var pdfData = createPDF();

            await App.SupabaseClient.Storage.From("invoices").Upload(pdfData, $"invoicespdf/{pdfPath}");
            await App.SupabaseClient.From<Invoice>().Insert(new Invoice
            {
                InvoiceId = Convert.ToInt32(pdfPath.Split(".")[0]),
                UserId = App.SupabaseClient.Auth.CurrentUser.Id
            });
            MessageBox.Show("Invoice has been sent.");
        }

        private byte[] createPDF()
        {
            ByteArrayOutputStream byteArrayOutputStream = new();
            PdfWriter writer = new(byteArrayOutputStream);
            PdfDocument pdfDocument = new(writer);
            Document document = new(pdfDocument);

            List<string> data = [CompanyNameAddressCustomer,OwnCompanyNameAddress,SummaryDescription,DateOfSupply];

            //Write the file content
            foreach (var text in data)
            {
                document.Add(new iText.Layout.Element.Paragraph(text));
            }
            document.Close();
            return byteArrayOutputStream.ToArray();
        }

    }
}
