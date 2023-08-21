using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyState
    {
        public MyState()
        {

        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlState? type = context.states!.Where(s => s.code == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 0;
                    item.name = "Mới";
                    item.des = "Mới";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }
                type = context.states!.Where(s => s.code == 1).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 1;
                    item.name = "Ghi Nhận";
                    item.des = "Ghi Nhận";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }
                type = context.states!.Where(s => s.code == 2).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 2;
                    item.name = "Đã xác thực";
                    item.des = "Đã xác thực";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 3).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 3;
                    item.name = "Đã phân công khảo sát";
                    item.des = "Đã phân công khảo sát";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 4).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 4;
                    item.name = "Đang khảo sát";
                    item.des = "Đang khảo sát";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 5).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 5;
                    item.name = "Hoàn thành khảo sát";
                    item.des = "Hoàn thành khảo sát";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 3).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 6;
                    item.name = "Đã phân công việc";
                    item.des = "Đã phân công việc";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }


                type = context.states!.Where(s => s.code == 4).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 7;
                    item.name = "Thực Hiện";
                    item.des = "Thực Hiện";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }
                
                type = context.states!.Where(s => s.code == 5).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 8;
                    item.name = "Kết Thúc";
                    item.des = "Kết Thúc";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }
                type = context.states!.Where(s => s.code == 6).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 9;
                    item.name = "Nghiệm Thu";
                    item.des = "Nghiệm Thu";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 7).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 10;
                    item.name = "Đã Hoàn Thành";
                    item.des = "Đã Hoàn Thành";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                type = context.states!.Where(s => s.code == 8).FirstOrDefault();
                if (type == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = 11;
                    item.name = "Hủy";
                    item.des = "Hủy";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> editAsync(int code, string des, string name)
        {
            using (DataContext context = new DataContext())
            {
                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (state == null)
                {
                    return false;
                }
                state.name = name;
                state.des = des;

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


        /* public async Task<bool> createAsync(int code, string name, string des)
         {
             if (string.IsNullOrEmpty(code.ToString()) || string.IsNullOrEmpty(name))
             {
                 return false;
             }
             using (DataContext context = new DataContext())
             {
                 SqlState? type = context.states!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0 || s.name.CompareTo(name) == 0)).FirstOrDefault();
                 if (type != null)
                 {
                     return false;
                 }

                 SqlState item = new SqlState();
                 item.ID = DateTime.Now.Ticks;
                 item.code = code;
                 item.name = name;
                 item.des = des;
                 context.sqlStates!.Add(item);

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
         public async Task<bool> deleteAsync(int code)
         {
             using (DataContext context = new DataContext())
             {
                 SqlState? type = context.sqlStates!.Where(s => s.isdeleted == false && s.code == code).FirstOrDefault();
                 if (type == null)
                 {
                     return false;
                 }

                 type.isdeleted = true;

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
         }*/

        public class ItemStateOrder
        {
            public int code { get; set; } = 0;
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemStateOrder> getList()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlState> states = context.states!.Where(s => s.isdeleted == false).OrderBy(s => s.code).ToList();
                List<ItemStateOrder> items = new List<ItemStateOrder>();
                foreach (SqlState state in states)
                {
                    ItemStateOrder item = new ItemStateOrder();
                    item.code = state.code;
                    item.name = state.name;
                    item.des = state.des;
                    items.Add(item);
                }
                return items;
            }
        }
    }

}
