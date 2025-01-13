using ArchOp.Models;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Security.Policy;
using Microsoft.IdentityModel.Tokens;

namespace ArchOp.ViewModels
{
    internal class InvoiceMakerViewModel : ViewModelBase
    {
        private readonly NavStore navStore;
        public string SummaryDescription { get; set; }
        public DateTime DueDate { get; set; }
        public ObservableCollection<Item> InvoiceItems { get; set; }

        private DateTime InvoiceDate => DateTime.Now;

        private double quantity;
        private double price;

        public string Name {  get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public double TotalPrice { get => Convert.ToDouble(Quantity) * Convert.ToDouble(Price); }

        private string[] availableVat = ["23%", "8%", "5%","0%", "Exempt"];
        public string[] AvailableVat { get => availableVat; }
        private double? selectedVat;
        public string SelectedVat
        {
            get => selectedVat != null ? $"{selectedVat}%" : "Exempt";
            set
            {
                if (value == "Exempt")
                {
                    selectedVat = null;
                }
                else
                {
                    selectedVat = Convert.ToDouble(value.TrimEnd('%'))/100.0;
                }
            }
        }

        public ObservableCollection<Company?> UserCompanies { get; set; }
        public Company SelectedCompany { get; set; }

        public Company OwnCompany { get; set; }

        private RelayCommand addItemCommand;
        public ICommand AddItemCommand => addItemCommand ??= new RelayCommand(AddItemButton);

        private RelayCommand createInvoiceCommand;
        public ICommand CreateInvoiceCommand => createInvoiceCommand ??= new RelayCommand(CreateInvoiceButton);

        public bool IsCreateInvoiceEnabled { get; set; }

        public InvoiceMakerViewModel(NavStore navStore)
        {
            this.navStore = navStore;
            InvoiceItems = [];

            DueDate = DateTime.Now;


            LoadOwnCompanyAsync();
            LoadUserCompaniesAsync();
        }

        public async Task<bool> CreateInvoice()
        {
            if (SelectedCompany == null)
            {
                System.Windows.MessageBox.Show("Please select a company", "Error.");
                return false;
            }


            IsCreateInvoiceEnabled = false;
            OnPropertyChanged(nameof(IsCreateInvoiceEnabled));
            
            int invoiceId;
            var invoiceUserId = await DBRequests.GetInvoiceUserId();
            string pdfPath = $"{invoiceId = await DBRequests.GetNewInvoiceId()}_{InvoiceDate.Year}.pdf";
            System.Windows.MessageBox.Show("Invoice is being made.");
            
            var pdfData = await CreatePDF(invoiceId, invoiceUserId);
            //id_rok.pdf
            /*
            await App.SupabaseClient.Storage.From("invoices").Upload(pdfData, $"{pdfPath}");
            await App.SupabaseClient.From<Invoice>().Insert(new Invoice
            {
                InvoiceId = Convert.ToInt32(pdfPath.Split("_")[0]),
                UserId = App.SupabaseClient.Auth.CurrentUser.Id,
                InvoiceYear = InvoiceDate.Year.ToString(),
                UserInvoiceId = await DBRequests.GetUserInvoiceId()
            });
            */
            await DBRequests.InsertInvoice(pdfData, pdfPath, pdfPath.Split("_")[0],
                App.SupabaseClient.Auth.CurrentUser.Id, InvoiceDate.Year.ToString(), invoiceUserId);



            System.Windows.MessageBox.Show("Invoice has been created.");
            
            IsCreateInvoiceEnabled = true;
            OnPropertyChanged(nameof(IsCreateInvoiceEnabled));

            return true;
        }

        private async Task<byte[]> CreatePDF(int invoiceId, int invoiceUserId)
        {
            //maybe need better protection???
            if (OwnCompany == null)
            {
                System.Windows.MessageBox.Show("Own company details not found.", "Error");
                return null;
            }

            if (SelectedCompany == null)
            {
                System.Windows.MessageBox.Show("Customer company details not found.", "Error");
                return null;
            }


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


            var oName = OwnCompany.CompanyName;
            var oAddress = OwnCompany.CompanyAddress;

            var cName = SelectedCompany.CompanyName;
            var cAddress = SelectedCompany.CompanyAddress;

            // Adding company and customer details
            Table detailsTable = new Table(2).UseAllAvailableWidth();
            detailsTable.AddCell(new Cell().Add(new Paragraph($"{oName} \n {oAddress}")).SetBorder(Border.NO_BORDER));
            detailsTable.AddCell(new Cell().Add(new Paragraph($"Invoice # {invoiceUserId}\nInvoice Date: {InvoiceDate:MM/dd/yyyy}\nDue Date: {DueDate:MM/dd/yyyy}"))
                .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

            detailsTable.AddCell(new Cell().Add(new Paragraph($"{cName} \n {cAddress}")).SetMarginTop(20).SetBorder(Border.NO_BORDER));
            detailsTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            document.Add(detailsTable);

            // Adding the item table
            Table itemTable = new Table(new float[]  {3, 6, 2, 2, 2, 2, 2}).UseAllAvailableWidth().SetMarginTop(20);
            itemTable.AddHeaderCell("Item");
            itemTable.AddHeaderCell("Description");
            itemTable.AddHeaderCell("Unit Price");
            itemTable.AddHeaderCell("Quantity");
            itemTable.AddHeaderCell("Brutto");
            itemTable.AddHeaderCell("Netto");
            itemTable.AddHeaderCell("VAT");



            double brutto = 0;
            double netto = 0;
            foreach (var item in InvoiceItems)
            {
                itemTable.AddCell(item.Name);
                itemTable.AddCell(item.Description);
                itemTable.AddCell(item.Price.ToString("F2"));
                itemTable.AddCell(item.Quantity.ToString());
                var b = item.Brutto();
                var n = item.Netto();
                brutto += b;
                netto += n;
                itemTable.AddCell(b.ToString());
                itemTable.AddCell(n.ToString());
                var s = item.DisplayVat == null ? "Exempt" : $"{item.Vat * 100}%";
                itemTable.AddCell(s);

            }

            document.Add(itemTable);

            // Adding summary section
            Table summaryTable = new Table(2).UseAllAvailableWidth().SetMarginTop(20);
            summaryTable.AddCell(new Cell().Add(new Paragraph("Brutto:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(brutto.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            summaryTable.AddCell(new Cell().Add(new Paragraph("Netto:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(netto.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            summaryTable.AddCell(new Cell().Add(new Paragraph("Amount Paid:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph("0.00").SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));

            summaryTable.AddCell(new Cell().Add(new Paragraph("VAT:")).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph($"{brutto-netto}").SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));



            summaryTable.AddCell(new Cell().Add(new Paragraph("Balance Due:").SimulateBold()).SetBorder(Border.NO_BORDER));
            summaryTable.AddCell(new Cell().Add(new Paragraph(netto.ToString("F2")).SetTextAlignment(TextAlignment.RIGHT).SimulateBold()).SetBorder(Border.NO_BORDER));

            document.Add(summaryTable);

            // Adding notes
            //Paragraph notes = new Paragraph("NOTES: Provide a concise, professional description of the services, product, and discount listed above.")
            //    .SetFontSize(10).SetMarginTop(20);
            //document.Add(notes);

            document.Close();
            return byteArrayOutputStream.ToArray();
        }

        public void AddItem()
        {
            try
            {
                if (Name.IsNullOrEmpty())
                {
                    System.Windows.MessageBox.Show("Added items need to have a name.");
                    return;

                }
                var item = new Item(Name, Description, Convert.ToDouble(Price), Convert.ToDouble(Quantity), TotalPrice, selectedVat);
                InvoiceItems.Add(item);

                IsCreateInvoiceEnabled = true;

                OnPropertyChanged(nameof(IsCreateInvoiceEnabled));
                OnPropertyChanged(nameof(InvoiceItems));
            }
            catch
            {
                System.Windows.MessageBox.Show("Invalid data types for item.", "Warning");
            }
        }

        public async Task LoadUserCompaniesAsync()
        {

            UserCompanies = new(await DBRequests.GetUserAddedCompaniesAsync());
            OnPropertyChanged(nameof(UserCompanies));
        }

        public async Task LoadOwnCompanyAsync()
        {
            
            OwnCompany = await DBRequests.GetOwnCompany();
            // Notify the UI about the property change.
            OnPropertyChanged(nameof(OwnCompany));
            
        }

        private async void CreateInvoiceButton()
        {
            await CreateInvoice();
        }

        private async void AddItemButton()
        {
            AddItem();
        }


    }
}
