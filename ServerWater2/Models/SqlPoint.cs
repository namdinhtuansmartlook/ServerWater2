using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Point")]
    public class SqlPoint
    {
        [Key]
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string namePoint { get; set; } = "";
        public string des { get; set; } = "";
        public string longitude { get; set; } = "";
        public string latitude { get; set; } = "";
        public string note { get; set; } = "";
        public string imageShow { get; set; } = "";
        public List<string>? images { get; set; }
        public bool isdeleted { get; set; } = false;
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public List<SqlArea>? areas { get; set; }
        public List<SqlDevice>? devices { get; set; }
    }
}
