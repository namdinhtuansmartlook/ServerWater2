using Microsoft.EntityFrameworkCore;
using ServerWater2.Models;
using static ServerWater2.APIs.MyUser;

namespace ServerWater2.APIs
{
    public class MyGroup
    {
        public MyGroup()
        {
        }

        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo("NK") == 0).FirstOrDefault();
                if (group == null)
                {
                    SqlGroup item = new SqlGroup();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "NK";
                    item.name = "Ninh Kiều";
                    item.des = "Ninh Kiều";
                    item.isdeleted = false;
                    context.groups!.Add(item);
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
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (group != null)
                {
                    return false;
                }

                SqlGroup item = new SqlGroup();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                context.groups!.Add(item);

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
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (group == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    group.des = des;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    group.name = name;
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
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (group == null)
                {
                    return false;
                }
                if(group.areas != null)
                {
                    foreach(SqlArea m_area in group.areas)
                    {
                        m_area.isdeleted = true;
                    }
                }

                group.isdeleted = true;

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

        public class MyItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public class ItemGroup
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public List<MyItemArea> areas { get; set; } = new List<MyItemArea>();
        }

        public List<ItemGroup> getListGroup()
        {
            List<ItemGroup> list = new List<ItemGroup>();
            using (DataContext context = new DataContext())
            {
                List<SqlGroup>? groups = context.groups!.Where(s => s.isdeleted == false).Include(s => s.areas).ToList();
                if (groups.Count > 0)
                {
                    foreach (SqlGroup item in groups)
                    {
                        ItemGroup tmp = new ItemGroup();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.des = item.des;
                        if(item.areas != null)
                        {
                            foreach(SqlArea area in item.areas)
                            {
                                MyItemArea m_area = new MyItemArea();
                                m_area.code = area.code;
                                m_area.name = area.name;
                                m_area.des = area.des;
                                tmp.areas.Add(m_area);
                            }    
                        }  
                       
                        list.Add(tmp);
                    }
                }
                return list;
            }

        }

        public List<MyItemArea> getListArea(string group)
        {
            List<MyItemArea> list = new List<MyItemArea>();
            using (DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(group) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (m_group == null)
                {
                    return new List<MyItemArea>();
                }
                if (m_group.areas == null)
                {
                    return new List<MyItemArea>();
                }

                foreach (SqlArea m_area in m_group.areas)
                {
                    MyItemArea item = new MyItemArea();
                    item.code = m_area.code;
                    item.name = m_area.name;
                    item.des = m_area.des;
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
