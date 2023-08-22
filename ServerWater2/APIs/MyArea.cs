using Microsoft.EntityFrameworkCore;
using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyArea
    {
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo("P1") == 0).FirstOrDefault();
                if (area == null)
                {
                    SqlArea item = new SqlArea();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "P1";
                    item.name = "Phường 1";
                    item.des = "Phường 1";
                    item.group = context.groups!.Where(s => s.code.CompareTo("NK") == 0 && s.isdeleted == false).FirstOrDefault();
                    item.isdeleted = false;
                    context.areas!.Add(item);
                }
                area = context.areas!.Where(s => s.code.CompareTo("P2") == 0).FirstOrDefault();
                if (area == null)
                {
                    SqlArea item = new SqlArea();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "P2";
                    item.name = "Phường 2";
                    item.des = "Phường 2";
                    item.group = context.groups!.Where(s => s.code.CompareTo("NK") == 0 && s.isdeleted == false).FirstOrDefault();
                    item.isdeleted = false;
                    context.areas!.Add(item);
                }
                
                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> createAsync(string code, string name, string des, string group)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(group))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(group) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if(m_group == null)
                {
                    return false;
                }
                if(m_group.areas == null)
                {
                    m_group.areas = new List<SqlArea>();
                }

                SqlArea? area = m_group.areas.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (area != null)
                {
                    return false;
                }

                SqlArea item = new SqlArea();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                m_group.areas.Add(item);

                context.areas!.Add(item);

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

        public async Task<bool> editAsync(string code, string name, string des, string group)
        {
            using (DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(group) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if(m_group == null || m_group.areas == null)
                {
                    return false;
                }
                SqlArea? area = m_group.areas.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (area == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    area.des = des;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    area.name = name;
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

        public async Task<bool> deleteAsync(string area, string group)
        {
            using (DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(group) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (m_group == null || m_group.areas == null)
                {
                    return false;
                }
                SqlArea? m_area = m_group.areas.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_area == null)
                {
                    return false;
                }
                
                m_group.areas.Remove(m_area);
                m_area.isdeleted = true;

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

        public class MyItemGroup
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
        }

        public class ItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public MyItemGroup group { get; set; } = new MyItemGroup();
        }

        public List<ItemArea> getListArea()
        {
            List<ItemArea> list = new List<ItemArea>();
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false).Include(s => s.group).ToList();
                if (areas.Count > 0)
                {
                    foreach (SqlArea item in areas)
                    {
                        ItemArea tmp = new ItemArea();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.des = item.des;

                        if(item.group != null)
                        {
                            tmp.group.code = item.group.code;
                            tmp.group.name = item.group.name;
                        }    

                        list.Add(tmp);
                    }
                }
                return list;
            }
        }
    }
}
