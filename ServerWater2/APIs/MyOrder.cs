using ServerWater2.Models;
using Newtonsoft.Json;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyState;
using Microsoft.EntityFrameworkCore;
using static ServerWater2.APIs.MyType;
using static ServerWater2.APIs.MyService;

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
        public async Task<bool> setStateOrder(long idUser, string code, int state, string note, string latitude, string longitude)
        {
            using (DataContext context = new DataContext())
            {
                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == state).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.ID == idUser).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false&& s.code.CompareTo(code) == 0)
                                                    .Include(s => s.state)
                                                    .FirstOrDefault();
                if (order == null)
                {
                    return false;
                }

                order.state = m_state;
               

               
                SqlLogOrder log = new SqlLogOrder();
                log.ID = DateTime.Now.Ticks;
                log.order = order;
                log.note = note;
                log.time = order.lastestTime;
                log.user = context.users!.Where(s => s.ID == idUser).FirstOrDefault();
                log.latitude = latitude;
                log.longitude = longitude;
                context.logs!.Add(log);

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
        public async Task<string> createUpdateOrderAsync(string customer, string phone,string addressCustomer,string addressWater, string addressContract, string service, string type, string note)
        {
            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(addressWater) || string.IsNullOrEmpty(addressContract) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(type))
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {
                SqlOrder? m_order = context.orders!.Where(s => s.name.CompareTo(customer) == 0 && s.phone.CompareTo(phone) == 0).FirstOrDefault();
                if(m_order == null)
                {
                    m_order = new SqlOrder();
                    m_order.ID = DateTime.Now.Ticks;
                    m_order.code = generatorcode();
                    m_order.name = customer;
                    m_order.addressCustomer = addressCustomer;
                    m_order.addressWater = addressWater;
                    m_order.addressContract = addressContract;
                    m_order.phone = phone;
                    m_order.note = note;
                    m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                    m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                    m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    m_order.createdTime = DateTime.Now.ToUniversalTime();
                    m_order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                    context.orders!.Add(m_order);
                    await context.SaveChangesAsync();

                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_order;
                    log.time = DateTime.Now.ToUniversalTime();
                    log.note = "New Order";
                    context.logs!.Add(log);

                }
                else
                {
                    if(m_order.state!.code < 3)
                    {
                        m_order.addressCustomer = addressCustomer;
                        m_order.addressWater = addressWater;
                        m_order.addressContract = addressContract;
                        m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                        m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                        if (string.IsNullOrEmpty(note))
                        {
                            m_order.note = note;
                        }

                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    }  
                    else
                    {
                        return "Nhan vien da xac nhan yeu cau !!!";
                    }    
                }


                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return m_order.code;
                }
                else
                {
                    return "";
                }
            }
        }
        public async Task<bool> confirmOrder(string token, string code)
        {
            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                if(order == null)
                {
                    return false;
                }
                if(order.state!.code != 0)
                {
                    return false;
                }    

                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code == 1).FirstOrDefault();
                if(state == null)
                {
                    return false;
                }

                order.receiver = m_user;
                order.state = state;
                order.lastestTime = DateTime.Now.ToUniversalTime();

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
        public async Task<bool> setCustomer(string token,string maDB, string code)
        {
            using(DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if(user == null)
                {
                    return false;
                }
                SqlCustomer? m_customer = context.customers!.Where(s => s.code.CompareTo(maDB) == 0 && s.isdeleted == false).Include(s => s.orders).FirstOrDefault();
                if(m_customer == null)
                {
                    return false;
                }
                //if(m_customer.orders == null)
                //{
                //    m_customer.orders = new List<SqlOrder>();
                //}
                //SqlOrder? tmp = m_customer.orders.Where(s => s.code.CompareTo(order) == 0 && s.isDelete == false).FirstOrDefault();
                //if(tmp != null)
                //{
                //    return false;
                //}

                SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).Include(s => s.customer).FirstOrDefault();
                if(m_order == null)
                {
                    return false;
                }

                m_order.customer = m_customer;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

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
        public async Task<bool> managerReceiveOrder(string token, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Include(s => s.state).Where(s => s.isDelete == false && s.code.CompareTo(code) == 0 && s.state!.code == 1).Include(s => s.manager).FirstOrDefault();
                if (order == null)
                {
                    return false;
                }
                if (order.manager != null)
                {
                    return false;
                }

                order.manager = m_user;
                order.lastestTime = DateTime.Now.ToUniversalTime();

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
        public async Task<bool> setWorkerOrder(string token, string code, string user)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }
                SqlUser? worker = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0 && s.role!.code.CompareTo("staff") == 0).FirstOrDefault();
                if (worker == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Include(s => s.state).Where(s => s.isDelete == false && s.code.CompareTo(code) == 0 && s.state!.code == 1).Include(s => s.worker).FirstOrDefault();
                if (order == null)
                {
                    return false;
                }
                if (order.worker != null)
                {
                    return false;
                }
                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code == 2).FirstOrDefault();
                if(state == null)
                {
                    return false;
                }
                order.worker = worker;
                order.state = state;
                order.lastestTime = DateTime.Now.ToUniversalTime();


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
        public async Task<bool> workingJob(string token, string code)
        {
            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }
                SqlOrder? m_order = context.orders!.Include(s => s.worker).Where(s => s.isDelete == false && s.code.CompareTo(code) == 0 && s.worker!.token.CompareTo(m_user.token) == 0).FirstOrDefault();
                if(m_order == null)
                {
                    return false;
                }
                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code == 3).FirstOrDefault();
                if (state == null)
                {
                    return false;
                }
                m_order.state = state;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();


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
        public class ItemUser
        {
            public string user { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }
      
        public class ItemOrder
        {
            public string id { get; set; } = "";
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string phone { get; set; } = "";
            public string addressCustomer { get; set; } = "";
            public string addressWater { get; set; } = "";
            public string addressContract { get; set; } = "";
            public ItemUser reciver { get; set; } = new ItemUser();
            public ItemUser manager { get; set; } = new ItemUser();
            public ItemUser worker { get; set; } = new ItemUser();
            public ItemCustomer customer  { get; set; } = new ItemCustomer();
            public string note { get; set; } = "";
            public ItemType type { get; set; } = new ItemType();
            public ItemService service { get; set; } = new ItemService();
            public ItemStateOrder state { get; set; } = new ItemStateOrder();
            public string createTime { get; set; } = "";
            public string lastestTime { get; set; } = "";

        }


        public List<ItemOrder> getListOrder(string token)
        {
            List<ItemOrder> list = new List<ItemOrder>();
            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return new List<ItemOrder>();
                }

                List<SqlOrder>? orders = context.orders!.Where(s => s.isDelete == false).Include(s => s.customer).Include(s => s.type).Include(s => s.service).Include(s => s.state).Include(s => s.receiver).Include(s => s.manager).Include(s => s.worker).ToList();
                if(orders == null)
                {
                    return new List<ItemOrder>();
                }

                foreach (SqlOrder item in orders)
                {
                    ItemOrder tmp = new ItemOrder();
                    tmp.id = item.ID.ToString();
                    tmp.code = item.code;
                    tmp.name = item.name;
                    tmp.phone = item.phone;
                    tmp.addressCustomer = item.addressCustomer;
                    tmp.addressWater = item.addressWater;
                    tmp.addressContract = item.addressContract;
                    if (item.customer != null)
                    {
                        if (item.customer.isdeleted == false)
                        {
                            ItemCustomer customer = new ItemCustomer();
                            customer.maDB = item.customer.code;
                            customer.sdt = item.customer.phone;
                            customer.tenkh = item.customer.name;
                            customer.diachi = item.customer.address;
                            customer.x = item.customer.latitude;
                            customer.y = item.customer.longitude;

                            tmp.customer = customer;
                        }

                    }

                    if (item.receiver != null)
                    {
                        ItemUser receiver = new ItemUser();
                        receiver.user = item.receiver.user;
                        receiver.displayName = item.receiver.displayName;
                        receiver.numberPhone = item.receiver.phoneNumber;

                        tmp.reciver = receiver;
                    }

                    if (item.manager != null)
                    {
                        ItemUser manager = new ItemUser();
                        manager.user = item.manager.user;
                        manager.displayName = item.manager.displayName;
                        manager.numberPhone = item.manager.phoneNumber;

                        tmp.manager = manager;
                    }

                    if (item.worker != null)
                    {
                        ItemUser worker = new ItemUser();
                        worker.user = item.worker.user;
                        worker.displayName = item.worker.displayName;
                        worker.numberPhone = item.worker.phoneNumber;

                        tmp.worker = worker;
                    }

                    tmp.note = item.note;
                    if (item.type != null)
                    {
                        ItemType type = new ItemType();

                        type.code = item.type.code;
                        type.name = item.type.name;
                        type.des = item.type.des;
                        tmp.type = type;
                    }

                    if (item.service != null)
                    {
                        ItemService service = new ItemService();
                        service.code = item.service.code;
                        service.name = item.service.name;
                        service.des = item.service.des;
                        tmp.service = service;
                    }
                    if (item.state != null)
                    {
                        ItemStateOrder state = new ItemStateOrder();
                        state.code = item.state.code;
                        state.name = item.state.name;
                        state.des = item.state.des;
                        tmp.state = state;
                    }
                    tmp.createTime = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.lastestTime = item.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                    list.Add(tmp);
                }
            }
            return list;
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
