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
        [Column("InvoiceYear")]
        public string? InvoiceYear { get; set; }
    
    
    }


}

