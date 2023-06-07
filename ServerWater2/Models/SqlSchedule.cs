using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Schedule")]
    public class SqlSchedule
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string des { get; set; } = "";
        public string period { get; set; } = "";
        public string note { get; set; } = "";
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public bool isdeleted { get; set; } = false;

    }
}
