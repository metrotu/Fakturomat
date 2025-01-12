using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ArchOp.Models
{
    [Table("Companies")]
    internal class Company : BaseModel
    {
        [PrimaryKey("CompanyId")]
        public int CompanyId {  get; set; }
        [Column("CompanyName")]
        public string CompanyName { get; set; }
        [Column("CompanyAddress")]
        public string CompanyAddress { get; set; }


    }
}
