using System.Windows;
using System.Windows.Controls;
using ArchOp.ViewModels;

namespace ArchOp
{
    /// <summary>
    /// Logika interakcji dla klasy AddCompanyPage.xaml
    /// </summary>
    public partial class AddCompanyPage : Page
    {
        public AddCompanyPage(NavStore navStore)
        {
            InitializeComponent();
            DataContext = new AddCompanyViewModel(navStore);
        }

        private void AddUserCompanyClick(object sender, RoutedEventArgs e)
        {
            ((AddCompanyViewModel)DataContext).AddToUserAddedCompanies();
        }
    }
}
