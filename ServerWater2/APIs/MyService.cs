using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyService
    {
        public MyService()
        {

        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlService? service = context.services!.Where(s => s.code.CompareTo("LM") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "LM";
                    item.name = "Lắp mới";
                    item.des = "Lắp mới";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                service = context.services!.Where(s => s.code.CompareTo("SC") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "SC";
                    item.name = "Sửa chữa";
                    item.des = "Sửa chữa";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                service = context.services!.Where(s => s.code.CompareTo("TT") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "TT";
                    item.name = "Thay thế";
                    item.des = "Thay thế";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                service = context.services!.Where(s => s.code.CompareTo("ST") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "ST";
                    item.name = "Sang tên";
                    item.des = "Sang tên";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                service = context.services!.Where(s => s.code.CompareTo("DKDM") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DKDM";
                    item.name = "Đăng kí định mức";
                    item.des = "Đăng kí định mức";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                int rows = await context.SaveChangesAsync();
            }
        }
        public class ItemService
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemService> getListService()
        {
            List<ItemService> list = new List<ItemService>();
            using (DataContext context = new DataContext())
            {
                List<SqlService>? services = context.services!.Where(s => s.isdeleted == false).ToList();
                if (services.Count > 0)
                {
                    foreach (SqlService item in services)
                    {
                        ItemService tmp = new ItemService();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.des = item.des;

                        list.Add(tmp);
                    }
                }
                return list;
            }
        }
    }
}
