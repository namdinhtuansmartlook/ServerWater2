using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater2.Models
{
    [Table("Device")]
    public class SqlDevice
    {
        public long ID { get; set; }
        public string code { get; set; } = "";
        public string nameDevice { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public DateTime createdTime { get; set; }
        public DateTime lastestTime { get; set; }
        public List<SqlPoint>? points { get; set; }
        public List<SqlLayer>? layers { get; set; }
        public DateTime startTimeSChedule { get; set; }
        public SqlType? type { get; set; }
    }
}
