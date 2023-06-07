using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ServerWater2.APIs
{
    public class MyStatus
    {
        public MyStatus() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlStatus? status = context.statuss!.Where(s => s.code.CompareTo("st1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    SqlStatus item = new SqlStatus();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "st1";
                    item.nameStatus = "trạng thái 1";
                    item.isdeleted = false;
                    item.isOnline = false;
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    context.statuss!.Add(item);
                }

                status = context.statuss!.Where(s => s.code.CompareTo("KT") == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    SqlStatus item = new SqlStatus();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "KT";
                    item.nameStatus = "Khởi tạo";
                    item.isdeleted = false;
                    item.isOnline = false;
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    context.statuss!.Add(item);
                }
                status = context.statuss!.Where(s => s.code.CompareTo("TH") == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    SqlStatus item = new SqlStatus();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "TH";
                    item.nameStatus = "Đang thực hiện";
                    item.isdeleted = false;
                    item.isOnline = false;
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    context.statuss!.Add(item);
                }

                status = context.statuss!.Where(s => s.code.CompareTo("HT") == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    SqlStatus item = new SqlStatus();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "HT";
                    item.nameStatus = "Đang thực hiện";
                    item.isdeleted = false;
                    item.isOnline = false;
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    context.statuss!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> createStatus(string token, string code, string name)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlStatus? status = context.statuss!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (status != null)
                {
                    return false;
                }
                status = new SqlStatus();
                status.ID = DateTime.Now.Ticks;
                status.code = code;
                status.nameStatus = name;
                status.isdeleted = false;
                status.createdTime = DateTime.Now.ToUniversalTime();
                status.lastestTime = status.createdTime;
                context.statuss!.Add(status);
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
        public async Task<bool> editStatus(string token, string code, string name)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlStatus? status = context.statuss!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    return false;
                }
                status.nameStatus = name;
                status.lastestTime = DateTime.Now.ToUniversalTime();
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
        public async Task<bool> deleteStatus(string token, string code)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlStatus? status = context.statuss!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (status == null)
                {
                    return false;
                }
                status.isdeleted = true;
                status.lastestTime = DateTime.Now.ToUniversalTime();

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

        public class ItemStatus
        {
            public string code { get; set; } = "";
            public string nameStatus { get; set; } = "";
            public bool isOnline { get; set; }
        }

        public List<ItemStatus> getListStatus()
        {
            using (DataContext context = new DataContext())
            {
                List<ItemStatus> list = new List<ItemStatus>();
                List<SqlStatus> statuses = context.statuss!.Where(s => s.isdeleted == false).ToList();
                if (statuses.Count > 0)
                {
                    foreach (SqlStatus status in statuses)
                    {
                        ItemStatus item = new ItemStatus();
                        item.code = status.code;
                        item.nameStatus = status.nameStatus;
                        item.isOnline = status.isOnline;
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}
