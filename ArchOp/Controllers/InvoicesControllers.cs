using ArchOp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.Controllers
{
    internal static class InvoicesControllers
    {
        public static async Task<int> GetNewInvoiceId()
        {

            // Fetch the highest InvoiceId (assuming InvoiceId is numeric)
            var response = await App.SupabaseClient
                .From<Invoice>() // Replace with your table model
                .Select("InvoiceId")
                .Order("InvoiceId", Supabase.Postgrest.Constants.Ordering.Descending)
                .Range(0,0)
                .Single(); // Fetch the single top row

            // Extract the InvoiceId and increment
            return response != null ? response.InvoiceId + 1 : 0;
        
        }


    }
}
