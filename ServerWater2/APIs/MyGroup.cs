using Microsoft.EntityFrameworkCore;
using ServerWater2.Models;
using static ServerWater2.APIs.MyArea;

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
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
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
                SqlGroup? group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.users).Include(s => s.areas).FirstOrDefault();
                if (group == null)
                {
                    return false;
                }
                if(group.users != null)
                {
                    foreach(SqlUser m_user in group.users)
                    {
                        m_user.group = null;
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
                List<SqlGroup>? groups = context.groups!.Where(s => s.isdeleted == false).Include(s => s.areas).Include(s => s.users).ToList();
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
                                if(area.isdeleted == false)
                                {
                                    MyItemArea m_area = new MyItemArea();
                                    m_area.code = area.code;
                                    m_area.name = area.name;

                                    tmp.areas.Add(m_area);
                                }    
                            }    
                        }  
                        list.Add(tmp);
                    }
                }
                return list;
            }
        }

        public async Task<bool> addArea(string code, string area)
        {
            using(DataContext context = new DataContext())
            {
                SqlArea? m_area = context.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_area == null)
                {
                    return false;
                }
                
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if(m_group == null)
                {
                    return false;
                }
                if (m_group.areas == null)
                {
                    m_group.areas = new List<SqlArea>();
                }
                else
                {
                    SqlArea? tmp = m_group.areas.Where(s => s.ID == m_area.ID).FirstOrDefault();
                    if (tmp != null)
                    {
                        return false;
                    }
                }    

                m_group.areas.Add(m_area);

                int rows = await context.SaveChangesAsync();
                if(rows > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }    
            }    
        }

        public async Task<bool> RemoveArea(string code, string area)
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? m_area = context.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_area == null)
                {
                    return false;
                }

                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (m_group == null)
                {
                    return false;
                }
                if (m_group.areas == null)
                {
                    return false;
                }
                
                m_group.areas.Remove(m_area);

                int rows = await context.SaveChangesAsync();
                return true;
            }
        }

        public List<MyItemArea> getListArea(string code)
        {
            List<MyItemArea> items = new List<MyItemArea>();

            using(DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if(m_group == null)
                {
                    return  new List<MyItemArea>();
                }    
                if(m_group.areas == null)
                {
                    return new List<MyItemArea>();
                }    

                foreach(SqlArea item in m_group.areas)
                {
                    MyItemArea area = new MyItemArea();
                    area.code = item.code;
                    area.name = item.name;

                    items.Add(area);
                }
            }
            return items;
        }
    }
}
