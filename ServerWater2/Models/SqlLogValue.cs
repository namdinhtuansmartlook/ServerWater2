using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{

    [Table("LogValue")]
    public class SqlLogValue
    {
        [Key]
        public long ID { get; set; }
        public SqlDevice? device { get; set; }
        public SqlValue? valueConfig { get; set; }
        public string value { get; set; } = "";
        public DateTime time { get; set; }
    }
}
