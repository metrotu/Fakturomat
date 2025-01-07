using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArchOp.ViewModels;

namespace ArchOp
{
    /// <summary>
    /// Interaction logic for SetCompanyPage.xaml
    /// </summary>
    public partial class SetCompanyPage : Page
    {
        public SetCompanyPage()
        {
            InitializeComponent();
        }

        public void SetUserCompanyClick(object sender, RoutedEventArgs e)
        {
            ((SetCompanyViewModel)DataContext).SetUserCompany();
        }
    }
}
