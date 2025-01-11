using ArchOp.Models;

namespace ArchOp.ViewModels
{
    internal class SetCompanyViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public string CompanyName { get; set; }
        public string CompanyAddress {  get; set; }
        public SetCompanyViewModel(NavStore navStore)
        {
            this.navStore = navStore;
        }


        public async void SetUserCompany()
        {
            // add some logic?
            if (await DBRequests.IsUserAlreadyPaired())
            {
                System.Windows.MessageBox.Show("Company already set, unable to change contact the administrator");
                return;
            }
            await DBRequests.PairUserCompanyWithUser(CompanyName,CompanyAddress);
        }

    }
}
