using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.ViewModels
{
    internal class AddCompanyViewModel : ViewModelBase
    {
        private readonly NavStore navStore;

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }


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



    }
}
