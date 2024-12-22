using ArchOp.Models;
using Supabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.ViewModels
{
    public class SentInvoicesViewModel
    {

        public SentInvoicesViewModel() { }

        void GetSentInvoices()
        {
            var user = App.SupabaseClient.Auth.CurrentUser.Id;
            var invoices = Invoice.GetInvoicesAsync(user);


        }








    }
}
