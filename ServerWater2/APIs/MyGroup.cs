using Microsoft.EntityFrameworkCore;
using ServerWater2.Models;

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
                if(group.areas != null)
                {
                    foreach(SqlArea m_area in group.areas)
                    {
                        m_area.group = null;
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

        public class MyItemUser
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string username { get; set; } = "";
            public string phone { get; set; } = "";
            public string role { get; set; } = "";
        }

        public class ItemGroup
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public List<MyItemArea> areas { get; set; } = new List<MyItemArea>();
            public List<MyItemUser> users { get; set; } = new List<MyItemUser>();
        }

        public List<ItemGroup> getListGroup()
        {
            List<ItemGroup> list = new List<ItemGroup>();
            using (DataContext context = new DataContext())
            {
                List<SqlGroup>? groups = context.groups!.Where(s => s.isdeleted == false).Include(s => s.areas).Include(s => s.users!).ThenInclude(s => s.role).ToList();
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
                        if(item.users != null)
                        {
                            foreach (SqlUser user in item.users)
                            {
                                MyItemUser m_user = new MyItemUser();
                                m_user.code = user.user;
                                m_user.name = user.displayName;
                                m_user.username = user.username;
                                m_user.phone = user.phoneNumber;
                                m_user.role = user.role!.name;
                                tmp.users.Add(m_user);
                            }
                        }
                        list.Add(tmp);
                    }
                }
                return list;
            }

        }

        public List<MyItemUser> getListUser(string group)
        {
            List<MyItemUser> list = new List<MyItemUser>();
            using(DataContext context = new DataContext())
            {
                SqlGroup? m_group = context.groups!.Where(s => s.code.CompareTo(group) == 0 && s.isdeleted == false).Include(s => s.users!).ThenInclude(s => s.role).FirstOrDefault();
                if(m_group == null)
                {
                    return new List<MyItemUser>();
                }
                if(m_group.users == null)
                {
                    return new List<MyItemUser>();
                }

                foreach(SqlUser m_user in m_group.users)
                {
                    MyItemUser item = new MyItemUser();
                    item.code = m_user.user;
                    item.name = m_user.displayName;
                    item.username = m_user.username;
                    item.phone = m_user.phoneNumber;
                    item.role = m_user.role!.name;
                    
                    list.Add(item);
                }
            }
            return list;
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
