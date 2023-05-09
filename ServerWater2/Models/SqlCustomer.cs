using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater2.Models
{
    [Table("Customer")]
    public class SqlCustomer
    {
        [Key]
        public long ID { get; set; }
        public string idKH { get; set; } = "";
        public string maDB { get; set; } = "";
        public string sdt { get; set; } = "";
        public string tenKH { get; set; } = "";
        public string diachiTT { get; set; } = "";
        public string diachiLH { get; set; } = "";
        public string diachiLD { get; set; } = "";
        //public int sonk { get; set; } = 0;
        //public string serialmodule { get; set; } = "";
        //public string serialdh { get; set; } = "";
        public string longitude { get; set; } = "";
        public string latitude { get; set; } = "";
       // public SqlArea? area { get; set; }
       public List<SqlUser>? users { get; set; }
        public bool isdeleted { get; set; } = false;
    }
}
