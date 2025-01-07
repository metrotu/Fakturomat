using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ArchOp.Models
{
    [Table("Users")]
    internal class Users : BaseModel
    {
        [PrimaryKey("UserId")]
        public string UserId { get; set; }

        [Column("UserCompanyId")]
        public int UserCompanyId { get; set; }

        [Column("UserAddedCompanies")]
        public string? UserAddedCompanies { get; set; }
        [Column("UserSupabaseId")]
        public string UserSupabaseId { get; set; }
    }
}
