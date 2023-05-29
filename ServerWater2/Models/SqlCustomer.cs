using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("Customer")]
    public class SqlCustomer
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string route { get; set; } = "";
        public string phone { get; set; } = "";
        public string name { get; set; } = "";
        public string address { get; set; } = "";
        public string note { get; set; } = "";
        public List<string>? images { get; set; }
        public string longitude { get; set; } = "";
        public string latitude { get; set; } = "";
       public List<SqlOrder>? orders { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
