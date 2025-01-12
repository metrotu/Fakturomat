using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public static async Task<IEnumerable<int>?> GetUserAddedCompaniesIdAsync()
        {

            var h = await App.SupabaseClient
              .From<Users>()
              .Select("UserAddedCompaniesId, SupabaseUserId")
              .Where(x => x.SupabaseUserId == App.SupabaseClient.Auth.CurrentUser.Id)
              .Single();

            if (h == null || h.UserAddedCompaniesId == null || h.UserAddedCompaniesId.Length == 0)
                return null;
        
            return h.UserAddedCompaniesId.Split(',').Select(x => Convert.ToInt32(x));
        
        }


        //Retrieve all companies that the user has added, CompanyName and CompanyAddress are exposed,
        //but you can expose more fields if needed
        public static async Task<List<Company?>> GetUserAddedCompaniesAsync()
        {
            var h = await GetUserAddedCompaniesIdAsync();
            if (h == null)
            {
                return null;
            }
            List<Company> companies = [];
            foreach (var companyId in h)
            {
                var res = await GetCompanyById(companyId);
                if (res != null)
                {
                    companies.Add(res);
                }
            }

            return companies;
        }



        public static async Task<IEnumerable<Invoice>> GetInvoicesAsync(string userId)
        {
            var response = await App.SupabaseClient
                .From<Invoice>()
                .Select("InvoiceId,InvoiceYear")
                .Where(x => x.UserId == userId)
                .Get();

            return response.Models;
        }

        public static async Task<Invoice> GetInvoiceById(int id)
        {
            var response = await App.SupabaseClient
                .From<Invoice>()
                .Select("InvoiceId,InvoiceYear")
                .Where(x => x.UserId == App.SupabaseClient.Auth.CurrentUser.Id && x.InvoiceId == id)
                .Single();
            return response;
        }

        public static async Task<Company> AddCompany(string companyName, string companyAddress)
        {
            await App.SupabaseClient
                .From<Company>()
                .Insert(new Company { CompanyName = companyName, CompanyAddress = companyAddress});
            return await App.SupabaseClient
                .From<Company>()
                .Select("CompanyId")
                .Where(x => x.CompanyName == companyName)
                .Single();
        }

        public static async Task PairUserCompanyWithUser(string companyName, string companyAddress)
        {
            var company = await GetCompanyByName(companyName);
            if (company == null)
            {
                company = await AddCompany(companyName, companyAddress);
            }
            string userId = App.SupabaseClient.Auth.CurrentSession.User.Id;
            var h = new Users { SupabaseUserId = userId, UserCompanyId = company.CompanyId, UserAddedCompaniesId = "" };
            await App.SupabaseClient
                .From<Users>()
                .Insert(h);

        }

        public static async Task AddToUserAddedCompanies(string companyId)
        {
            Users? user = await App.SupabaseClient
                .From<Users>()
                .Select("SupabaseUserId, UserAddedCompaniesId")
                .Where(x => x.SupabaseUserId == App.SupabaseClient.Auth.CurrentUser.Id)
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
                currUserAddedCompanies = $"{currUserAddedCompanies},{companyId}";
            }

            await App.SupabaseClient
               .From<Users>()
               .Where(x => x.SupabaseUserId == user.SupabaseUserId)
               .Set(x => x.UserAddedCompaniesId, currUserAddedCompanies)
               .Update();
        }

        public static async Task<Company?> GetCompanyByName(string companyName)
        {
            return await App.SupabaseClient
                .From<Company>()
                .Select("CompanyId, CompanyName, CompanyAddress")
                .Where(x => x.CompanyName == companyName)
                .Single();
        }

        //here we can chose what to expose from a company
        public static async Task<Company?> GetCompanyById(int companyId)
        {
            return await App.SupabaseClient
                .From<Company>()
                .Select("CompanyName, CompanyAddress") 
                .Where(x => x.CompanyId == companyId)
                .Single();
        }

        public static async Task<bool> AlreadyAddedToUserCompanies(string companyName)
        {
            var h = await GetUserAddedCompaniesAsync();
            return h != null && h.Any(x => x.CompanyName == companyName);
        }

        public static async Task<bool> IsUserAlreadyPaired()
        {
            return (await App.SupabaseClient
                .From<Users>()
                .Select("UserCompanyId")
                .Where(x => x.SupabaseUserId == App.SupabaseClient.Auth.CurrentUser.Id)
                .Get()).Models.Count != 0;
        }

        public static async Task<Company> GetOwnCompany()
        {
            var id = (await App.SupabaseClient
                .From<Users>()
                .Select("UserCompanyId")
                .Where(x => x.SupabaseUserId == App.SupabaseClient.Auth.CurrentUser.Id)
                .Single()).UserCompanyId;
            
            return await GetCompanyById(id.Value);

        }

    }
}
