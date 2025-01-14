using System.Windows.Controls;
using ArchOp.ViewModels;


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
