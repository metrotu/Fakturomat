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
    public partial class InvoiceMakerPage : Page
    {
        public InvoiceMakerPage(NavStore navStore)
        {
            InitializeComponent();
            DataContext = new InvoiceMakerViewModel(navStore);
        }
       
    }
}
