using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ArchOp.ViewModels
{
    internal class AddCompanyViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }

        private RelayCommand addCompanyCommand;
        public ICommand AddCompanyCommand => addCompanyCommand ??= new RelayCommand(AddCompanyButton);

        public AddCompanyViewModel(NavStore navStore)
        {
            this.navStore = navStore;

        }

        public async void AddToUserAddedCompanies()
        {

            if (!await DBRequests.AlreadyAddedToUserCompanies(CompanyName))
            {
                int? companyId = null;
                try
                {
                    companyId = (await DBRequests.GetCompanyByName(CompanyName)).CompanyId;
                }
                catch
                {
                    if (companyId == null)
                    {
                        var company = await DBRequests.AddCompany(CompanyName, CompanyAddress);
                        companyId = company.CompanyId;
                        System.Windows.MessageBox.Show($"{CompanyName} added to the database.");

                    }
                }
                finally
                {
                    await DBRequests.AddToUserAddedCompanies(companyId.ToString());
                    System.Windows.MessageBox.Show($"{CompanyName} added to your companies.");
                }
            }

        }

        private async void AddCompanyButton()
        {
            AddToUserAddedCompanies();
            navStore.CurrentViewModel = new HomePageViewModel(navStore);
        }
    }
}
