using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ArchOp.Controllers;
using ArchOp.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


namespace ArchOp
{
    /// <summary>
    /// Interaction logic for InvoiceMakerPage.xaml
    /// </summary>
    public partial class InvoiceMakerPage : Page
    {
        List<TextBox> tBox;
        public InvoiceMakerPage()
        {
            InitializeComponent();
            tBox = [TextBox1,TextBox2,TextBox3,TextBox4];
           
        }

        private async void CreateInvoiceClick(object sender, RoutedEventArgs e)
        {
            string pdfPath = $"{await InvoicesControllers.GetNewInvoiceId()}.pdf";

            await App.SupabaseClient.Storage.From("invoices").Upload(createPDF(), $"invoicespdf/{pdfPath}");
            await App.SupabaseClient.From<Invoice>().Insert(new Invoice 
            { 
                InvoiceId = Convert.ToInt32(pdfPath.Split(".")[0]),
                UserId = App.SupabaseClient.Auth.CurrentUser.Id
            });


        }

        private byte[] createPDF()
        {
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            PdfWriter writer = new PdfWriter(byteArrayOutputStream);
            PdfDocument pdfDocument = new PdfDocument(writer);
            Document document = new Document(pdfDocument);

            //Write the file content
            foreach (var textBox in tBox)
            {
                document.Add(new iText.Layout.Element.Paragraph(textBox.Text));
            }
            document.Close();
            return byteArrayOutputStream.ToArray();
        }

    }
}
