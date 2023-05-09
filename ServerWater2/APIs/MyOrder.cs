using ServerWater2.Models;
using Newtonsoft.Json;

namespace ServerWater2.APIs  
{

    public class MyOrder
    {
        public MyOrder()
        {
        }
        public string generatorcode()
        {
            using (DataContext context = new DataContext())
            {
                string code = DataContext.randomString(16);
                while (true)
                {
                    SqlOrder? tmp = context.orders!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (tmp == null)
                    {
                        return code;
                    }
                }
            }
        }
        public async Task<string> createOrderAsync(string customer, string phone,string addressCustomer,string addressWater, string addressContract, string service, string type, string note)
        {
            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(addressWater) || string.IsNullOrEmpty(addressContract) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(type))
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {

                SqlCustomer? m_customer = context.customers!.Where(s => s.isdeleted == false && s.tenKH.CompareTo(customer) == 0 && s.sdt.CompareTo(phone) == 0).FirstOrDefault();
                if (m_customer == null)
                {
                    SqlCustomer tmp = new SqlCustomer();

                    tmp.ID = DateTime.Now.Ticks;
                    tmp.idKH = "stvg_"+ DateTime.Now.Ticks;
                    tmp.maDB = "";
                    tmp.sdt = phone;
                    tmp.tenKH = customer;
                    tmp.diachiTT = addressCustomer;
                    tmp.diachiLH = addressContract;
                    tmp.diachiLD = addressWater;
                    tmp.latitude = "";
                    tmp.longitude = "";
                    tmp.isdeleted = false;
                    context.customers!.Add(tmp);

                    await context.SaveChangesAsync();
                }

                SqlService? m_service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                if (m_service == null)
                {
                    return "";
                }
                SqlType? m_request = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                if(m_request == null)
                {
                    return "";
                }

                SqlOrder order = new SqlOrder();
                order.ID = DateTime.Now.Ticks;
                order.customer = m_customer;
                order.code = generatorcode();
                order.note = note;
                order.type = m_request;
                order.service = m_service;
                order.lastestTime = DateTime.Now.ToUniversalTime();
                order.createdTime = DateTime.Now.ToUniversalTime();
                order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                context.orders!.Add(order);
                await context.SaveChangesAsync();

                SqlLogRequest log_request= new SqlLogRequest();
                log_request.ID = DateTime.Now.Ticks;
                log_request.order = order; 
                log_request.customer = m_customer;
                log_request.time = DateTime.Now.ToUniversalTime();
                log_request.note = "New Order";
                context.logRequests!.Add(log_request);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return order.code;
                }
                else
                {
                    return "";
                }
            }
        }

       

       



        //public async Task<bool> removeImageBeforeAsync(string token, string code, string image)
        //{
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            if (string.IsNullOrEmpty(image))
        //            {
        //                return false;
        //            }

        //            if (order.beforeImages == null)
        //            {
        //                return false;
        //            }

        //            string buf = "";
        //            foreach (string tmp in order.beforeImages)
        //            {
        //                ItemImage? buffer = JsonConvert.DeserializeObject<ItemImage>(tmp);
        //                if (buffer == null)
        //                {
        //                    continue;
        //                }
        //                if (buffer.code.CompareTo(image) == 0)
        //                {
        //                    buf = tmp;
        //                    break;
        //                }
        //            }
        //            if (string.IsNullOrEmpty(buf))
        //            {
        //                return false;
        //            }

        //            order.beforeImages.Remove(buf);
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> updateNoteBeforeAsync(string token, string code, string note)
        //{
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            order.beforeNote = note;
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> updateNoteRepairAsync(string token, string code, string note)
        //{
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            order.repairNote = note;
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> updateNoteAfterAsync(string token, string code, string note)
        //{
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            order.afterNote = note;
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> addImageAfterAsync(string token, string code, byte[] data)
        //{
        //    string codefile = await Program.api_file.saveFileAsync(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.image"), data);
        //    if (string.IsNullOrEmpty(codefile))
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            ItemImage image = new ItemImage();
        //            image.code = codefile;
        //            image.user = user.user;
        //            image.time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //            if (order.afterImages == null)
        //            {
        //                order.afterImages = new List<string>();
        //            }
        //            order.afterImages.Add(JsonConvert.SerializeObject(image));
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> removeImageAfterAsync(string token, string code, string image)
        //{
        //    try
        //    {
        //        long id = long.Parse(code);
        //        using (DataContext context = new DataContext())
        //        {
        //            SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
        //            if (user == null)
        //            {
        //                return false;
        //            }

        //            SqlOrder? order = context.orders!.Where(s => s.ID == id).FirstOrDefault();
        //            if (order == null)
        //            {
        //                return false;
        //            }

        //            if (string.IsNullOrEmpty(image))
        //            {
        //                return false;
        //            }

        //            if (order.afterImages == null)
        //            {
        //                return false;
        //            }

        //            string buf = "";
        //            foreach (string tmp in order.afterImages)
        //            {
        //                ItemImage? buffer = JsonConvert.DeserializeObject<ItemImage>(tmp);
        //                if (buffer == null)
        //                {
        //                    continue;
        //                }
        //                if (buffer.code.CompareTo(image) == 0)
        //                {
        //                    buf = tmp;
        //                    break;
        //                }
        //            }
        //            if (string.IsNullOrEmpty(buf))
        //            {
        //                return false;
        //            }
        //            order.afterImages.Remove(buf);
        //            order.timeLastest = DateTime.Now.ToUniversalTime();

        //            int rows = await context.SaveChangesAsync();
        //            if (rows > 0)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
