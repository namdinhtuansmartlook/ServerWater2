using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{

    [Table("Order")]
    public class SqlOrder
    {
        [Key]
        public long ID { get; set; }
        public SqlUser? receiver { get; set; }
        public SqlUser? manager { get; set; }
        public SqlUser? worker { get; set; }
        public SqlCustomer? customer { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string phone { get; set; } = "";
        public SqlGroup? group { get; set; }
        public SqlArea? area { get; set; }
        public string addressWater { get; set; } = "";
        public string addressCustomer { get; set; } = "";
        public string addressContract { get; set; } = "";
        public SqlService? service { get; set; }
        public SqlType? type { get; set; }
        public SqlState? state { get; set; }
        public List<SqlCertificate>? certificates { get; set; }
        public string document { get; set; } 
        public string note { get; set; } = "";
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public bool isFinish { get; set; } = false;
        public bool isDelete { get; set; } = false;
    }
}
