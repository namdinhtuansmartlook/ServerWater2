using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("LogOrder")]
    public class SqlLogOrder
    {
        [Key]
        public long ID { get; set; }
        public SqlOrder? order { get; set; }
        public SqlUser? user { get; set; }
        public SqlAction? action { get; set; } 
        public DateTime time { get; set; }
        public string note { get; set; } = "";
        public List<string>? images { get; set; }
        public string latitude { get; set; } = "";
        public string longitude { get; set; } = "";
    }
}
