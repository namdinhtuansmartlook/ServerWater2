using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ServerWater2.Models
{
    [Table("UserRole")]
    public class SqlRole
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string des { get; set; } = "";
        public string note { get; set; } = "";
        public bool isdeleted { get; set; } = false;
    }
}
