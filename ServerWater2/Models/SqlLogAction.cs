using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("LogAction")]
    public class SqlLogAction
    {
        [Key]
        public long ID { get; set; }
        public SqlOrder? order { get; set; }
        public SqlState? state { get; set; }
        public SqlUser? user { get; set; }
        public DateTime time { get; set; }
        public string note { get; set; } = "";
    }
}
