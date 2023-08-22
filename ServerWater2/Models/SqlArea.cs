using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("Area")]
    public class SqlArea
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string des { get; set; } = "";
        public SqlGroup? group { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
