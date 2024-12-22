using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ArchOp.Models
{
    [Table("Invoices")]
    internal class Invoice : BaseModel
    {
        [PrimaryKey("InvoiceId")]
        public int InvoiceId { get; set; }
        [Column("UserId")]
        public string UserId { get; set; }


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

    }


}

