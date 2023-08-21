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
                    item.isdeleted = false;
                    context.areas!.Add(item);
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
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (area != null)
                {
                    return false;
                }

                SqlArea item = new SqlArea();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
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

        public async Task<bool> editAsync(string code, string name, string des)
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
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

        public async Task<bool> deleteAsync(string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (area == null)
                {
                    return false;
                }

                area.isdeleted = true;

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
            public string des { get; set; } = "";
        }

        public class ItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public List<MyItemGroup> groups { get; set; } = new List<MyItemGroup>();
        }

        public List<ItemArea> getListArea()
        {
            List<ItemArea> list = new List<ItemArea>();
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false).Include(s => s.groups).ToList();
                if (areas.Count > 0)
                {
                    foreach (SqlArea item in areas)
                    {
                        ItemArea tmp = new ItemArea();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.des = item.des;
                        if(item.groups != null)
                        {
                            foreach(SqlGroup group in item.groups)
                            {
                                if(group.isdeleted == false)
                                {
                                    MyItemGroup m_group = new MyItemGroup();
                                    m_group.code = group.code;
                                    m_group.name = group.name;
                                    m_group.des = group.des;
                                    tmp.groups.Add(m_group);
                                }    
                            }    
                        }    

                        list.Add(tmp);
                    }
                }
                return list;
            }
        }
    }
}
