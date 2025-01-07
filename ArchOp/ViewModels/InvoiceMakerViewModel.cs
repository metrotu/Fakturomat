﻿using ArchOp.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace ArchOp.ViewModels
{
    internal class InvoiceMakerViewModel : INotifyPropertyChanged
    {
        public string CompanyNameAddressCustomer { get; set; }
        public string OwnCompanyNameAddress { get; set; }
        public string SummaryDescription { get; set; }
        public DateTime DueDate { get; set; }
        public ObservableCollection<Item> InvoiceItems { get; set; }

        private DateTime InvoiceDate => DateTime.Now;

        public string Name {  get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get => Quantity * Price; }

        public InvoiceMakerViewModel()
        {
            InvoiceItems = [];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task<bool> SendInvoice()
        {
            string pdfPath = $"{await DBRequests.GetNewInvoiceId()}.pdf";
            System.Windows.MessageBox.Show("Invoice is being made.");
            
            var pdfData = await CreatePDF();

            await App.SupabaseClient.Storage.From("invoices").Upload(pdfData, $"invoicespdf/{pdfPath}");
            await App.SupabaseClient.From<Invoice>().Insert(new Invoice
            {
                InvoiceId = Convert.ToInt32(pdfPath.Split(".")[0]),
                UserId = App.SupabaseClient.Auth.CurrentUser.Id
            });

            System.Windows.MessageBox.Show("Invoice has been sent.");
            return true;
        }

        private async Task<byte[]> CreatePDF()
        {
            ByteArrayOutputStream byteArrayOutputStream = new();
            PdfWriter writer = new(byteArrayOutputStream);
            PdfDocument pdfDocument = new(writer);
            Document document = new(pdfDocument);

            // Adding the header
            Paragraph header = new Paragraph("INVOICE")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(20)
                .SimulateBold();
            document.Add(header);

            // Adding company and customer details
            Table detailsTable = new Table(2).UseAllAvailableWidth();
            detailsTable.AddCell(new Cell().Add(new Paragraph(OwnCompanyNameAddress)).SetBorder(Border.NO_BORDER));
            detailsTable.AddCell(new Cell().Add(new Paragraph($"Invoice # 123456\nInvoice Date: {InvoiceDate:MM/dd/yyyy}\nDue Date: {DueDate:MM/dd/yyyy}"))
                .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

            detailsTable.AddCell(new Cell().Add(new Paragraph(CompanyNameAddressCustomer)).SetMarginTop(20).SetBorder(Border.NO_BORDER));
            detailsTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            document.Add(detailsTable);

            // Adding the item table
            Table itemTable = new Table(new float[] { 3, 6, 2, 2, 2 }).UseAllAvailableWidth().SetMarginTop(20);
            itemTable.AddHeaderCell("Item");
            itemTable.AddHeaderCell("Description");
            itemTable.AddHeaderCell("Unit Price");
            itemTable.AddHeaderCell("Quantity");
            itemTable.AddHeaderCell("Amount");

            double subtotal = 0;

            foreach (var item in InvoiceItems)
            {
                itemTable.AddCell(item.Name);
                itemTable.AddCell(item.Description);
                itemTable.AddCell(item.Price.ToString("F2"));
                itemTable.AddCell(item.Quantity.ToString());
                double amount = item.Price * item.Quantity;
                subtotal += amount;
                itemTable.AddCell(amount.ToString("F2"));
            }

            document.Add(itemTable);

            // Adding summary section
            Table summaryTable = new Table(2).UseAllAvailableWidth().SetMarginTop(20);
            summaryTable.AddCell(new Cell().Add(new Paragraph("Subtotal:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(subtotal.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            double total = subtotal; // Add any discounts or taxes here if needed
            summaryTable.AddCell(new Cell().Add(new Paragraph("Total:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(total.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            summaryTable.AddCell(new Cell().Add(new Paragraph("Amount Paid:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph("0.00").SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            summaryTable.AddCell(new Cell().Add(new Paragraph("Balance Due:").SimulateBold()).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(total.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT).SimulateBold()).SetBorder(Border.NO_BORDER));

            document.Add(summaryTable);

            // Adding notes
            Paragraph notes = new Paragraph("NOTES: Provide a concise, professional description of the services, product, and discount listed above.")
                .SetFontSize(10).SetMarginTop(20);
            document.Add(notes);

            document.Close();
            return byteArrayOutputStream.ToArray();
        }

        public void AddItem()
        {
            try
            {
                InvoiceItems.Add(new Item(Name, Description, Price, Quantity, TotalPrice));
                OnPropertyChanged(nameof(InvoiceItems));
            }
            catch
            {
                System.Windows.MessageBox.Show("Invalid data types for item.", "Warning");
            }
        }

      
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
