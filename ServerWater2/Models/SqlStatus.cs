using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Status")]
    public class SqlStatus
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string nameStatus { get; set; } = "";
        public bool isOnline { get; set; } = false;
        public bool isdeleted { get; set; } = false;
        public DateTime lastestTime { get; set; }
        public DateTime createdTime { get; set; }
        public List<SqlType>? types { get; set; }
    }
}
