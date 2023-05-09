using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.APIs
{
    public class MyCustomer
    {
        public MyCustomer() { }
       
        public async Task<bool> editCustomerAsync(string idkh, string danhbo, string sdt, string tenkh, string diachiTT, string diachiLH, string diachiLD, string latidude, string longitude)
        {
            if (string.IsNullOrEmpty(idkh))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {

                SqlCustomer? m_customer = context.customers!.Where(s => s.isdeleted == false && s.idKH.CompareTo(idkh) == 0).FirstOrDefault();
                if (m_customer == null)
                {
                    return false;
                }


                if (!string.IsNullOrEmpty(danhbo))
                {
                    m_customer.maDB = danhbo;
                }
                if (!string.IsNullOrEmpty(sdt))
                {
                    m_customer.sdt = sdt;
                }
                if (!string.IsNullOrEmpty(tenkh))
                {
                    m_customer.tenKH = tenkh;
                }
                if (!string.IsNullOrEmpty(diachiTT))
                {
                    m_customer.diachiTT = diachiTT;
                }
                if (!string.IsNullOrEmpty(diachiLH))
                {
                    m_customer.diachiLH = diachiLH;
                }
                if (!string.IsNullOrEmpty(diachiLD))
                {
                    m_customer.diachiLD = diachiLD;
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


        public async Task<bool> deleteCustomerAsync(string idkh)
        {
            if (string.IsNullOrEmpty(idkh))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {

                SqlCustomer? mcustomer = context.customers!.Where(s => s.isdeleted == false && s.idKH.CompareTo(idkh) == 0).FirstOrDefault();
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

        public class ItemCustomer
        {
            public string idkh { get; set; } = "";
            public string danhbo { get; set; } = "";
            public string sdt { get; set; } = "";
            public string tenkh { get; set; } = "";
            public string diachiTT { get; set; } = "";
            public string diachiLH { get; set; } = "";
            public string diachiLD { get; set; } = "";
            public string latidude { get; set; } = "";
            public string longitude { get; set; } = "";
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

                    tmp.idkh = item.idKH;
                    tmp.danhbo = item.maDB;
                    tmp.sdt = item.sdt;
                    tmp.tenkh = item.tenKH;
                    tmp.diachiTT = item.diachiTT;
                    tmp.diachiLH = item.diachiLH;
                    tmp.diachiLD = item.diachiLD;
                    tmp.latidude = item.latitude;
                    tmp.longitude = item.longitude;

                    lists.Add(tmp);
                }
                return lists;
            }
        }
/*
        public class ItemInfoValueForDevice
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public SqlStatus? state { get; set; } = new SqlStatus();

        }

        public class ItemInfoCustomerForDevice
        {
            public long idkh { get; set; } = 0;
            public string madb { get; set; } = "";
            public string tenkh { get; set; } = "";
            public string diachi { get; set; } = "";

            public ItemInfoValueForDevice module { get; set; } = new ItemInfoValueForDevice();
            public ItemInfoValueForDevice dongho { get; set; } = new ItemInfoValueForDevice();
        }


        public class DataStatementPlot
        {
            public List<string> times { get; set; } = new List<string>();
            public List<ItemInfoCustomerForDevice> data { get; set; } = new List<ItemInfoCustomerForDevice>();

        }*/

    }
}
