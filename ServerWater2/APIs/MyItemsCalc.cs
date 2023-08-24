using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyItemsCalc
    {
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlCalcItems? m_item = context.calcItems!.Where(s => s.code.CompareTo("1") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "1";
                    item.name = "Ống PVC D.27";
                    item.unit = "m";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("2") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "2";
                    item.name = "Ống HDPE D.25";
                    item.unit = "m";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("3") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "3";
                    item.name = "ĐKT: 63 x 3/4";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("4") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "4";
                    item.name = "Côn PVC D.21RTx27";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("5") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "5";
                    item.name = "Co PVC D.27x90";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("6") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "6";
                    item.name = "Van PVC D.27";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("7") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "7";
                    item.name = "Van góc đồng 1C lá lật 25 x 1 / 2";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("8") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "8";
                    item.name = "Nối thẳng bằng đồng 25 x 3/4";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("9") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "9";
                    item.name = "Đồng hồ đo nước D.15 ly (Công ty cấp)";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("10") == 0 ).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "10";
                    item.name = "Đuôi đồng hồ D.15 ly";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("11") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "11";
                    item.name = "Hộp đồng hồ";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("12") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "12";
                    item.name = "Co HDPE D25x90";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("13") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "13";
                    item.name = "Keo dán ống";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("14") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "14";
                    item.name = "Cao su non 10m";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("15") == 0 ).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "15";
                    item.name = "Đào đất cấp 2";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }

                m_item = context.calcItems!.Where(s => s.code.CompareTo("16") == 0).FirstOrDefault();
                if (m_item == null)
                {
                    SqlCalcItems item = new SqlCalcItems();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "16";
                    item.name = "Lâp đất";
                    item.unit = "Cái";
                    item.isdeleted = false;
                    context.calcItems!.Add(item);
                }



                int rows = await context.SaveChangesAsync();
            }
        }
        public async Task<bool> createAsync(string code, string name, string des, string unit)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(unit))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlCalcItems? m_item = context.calcItems!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_item != null)
                {
                    return false;
                }

                SqlCalcItems item = new SqlCalcItems();
                item.ID = DateTime.Now.Ticks;
                item.code = code;
                item.name = name;
                item.des = des;
                item.unit = unit;
                context.calcItems!.Add(item);

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

        public async Task<bool> editAsync(string code, string name, string des, string unit)
        {
            using (DataContext context = new DataContext())
            {
                SqlCalcItems? m_item = context.calcItems!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_item == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(des))
                {
                    m_item.des = des;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    m_item.name = name;
                }
                if (!string.IsNullOrEmpty(unit))
                {
                    m_item.unit = unit;
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
                SqlCalcItems? m_item = context.calcItems!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_item == null)
                {
                    return false;
                }

                m_item.isdeleted = true;

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
        public class ItemCalcItems
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string unit { get; set; } = "";
        }

        public List<ItemCalcItems> getList()
        {
            List<ItemCalcItems> list = new List<ItemCalcItems>();
            using (DataContext context = new DataContext())
            {
                List<SqlCalcItems>? items = context.calcItems!.Where(s => s.isdeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (SqlCalcItems item in items)
                    {
                        ItemCalcItems tmp = new ItemCalcItems();
                        tmp.code = item.code;
                        tmp.name = item.name;
                        tmp.unit = item.unit;

                        list.Add(tmp);
                    }
                }
                return list;
            }
        }
    }
}
