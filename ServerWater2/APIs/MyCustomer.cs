using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Drawing;

namespace ServerWater2.APIs
{
    public class MyCustomer
    {
        public MyCustomer() { }

        public static bool flagblock = false;

        public async Task<bool> createCustomerAsync(string code, string phone, string name, string address, string note, string latidude, string longitude)
        {
            if (string.IsNullOrEmpty(code)|| string.IsNullOrEmpty(name))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {

                SqlCustomer? m_customer = context.customers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_customer != null)
                {
                    return false;
                }

                SqlCustomer customer = new SqlCustomer();
                customer.ID = DateTime.Now.Ticks;
                customer.code = code;
                customer.phone = phone;
                customer.name = name;
                customer.address = address;
                customer.note = note;
                customer.latitude = latidude;
                customer.longitude = longitude;

                context.customers!.Add(customer);
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
        public async Task<bool> editCustomerAsync( string code, string phone, string name, string address,string note, string latidude, string longitude)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {

                SqlCustomer? m_customer = context.customers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_customer == null)
                {
                    return false;
                }


                
                if (!string.IsNullOrEmpty(phone))
                {
                    m_customer.phone = phone;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    m_customer.name = name;
                }
                if (!string.IsNullOrEmpty(address))
                {
                    m_customer.address = address;
                }
                if (!string.IsNullOrEmpty(note))
                {
                    m_customer.note = note;
                }
                if (!string.IsNullOrEmpty(latidude))
                {
                    m_customer.latitude = latidude;
                }
                if (!string.IsNullOrEmpty(longitude))
                {
                    m_customer.longitude = longitude;
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


        public async Task<bool> deleteCustomerAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {

                SqlCustomer? mcustomer = context.customers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (mcustomer == null)
                {
                    return false;
                }

                mcustomer.isdeleted = true;


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
        public async Task<string> addImageCustomer(string token, string code, byte[] image)
        {
            try
            {
                string m_file = "";
                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                    if (user == null)
                    {
                        return "Error user";
                    }
                    
                    byte[]? tmp = await Program.api_file.getImageChanged(image);
                    if (tmp != null)
                    {
                        m_file = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), tmp);
                        if (string.IsNullOrEmpty(m_file))
                        {
                            return "Error file";
                        }
                    }
                    else
                    {
                        return "Code file empty";
                    }
                }
                using (DataContext context = new DataContext())
                {
                    while (flagblock)
                    {
                        Thread.Sleep(1);
                    }
                    flagblock = true;
                    SqlCustomer? customer = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                    if (customer == null)
                    {
                        return "Error customer";
                    }
                    if (customer.images == null)
                    {
                        customer.images = new List<string>();
                    }
                    customer.images.Add(m_file);

                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        flagblock = false;
                        return m_file;
                    }
                    else
                    {
                        flagblock = false;
                        return "null";
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return ex.Message;
            }
        }

        public async Task<bool> removeImageCustomer(string token, string code, string image)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlCustomer? customer = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (customer == null)
                {
                    return false;
                }
                if (customer.images != null)
                {
                    customer.images.Remove(image);
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

        
        public class ItemCustomer
        {
            public string maDB { get; set; } = "";
            public string route { get; set; } = "";
            public string sdt { get; set; } = "";
            public string tenkh { get; set; } = "";
            public string diachi { get; set; } = "";
            public string note { get; set; } = "";
            public string x { get; set; } = "";
            public string y { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
        }

        public List<ItemCustomer> listCustomer()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlCustomer>? customers = context.customers!.Where(s => s.isdeleted == false).ToList();
                if (customers.Count == 0)
                {
                    return new List<ItemCustomer>();
                }
                List<ItemCustomer> lists = new List<ItemCustomer>();

                foreach (SqlCustomer item in customers)
                {
                    ItemCustomer tmp = new ItemCustomer();

                    tmp.maDB = item.code;
                    tmp.route = string.IsNullOrEmpty(item.route) ? "Coming Soon !!!" : item.route;
                    tmp.sdt = item.phone;
                    tmp.tenkh = item.name;
                    tmp.diachi = item.address;
                    tmp.note = item.note;
                    tmp.x = item.latitude;
                    tmp.y = item.longitude;
                    if(item.images != null)
                    {
                        tmp.images = item.images;
                    }
                   

                    lists.Add(tmp);
                }
                return lists;
            }
        }
    }
}
