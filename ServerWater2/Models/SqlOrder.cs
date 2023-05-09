using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{

    [Table("Order")]
    public class SqlOrder
    {
        [Key]
        public long ID { get; set; }
        public SqlUser? user { get; set; }
        public SqlCustomer? customer { get; set; }
        public string code { get; set; } = "";
        public string note { get; set; } = "";
        public List<string>? images { get; set; }
        public SqlType? type { get; set; }
        public SqlService? service { get; set; }
        public SqlState? state { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public bool isDelete { get; set; } = false;
    }
}
