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
                SqlService? service = context.services!.Where(s => s.code.CompareTo("service1") == 0).FirstOrDefault();
                if (service == null)
                {
                    SqlService item = new SqlService();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "service1";
                    item.name = "Nước sinh hoạt hộ gia đình";
                    item.des = "Nước sinh hoạt hộ gia đình";
                    item.isdeleted = false;
                    context.services!.Add(item);
                }
                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> createAsync(string code, string name, string des)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlService? service = context.services!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0)).FirstOrDefault();
                if (service != null)
                {
                    return false;
                }

                SqlService item = new SqlService();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                context.services!.Add(item);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> editAsync(string code, string name, string des)
        {
            using (DataContext context = new DataContext())
            {
                SqlService? service = context.services!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0)).FirstOrDefault();
                if (service == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    service.des = des;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    service.name = name;
                }
               

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> deleteAsync(string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlService? service = context.services!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0)).FirstOrDefault();
                if (service == null)
                {
                    return false;
                }

                service.isdeleted = true;

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
