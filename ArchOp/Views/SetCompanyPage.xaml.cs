using System.Windows;
using System.Windows.Controls;
using ArchOp.ViewModels;

namespace ArchOp
{
    /// <summary>
    /// Interaction logic for SetCompanyPage.xaml
    /// </summary>
    public partial class SetCompanyPage : Page
    {
        public SetCompanyPage(NavStore navStore)
        {
            InitializeComponent();
            DataContext = new SetCompanyViewModel(navStore);
        }

        public void SetUserCompanyClick(object sender, RoutedEventArgs e)
        {
            ((SetCompanyViewModel)DataContext).SetUserCompany();
        }
    }
}
