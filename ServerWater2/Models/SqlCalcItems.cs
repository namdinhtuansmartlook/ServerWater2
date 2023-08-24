using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("CalcItems")]
    public class SqlCalcItems
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string des { get; set; } = "";
        public string unit { get; set; } = "";
        public bool isdeleted { get; set; } = false;
    }
}
