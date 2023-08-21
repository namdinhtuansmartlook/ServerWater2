using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyAction
    {
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlAction? action = context.actions!.Where(s => s.code.CompareTo("XN") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "XN";
                    item.name = "Đã xác nhận";
                    item.des = "Đã xác nhận";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("DXN") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DXN";
                    item.name = "Đợi xác nhận";
                    item.des = "Đợi xác nhận";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("DTN") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DTN";
                    item.name = "Đã tiếp nhận";
                    item.des = "Đã tiếp nhận";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("TKH") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "TKH";
                    item.name = "Đã tạo khách hàng";
                    item.des = "Đã tạo khách hàng";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("PCKS") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "PCKS";
                    item.name = "Đã phân công khảo sát";
                    item.des = "Đã phân công khảo sát";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("KS") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "KS";
                    item.name = "Đang khảo sát";
                    item.des = "Đang khảo sát";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("HTKS") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "HTKS";
                    item.name = "Hoàn thành khảo sát";
                    item.des = "Hoàn thành khảo sát";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("DPC") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DPC";
                    item.name = "Đã phân công thi công";
                    item.des = "Đã phân công thi công";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("DH") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DH";
                    item.name = "Đã hủy";
                    item.des = "Đã hủy";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("NT") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "NT";
                    item.name = "Nghiệm thu";
                    item.des = "Nghiệm thu";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("ABDCV") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "ABDCV";
                    item.name = "Hình ảnh bắt đầu công việc";
                    item.des = "Hình ảnh bắt đầu công việc";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("KT") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "KT";
                    item.name = "Kết thúc công việc";
                    item.des = "Kết thúc công việc";
                    item.isdeleted = false;
                    context.actions!.Add(item);
                }

                action = context.actions!.Where(s => s.code.CompareTo("AKTCV") == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    SqlAction item = new SqlAction();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "HKTCV";
                    item.name = "Hình ảnh kết thúc công việc";
                    item.des = "Hình ảnh kết thúc công việc";
                    item.isdeleted = false;
                    context.actions!.Add(item);
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
                SqlAction? action = context.actions!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (action != null)
                {
                    return false;
                }

                SqlAction item = new SqlAction();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                context.actions!.Add(item);

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
                SqlAction? action = context.actions!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    action.des = des;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    action.name = name;
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
                SqlAction? action = context.actions!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (action == null)
                {
                    return false;
                }

                action.isdeleted = true;

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
        public class ItemAction
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemAction> getListAction()
        {
            List<ItemAction> list = new List<ItemAction>();
            using (DataContext context = new DataContext())
            {
                List<SqlAction>? actions = context.actions!.Where(s => s.isdeleted == false).ToList();
                if (actions.Count > 0)
                {
                    foreach (SqlAction item in actions)
                    {
                        ItemAction tmp = new ItemAction();
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
