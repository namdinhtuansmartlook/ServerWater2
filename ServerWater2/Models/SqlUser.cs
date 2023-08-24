using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerWater2.Models
{
    [Table("User")]
    public class SqlUser
    {
        [Key]
        public long ID { get; set; }
        public string user { get; set; } = "";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string token { get; set; } = "";
        public string displayName { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public string phoneNumber { get; set; } = "";
        public string des { get; set; } = "";
        public string avatar { get; set; } = "";
        public SqlRole? role { get; set; }
        public List<string>? idToken { get; set; }
        public List<SqlOrder>? receiverOrders { get; set; }
        public List<SqlOrder>? managerOrders { get; set; }
        public List<SqlOrder>? workerOrders { get; set; }
        public List<SqlOrder>? surveyOrders { get; set; }
        //public List<SqlArea>? areas { get; set; }
    }
}
