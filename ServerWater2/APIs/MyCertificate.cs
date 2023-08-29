﻿using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyCertificate
    {
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlCertificate? certificate = context.certificates!.Where(s => s.code.CompareTo("C1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate == null)
                {
                    SqlCertificate item = new SqlCertificate();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "C1";
                    item.name = "Giấy chủ quyền nhà/đất";
                    item.des = "Giấy chủ quyền nhà/đất";
                    item.isdeleted = false;
                    context.certificates!.Add(item);
                }
                certificate = context.certificates!.Where(s => s.code.CompareTo("C2") == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate == null)
                {
                    SqlCertificate item = new SqlCertificate();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "C2";
                    item.name = "Giấy tờ mua bán nhà/đất";
                    item.des = "Giấy tờ mua bán nhà/đất";
                    item.isdeleted = false;
                    context.certificates!.Add(item);
                }
                certificate = context.certificates!.Where(s => s.code.CompareTo("C3") == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate == null)
                {
                    SqlCertificate item = new SqlCertificate();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "C3";
                    item.name = "Hợp đồng thuê nhà/đất";
                    item.des = "Hợp đồng thuê nhà/đất";
                    item.isdeleted = false;
                    context.certificates!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }
        public class ItemCertificate
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemCertificate> getList()
        {
            List<ItemCertificate> list = new List<ItemCertificate>();
            using (DataContext context = new DataContext())
            {
                List<SqlCertificate>? actions = context.certificates!.Where(s => s.isdeleted == false).ToList();
                if (actions.Count > 0)
                {
                    foreach (SqlCertificate item in actions)
                    {
                        ItemCertificate tmp = new ItemCertificate();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.des = item.des;

                        list.Add(tmp);
                    }
                }
                return list;
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
                SqlCertificate? certificate = context.certificates!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate != null)
                {
                    return false;
                }

                SqlCertificate item = new SqlCertificate();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                context.certificates!.Add(item);

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
                SqlCertificate? certificate = context.certificates!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    certificate.name = name;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    certificate.des = des;
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
                SqlCertificate? certificate = context.certificates!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (certificate == null)
                {
                    return false;
                }

                certificate.isdeleted = true;

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
    }
}