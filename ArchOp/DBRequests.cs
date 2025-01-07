using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchOp.Models;

namespace ArchOp
{
    internal static class DBRequests
    {
        public static async Task<int> GetNewInvoiceId()
        {

            // Fetch the highest InvoiceId (assuming InvoiceId is numeric)
            var response = await App.SupabaseClient
                .From<Invoice>() // Replace with your table model
                .Select("InvoiceId")
                .Order("InvoiceId", Supabase.Postgrest.Constants.Ordering.Descending)
                .Range(0, 0)
                .Single(); // Fetch the single top row

            // Extract the InvoiceId and increment
            return response != null ? response.InvoiceId + 1 : 0;
        }

        public static async Task<IEnumerable<Invoice>> GetInvoicesAsync(string userId)
        {
            var response = await App.SupabaseClient
                .From<Invoice>()
                .Select("InvoiceId")
                .Where(x => x.UserId == userId)
                .Get();

            return response.Models;
        }

        public static async Task<Invoice> GetInvoiceById(int id)
        {
            var response = await App.SupabaseClient
                .From<Invoice>()
                .Select("InvoiceId")
                .Where(x => x.UserId == App.SupabaseClient.Auth.CurrentUser.Id && x.InvoiceId == id)
                .Single();
            return response;
        }

        public static async Task AddCompany(string companyName, string companyAddress)
        {
            await App.SupabaseClient
                .From<Company>()
                .Insert(new Company { CompanyName = companyName, CompanyAddress = companyAddress});
        }


        public static async Task PairUserCompanyWithUser(string companyName)
        {
            var company = await GetCompanyByName(companyName);
            await App.SupabaseClient
                .From<Users>()
                .Insert(new Users
                {
                    UserSupabaseId = App.SupabaseClient.Auth.CurrentSession.User.Id,
                    UserCompanyId = company.CompanyId
                });
        }

        public static async Task AddToUserAddedCompanies(string supabaseId, string companyId)
        {
            Users? user = await App.SupabaseClient
                .From<Users>()
                .Select("UserSupabaseId")
                .Where(x => x.UserSupabaseId == supabaseId)
                .Single();
            
            if (user == null)
                return;

            var currUserAddedCompanies = user.UserAddedCompaniesId;

            if (currUserAddedCompanies == null || currUserAddedCompanies.Length == 0)
            {
                currUserAddedCompanies = $"{companyId}";
            }
            else
            {
                currUserAddedCompanies = $",{companyId}";
            }

            await App.SupabaseClient
               .From<Users>()
               .Where(x => x.UserSupabaseId == supabaseId)
               .Set(x => x.UserAddedCompaniesId, currUserAddedCompanies)
               .Update();
        }

        public static async Task<Company?> GetCompanyByName(string companyName)
        {
            return await App.SupabaseClient
                .From<Company>()
                .Select("CompanyId")
                .Where(x => x.CompanyName == companyName)
                .Single();
        }

        public static async Task<Company?> GetCompanyById(int companyId)
        {
            return await App.SupabaseClient
                .From<Company>()
                .Select("CompanyId")
                .Where(x => x.CompanyId == companyId)
                .Single();
        }

    }
}
