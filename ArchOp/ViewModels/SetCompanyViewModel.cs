using ArchOp.Models;

namespace ArchOp.ViewModels
{
    internal class SetCompanyViewModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress {  get; set; }
        public SetCompanyViewModel() { }


        public async void SetUserCompany()
        {
            // add some logic?
           if(await DBRequests.GetCompanyByName(CompanyName) == null)
                await DBRequests.AddCompany(CompanyName, CompanyAddress);
            await DBRequests.PairUserCompanyWithUser(CompanyName);
        }

    }
}
