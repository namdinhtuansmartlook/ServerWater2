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
        public string nameArea { get; set; } = "";
        public string des { get; set; } = "";
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public List<SqlUser>? users { get; set; }
        public List<SqlCustomer>? customers { get; set; }
        public List<SqlPoint>? points { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
