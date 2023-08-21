

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("ViewForm")]
    public class SqlViewForm
    {

        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string type { get; set; } = "";
        public string data { get; set; } = "";
        public bool isdeleted { get; set; } = false;
    }
}
