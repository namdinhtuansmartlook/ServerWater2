/*using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GIS.Models
{
    [Table("LogDevice")]
    public class SqlLogDevice
    {
        [Key]
        public long ID { get; set; }
        public SqlPoint? point { get; set; }
        public SqlDevice? device { get; set; }
        public SqlSchedule? schedule { get; set; }
        public SqlUser? user { get; set; }
        public List<string>? images { get; set; }
        public string note { get; set; } = "";
        public DateTime timeDo { get; set; }
        public DateTime timeRef { get; set; }
    }
}
*/