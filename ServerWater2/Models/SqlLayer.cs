using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Layer")]
    public class SqlLayer
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string nameLayer { get; set; } = "";
        public string des { get; set; } = "";
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public List<SqlDevice>? devices { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
