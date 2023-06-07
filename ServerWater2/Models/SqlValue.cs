using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Value")]
    public class SqlValue
    {
        public long ID { get; set; }
        public string nameValue { get; set; } = "";
        public string unit { get; set; } = "";
        public string des { get; set; } = "";
        public DateTime createdTime { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
