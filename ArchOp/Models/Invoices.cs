using Microsoft.Win32;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.IO;
using System.Windows;

namespace ArchOp.Models
{
    [Table("Invoices")]
    internal class Invoice : BaseModel
    {
        [PrimaryKey("InvoiceId")]
        public int InvoiceId { get; set; }
        [Column("UserId")]
        public string UserId { get; set; }
        [Column("InvoiceYear")]
        public string? InvoiceYear { get; set; }
        [Column("InvoiceUserId")]
        public int? InvoiceUserId { get; set; }
        public string InvoiceDisplayName { get=> $"{InvoiceUserId} / {InvoiceYear}"; }

    }


}

