using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("Group")]
    public class SqlGroup
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string des { get; set; } = "";

        public List<SqlUser>? users { get; set; }
        public List<SqlArea>? areas { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
