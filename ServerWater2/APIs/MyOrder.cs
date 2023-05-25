using ServerWater2.Models;
using Newtonsoft.Json;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyState;
using Microsoft.EntityFrameworkCore;
using static ServerWater2.APIs.MyType;
using static ServerWater2.APIs.MyService;
using System.Numerics;
using Microsoft.OpenApi.Writers;
using static ServerWater2.APIs.MyAction;
using static ServerWater2.APIs.MyLogOrder;
using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Serilog;
using static ServerWater2.Program;
using static Azure.Core.HttpHeader;
using static ServerWater2.APIs.MyOrder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ServerWater2.APIs  
{

    public class MyOrder
    {
        
        public MyOrder()
        {
        }
        public static bool flagblock = false;

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

        public async Task<bool> setStateOrder(long idUser, string code, string note, string latitude, string longitude)
        {
            using (DataContext context = new DataContext())
            {
                
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.ID == idUser).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false &&  s.isFinish == false && s.code.CompareTo(code) == 0)
                                                    .Include(s => s.state)
                                                    .FirstOrDefault();
                if (order == null)
                {
                    return false;
                }
                

                SqlLogOrder log = new SqlLogOrder();
                log.ID = DateTime.Now.Ticks;
                log.order = order;
                log.user = user;
                log.latitude = latitude;
                log.longitude = longitude;
                log.note = note;
                log.time = DateTime.Now.ToUniversalTime();
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

        public async Task<bool> setAction(string token, string order, string action, string note)
        {
            
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlAction? m_action = context.actions!.Where(s => s.code.CompareTo(action) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_action == null)
                {
                    return false;
                }

                SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(order) == 0 && s.order!.isDelete == false).OrderByDescending(s => s.time).FirstOrDefault();
                if (m_log == null)
                {
                    return false;
                }

                m_log.action = m_action;
                if (!string.IsNullOrEmpty(note))
                {
                   /* try
                    {
                        ItemNote? m_note = JsonConvert.DeserializeObject<ItemNote>(note);
                        if (m_note != null)
                        {
                            ItemNote? image = JsonConvert.DeserializeObject<ItemNote>(m_log.note);
                            if (image != null)
                            {
                                if (m_note.images.Count > 0)
                                {
                                    foreach (string item in m_note.images)
                                    {
                                        image.images.Add(item);
                                    }
                                }

                            }
                            m_log.note = JsonConvert.SerializeObject(image);
                        }
                    }
                    catch (Exception)
                    {
                        ItemNote? image = JsonConvert.DeserializeObject<ItemNote>(m_log.note);
                        if (image != null)
                        {
                            image.note = note;
                            image.images = image.images;
                        }
                        m_log.note = JsonConvert.SerializeObject(image);

                    }*/

                    m_log.note = note;
                }
                m_log.time = DateTime.Now.ToUniversalTime();
                int rows = await context.SaveChangesAsync();

                return true;
            }
        }

        public class ItemNotifyOrder
        {
            public string order { get; set; } = "";
            public string state { get; set; } = "";
            public string time { get; set; } = "";
        }

        public async Task<bool> saveNotification(int state, string notification)
        {
            if(string.IsNullOrEmpty(notification))
            {
                return false;
            }
            using(DataContext context = new DataContext())
            {
                try
                {
                    List<SqlUser>? users = context.users!.Where(s => s.isdeleted == false && s.isClear == false).Include(s => s.role).ToList();
                    if (users.Count > 0)
                    {
                        if (state == 0)
                        {
                            users = users.Where(s => s.role!.code.CompareTo("admin") == 0 || s.role!.code.CompareTo("receiver") == 0).ToList();

                        }
                        else if (state == 1)
                        {
                            users = users.Where(s => s.role!.code.CompareTo("manager") == 0).ToList();

                        }
                        else
                        {
                            users = users.Where(s => s.role!.code.CompareTo("staff") == 0).ToList();

                        }


                        ItemNotifyOrder? item = JsonConvert.DeserializeObject<ItemNotifyOrder>(notification);
                        if (item == null)
                        {
                            return false;
                        }

                        foreach (SqlUser m_user in users)
                        {
                            if (string.IsNullOrEmpty(m_user.notifications))
                            {
                                List<ItemNotifyOrder> items = new List<ItemNotifyOrder>();
                                items.Add(item);
                                m_user.notifications = JsonConvert.SerializeObject(items);
                            }
                            else
                            {
                                List<ItemNotifyOrder>? items = JsonConvert.DeserializeObject<List<ItemNotifyOrder>>(m_user.notifications);
                                if (items != null)
                                {
                                    items.Add(item);
                                }
                                m_user.notifications = JsonConvert.SerializeObject(items);
                            }
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
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex.ToString());
                    return false;
                }
            }
        }


        public async Task<string> createUpdateOrderAsync(string code, string name, string customer, string phone, string addressCustomer, string addressOrder, string addressContract, string service, string type, string note)
        {
            using (DataContext context = new DataContext())
            {
                SqlOrder? m_order = null;

                if (string.IsNullOrEmpty(code))
                {
                    m_order = new SqlOrder();
                    m_order.ID = DateTime.Now.Ticks;
                    m_order.code = generatorcode();
                    m_order.name = name;
                    m_order.phone = phone;
                    m_order.addressCustomer = addressCustomer;
                    m_order.addressWater = addressOrder;
                    m_order.addressContract = addressContract;
                    m_order.note = note;
                    m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                    m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                    m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    m_order.createdTime = DateTime.Now.ToUniversalTime();
                    m_order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                    context.orders!.Add(m_order);
                    await context.SaveChangesAsync();

                    /*ItemNote image = new ItemNote();
                    image.note = String.Format("{0}_{1} : {2}", m_order.code, m_order.service!.name, m_order.state!.name);
                    image.images = new List<string>();*/

                    string m_note = string.Format("{0}_{1} : {2}", m_order.code, m_order.service!.name, m_order.state!.name);

                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_order;
                    log.time = DateTime.Now.ToUniversalTime();
                    log.note = m_note;
                    context.logs!.Add(log);
                }
                else 
                {
                    m_order = new SqlOrder();
                    m_order.ID = DateTime.Now.Ticks;
                    m_order.code = generatorcode();
                    m_order.name = name;
                    m_order.phone = phone;

                    m_order.customer = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.name.CompareTo(customer) == 0 && s.isdeleted == false).FirstOrDefault();
                    m_order.addressCustomer = addressCustomer;
                    m_order.addressWater = addressOrder;
                    m_order.addressContract = addressContract;
                    m_order.note = note;
                    m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                    m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                    m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    m_order.createdTime = DateTime.Now.ToUniversalTime();
                    m_order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                    context.orders!.Add(m_order);
                    await context.SaveChangesAsync();

                    /*ItemNote image = new ItemNote();
                    image.note = String.Format("{0}_{1} : {2}", m_order.code, m_order.service!.name, m_order.state!.name);
                    image.images = new List<string>();*/

                    string m_note = string.Format("{0}_{1} : {2}", m_order.code, m_order.service!.name, m_order.state!.name);

                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_order;
                    log.time = DateTime.Now.ToUniversalTime();
                    log.note = m_note;
                    context.logs!.Add(log);
                }

                ItemNotifyOrder itemNotify = new ItemNotifyOrder();
                itemNotify.order= m_order.code;
                itemNotify.state = m_order.state.name;
                itemNotify.time = m_order.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                string notification = JsonConvert.SerializeObject(itemNotify);
                bool flag =await saveNotification(m_order.state.code, notification);
                if (flag)
                {
                    List<HttpNotification> datas = Program.httpNotifications.Where(s => s.state.CompareTo(itemNotify.state) == 0).ToList();
                    foreach (HttpNotification m_data in datas)
                    {
                        m_data.messagers.Add(notification);
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

        public async Task<string> createNewOrder(string name, string phone, string addressCustomer, string addressOrder, string addressContract, string service, string type, string note)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(type))
            {
                return "";
            }
            using(DataContext context = new DataContext())
            {
                SqlType? m_type = context.types!.Where(s => s.code.CompareTo(type) == 0 && s.isdeleted == false).FirstOrDefault();
                if(m_type == null)
                {
                    return "";
                }

                SqlService? m_service = context.services!.Where(s => s.code.CompareTo(service) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_type == null)
                {
                    return "";
                }

                string order = await createUpdateOrderAsync("", name, "",phone, addressCustomer, addressOrder, addressContract, service, type, note);
                if(!string.IsNullOrEmpty(order))
                {
                    return order;
                }
                else
                {
                    return "";
                }    
            }
        }

        public async Task<bool> testOrder(string user,string code, int state)
        {
            using(DataContext context = new DataContext())
            {
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).Include(s => s.type).Include(s => s.worker).FirstOrDefault();
                if(order == null)
                {
                    return false;
                }
                SqlUser? muser = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0).FirstOrDefault();
                if(muser == null)
                {
                    return false;
                }

                if(state == 0)
                {
                    string tmp = await createNewOrder(order.name, order.phone, order.addressCustomer, order.addressWater, order.addressContract, order.service!.code, order.type!.code, order.note);
                }
                if(state == 1)
                {
                    bool check = await confirmOrder(muser.token, code);
                }
                if (state == 3)
                {
                    bool check = await setAssginOrder(muser.token, code, "staff");
                }
                return true;
            }
        }

        public async Task<string> createRequestOrder(string code, string name, string phone, string customer, string addressCustomer, string addressContract, string service, string note)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(service))
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {
                SqlService? m_service = context.services!.Where(s => s.code.CompareTo(service) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_service == null)
                {
                    return "";
                }

                string order = await createUpdateOrderAsync(code, name, customer, phone, addressCustomer, addressCustomer, addressContract, service, "", note);
                if (!string.IsNullOrEmpty(order))
                {
                    return order;
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
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }
                if(m_user.role!.code.CompareTo("manager") == 0)
                {
                    return false;
                }    

                SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                if(order == null)
                {
                    return false;
                }
                if(order.state!.code != 0)
                {
                    return false;
                }

                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 1).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }
                order.state = m_state;
                order.receiver = m_user;
                order.lastestTime = DateTime.Now.ToUniversalTime();

                ItemNotifyOrder itemNotify = new ItemNotifyOrder();
                itemNotify.order = order.code;
                itemNotify.state = order.state.name;
                itemNotify.time = order.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                
                string notification = JsonConvert.SerializeObject(itemNotify);
                bool flag = await saveNotification(order.state.code, notification);
                if (flag)
                {
                    List<HttpNotification> datas = Program.httpNotifications.Where(s => s.state.CompareTo(itemNotify.state) == 0).ToList();
                    foreach (HttpNotification m_data in datas)
                    {
                        m_data.messagers.Add(notification);
                    }
                }
                /*ItemNote image = new ItemNote();
                image.note = string.Format("{0} : {1} ", order.state!.name, order.code);
                image.images = new List<string>();*/

                string note = string.Format("{0} : {1} ", order.state!.name, order.code);

                flag = await setStateOrder(m_user.ID, order.code, note, "", "");

                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> setCustomer(string token, string maDB, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.receiverOrders).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlCustomer? m_customer = context.customers!.Where(s => s.code.CompareTo(maDB) == 0 && s.isdeleted == false).Include(s => s.orders).FirstOrDefault();
                if (m_customer == null)
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

                SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                if (m_order == null)
                {
                    return false;
                }
                
                if(m_order.state!.code < 1)
                {
                    return false;
                }

                m_order.customer = m_customer;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                image.note = string.Format("KH : {0} - MDB : {1} Theo DH : {2}_{3} ", m_customer.name, m_customer.code, m_order.code, m_order.service!.name);
                image.images = new List<string>();*/

                string note = string.Format("KH : {0} - MDB : {1} Theo DH : {2}_{3} ", m_customer.name, m_customer.code, m_order.code, m_order.service!.name);

                bool flag = await setStateOrder(user.ID, m_order.code, note, m_customer.latitude, m_customer.longitude);
                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }

        public async Task<bool> setConfirmedOrder(string token, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Include(s => s.role).Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Include(s => s.state).Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.manager).Include(s => s.customer).FirstOrDefault();
                if (order == null)
                {
                    return false;
                }
                if(order.state!.code != 1)
                {
                    return false;
                }
                if (order.manager != null)
                {
                    return false;
                }

                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 2).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                order.state = m_state;
                order.manager = m_user;
                order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                image.note = string.Format("{0} : {1} -  DH : {1}  ", m_user.user, order.state!.name, order.code);
                image.images = new List<string>();*/

                string note = string.Format("{0} : {1} -  DH : {1}  ", m_user.user, order.state!.name, order.code);
                bool flag = await setStateOrder(m_user.ID, order.code, note, "", "");
                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> setAssginOrder(string token, string code, string user)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.managerOrders!).ThenInclude(s => s.state).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }
                SqlUser? worker = context.users!.Include(s => s.role).Where(s => s.isdeleted == false  && s.user.CompareTo(user) == 0).FirstOrDefault();
                if (worker == null)
                {
                    return false;
                }

                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 3).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                SqlOrder? order = null;
                string note = "";

                if (m_user.role!.code.CompareTo("admin") == 0 || m_user.role!.code.CompareTo("receiver") == 0)
                {
                    order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if(order.state!.code > 2)
                    {
                        return false;
                    }

                    if(order.manager == null)
                    {
                        /*ItemNote m_image = new ItemNote();
                        m_image.note = string.Format("{0} : {1} -  DH : {1}  ", m_user.user, order.state!.name, order.code);
                        m_image.images = new List<string>();*/

                        note = string.Format("{0} : {1} -  DH : {1}  ", m_user.user, order.state!.name, order.code);
                        bool flag1 =  await setStateOrder(m_user.ID, order.code, note, "", "");

                    }
                }
                else
                {
                   
                    order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if (order.state!.code > 2)
                    {
                        return false;
                    }

                }

                order.state = m_state;
                order.worker = worker;
                order.lastestTime = DateTime.Now.ToUniversalTime();

                ItemNotifyOrder itemNotify = new ItemNotifyOrder();
                itemNotify.order = order.code;
                itemNotify.state = order.state.name;
                itemNotify.time = order.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                string notification = JsonConvert.SerializeObject(itemNotify);
                
                bool flag = await saveNotification(order.state.code, notification);
                if (flag)
                {
                    List<HttpNotification> datas = Program.httpNotifications.Where(s => s.state.CompareTo(itemNotify.state) == 0).ToList();
                    foreach (HttpNotification m_data in datas)
                    {
                        m_data.messagers.Add(notification);
                    }
                }
                /*ItemNote image = new ItemNote();
                image.note = string.Format("{0} : {1} -  DH : {1}  ", order.state!.name, m_user.user, order.code);
                image.images = new List<string>();
*/
                note = string.Format("{0} : {1} -  DH : {1}  ", order.state!.name, m_user.user, order.code);

                flag = await setStateOrder(m_user.ID, order.code, note, "", "");
                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public class ItemImage
        {
            public string code { get; set; } = "";
            public string user { get; set; } = "";
            public string time { get; set; } = "";
        }

        public class ItemNote
        {
            public string note { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
        }

        public async Task<string> addImageWorkingAsync(string token, string code, byte[] data)
        {
            try
            {
                string codefile = "";
                SqlUser? user = null;
                SqlLogOrder? m_log = null;

                using (DataContext context = new DataContext())
                {
                    user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return "";
                    }

                    m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(code) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return "";
                    }
                    if (m_log.order!.state!.code > 3)
                    {
                        return "";
                    }
                    //Console.WriteLine(data.Length);
                    codefile = await Program.api_file.saveFileAsync(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.image"), data);
                    if (string.IsNullOrEmpty(codefile))
                    {
                        return "";
                    }
                    if(m_log.images== null)
                    {
                        m_log.images = new List<string>();
                    }
                    m_log.images.Add(codefile);
                    
                   // bool flag = await setStateOrder(user.ID, code, "Add image before", m_log.latitude, m_log.longitude);
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return codefile;
                    }
                    else
                    {
                        return "";
                    }
                }
                
                /*using (DataContext context = new DataContext())
                {
                    while (flagblock)
                    {
                        Thread.Sleep(1);
                    }
                    flagblock = true;

                    ItemNote image = new ItemNote();
                    image.note = m_log.note;
                    image.images.Add(codefile);


                    string note = JsonConvert.SerializeObject(image);

                    bool flag = await setStateOrder(user.ID, code, codefile, m_log.latitude, m_log.longitude);
                    bool flag = await setAction(token, code, "action6", note);
                    if (flag)
                    {
                        flagblock = false;
                        return codefile;
                    }
                    else
                    {
                        flagblock = false;
                        return "";
                    }
                }*/
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> removeImageWorkingAsync(string token, string order, string code)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return false;
                    }

                    SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(order) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return false;
                    }
                    if (m_log.order!.state!.code > 3)
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(code))
                    {
                        return false;
                    }
                    if (m_log.images != null)
                    {
                        m_log.images.Remove(code);
                    }
                    /* ItemNote? m_note = JsonConvert.DeserializeObject<ItemNote>(m_log.note);

                     ItemNote image = new ItemNote();
                     if (m_note != null)
                     {
                         if (m_note.images.Count > 0)
                         {
                             m_note.images.Remove(code);
                         }
                         image.note = "Remove Before Image";
                         image.images = m_note.images;


                     }
                     string note = JsonConvert.SerializeObject(image);

                     bool flag = await setStateOrder(user.ID, order, note, m_log.latitude, m_log.longitude);*/
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> beginWorkOrder(string token, string code, string x, string y, string note)
        {
            using(DataContext context = new DataContext())
            {
                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 4).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }

                SqlOrder? m_order = null;

                if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 3)
                    {
                        return false;
                    }
                }
                else if (m_user.role!.code.CompareTo("manager") == 0 || m_user.role!.code.CompareTo("survey") == 0)
                {
                    m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 3)
                    {
                        return false;
                    }
                }
                else 
                {
                    m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if(m_order.state!.code != 3)
                    {
                        return false;
                    }
                }

                m_order.state = m_state;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                if (!string.IsNullOrEmpty(note))
                {
                    image.note = note;
                    image.images = new List<string>();


                }
                else
                {
                    image.note = string.Format("{0} {1} : {2}", m_user.user, m_order.state!.name, m_order.code);
                    image.images = new List<string>();
                }*/

                bool flag = await setStateOrder(m_user.ID, m_order.code, note, x, y);
                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> finishWorkOrder(string token, string code, string note)
        {
            using (DataContext context = new DataContext())
            {
                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 5).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlOrder? m_order = null;

                if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 4)
                    {
                        return false;
                    }
                }
                else if (m_user.role!.code.CompareTo("manager") == 0 || m_user.role!.code.CompareTo("survey") == 0)
                {
                    m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 4)
                    {
                        return false;
                    }
                }
                else
                {
                    m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 4)
                    {
                        return false;
                    }
                }

                m_order.state = m_state;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                if (!string.IsNullOrEmpty(note))
                {
                    image.note = note;
                    image.images = new List<string>();


                }
                else
                {
                    image.note = string.Format("{0} {1} : {2}", m_user.user, m_order.state!.name, m_order.code);
                    image.images = new List<string>();
                }
                string m_note = JsonConvert.SerializeObject(image);*/

                bool flag = await setStateOrder(m_user.ID, m_order.code, note, "", "");
                if (flag)
                {
                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return flag;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<string> addImageFinishAsync(string token, string code, byte[] data)
        {
            try
            {
                string codefile = "";
                SqlUser? user = null;
                SqlLogOrder? m_log = null;

                using (DataContext context = new DataContext())
                {
                    user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return "";
                    }

                    m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(code) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return "";
                    }
                    if (m_log.order!.state!.code != 5)
                    {
                        return "";
                    }

                    codefile = await Program.api_file.saveFileAsync(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.image"), data);
                    if (string.IsNullOrEmpty(codefile))
                    {
                        return "";
                    }
                    if (m_log.images == null)
                    {
                        m_log.images = new List<string>();
                    }
                    m_log.images.Add(codefile);

                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return codefile;
                    }
                    else
                    {
                        return "";
                    }
                }

               /* using (DataContext context = new DataContext())
                {
                    while (flagblock)
                    {
                        Thread.Sleep(1);
                    }
                    flagblock = true;

                    ItemNote image = new ItemNote();
                    image.note = m_log.note;
                    image.images.Add(codefile);


                    string note = JsonConvert.SerializeObject(image);

                    bool flag = await setAction(token, code, "action7", note);

                    //bool flag = await setStateOrder(user.ID, code, note, m_log.latitude, m_log.longitude);
                    if (flag)
                    {
                        flagblock = false;
                        return codefile;
                    }
                    else
                    {
                        flagblock = false;
                        return "";
                    }
                }*/
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async Task<bool> removeImageFinishAsync(string token, string order, string code)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return false;
                    }

                    SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(order) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return false;
                    }
                    if (m_log.order!.state!.code != 5)
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(code))
                    {
                        return false;
                    }
                    if (m_log.images != null)
                    {
                        m_log.images.Remove(code);
                    }
                    /* ItemNote? m_note = JsonConvert.DeserializeObject<ItemNote>(m_log.note);

                     ItemNote image = new ItemNote();
                     if (m_note != null)
                     {
                         if (m_note.images.Count > 0)
                         {
                             m_note.images.Remove(code);
                         }
                         image.note = "Remove After Image";
                         image.images = m_note.images;


                     }
                     string note = JsonConvert.SerializeObject(image);*/
                    /* ItemImage image = new ItemImage();
                     image.code = code;
                     image.user = user.user;
                     image.time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                     string note = "Remove After :" + JsonConvert.SerializeObject(image);*/

                    //bool flag = await setStateOrder(user.ID, order, note, m_log.latitude, m_log.longitude);

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> addImageSignAsync(string token, string code, byte[] data)
        {
            string codefile = await Program.api_file.saveFileAsync(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.image"), data);
            if (string.IsNullOrEmpty(codefile))
            {
                return "";
            }
            try
            {
                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return "";
                    }

                    SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(code) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return "";
                    }
                    if (m_log.order!.state!.code < 3)
                    {
                        return "";
                    }

                    
                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_log.order;
                    log.user = user;
                    log.latitude = "";
                    log.longitude = "";
                    log.note = "Add Sign";
                    log.images = new List<string>();
                    log.images.Add(codefile);
                    log.time = DateTime.Now.ToUniversalTime();
                    context.logs!.Add(log);

                    int rows = await context.SaveChangesAsync();
                    if (rows > 0)
                    {
                        return codefile;
                    }
                    else
                    {
                        return "";
                    }


                   /* string note = "Add Sign";

                    bool flag = await setStateOrder(user.ID, code, note, "", "");
                    if (flag)
                    {
                        return codefile;
                    }
                    else
                    {
                        return "";
                    }*/
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> removeImageSignAsync(string token, string order, string code)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                    if (user == null)
                    {
                        return false;
                    }

                    SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(order) == 0 && s.order!.isDelete == false).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                    if (m_log == null)
                    {
                        return false;
                    }
                    if (m_log.order!.state!.code < 3)
                    {
                        return false;
                    }

                    if (string.IsNullOrEmpty(code))
                    {
                        return false;
                    }

                    if (m_log.images != null)
                    {
                        m_log.images.Remove(code);
                        m_log.note = "Remove Sign";
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
                    /* ItemNote? m_note = JsonConvert.DeserializeObject<ItemNote>(m_log.note);
                     ItemNote image = new ItemNote();

                     if (m_note != null)
                     {
                         if(m_note.images.Count > 0)
                         {
                             m_note.images.Remove(code);
                         }
                         image.note = "Remove Sign";
                         image.images = m_note.images;


                     }
                     string note = JsonConvert.SerializeObject(image);

                     bool flag = await setStateOrder(user.ID, order, note, "", "");
                     return flag;*/

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> finishOrder(string token, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 6).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.customer)
                                                .FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }


                SqlOrder? m_order = null;

                if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 5)
                    {
                        return false;
                    }
                    if(m_order.customer == null)
                    {
                        return false;
                    }    
                }
                else if (m_user.role!.code.CompareTo("manager") == 0 || m_user.role!.code.CompareTo("survey") == 0)
                {
                    m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 5)
                    {
                        return false;
                    }
                    if (m_order.customer == null)
                    {
                        return false;
                    }
                }
                else
                {
                    m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.customer).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 5)
                    {
                        return false;
                    }
                    if (m_order.customer == null)
                    {
                        return false;
                    }
                }

                m_order.state = m_state;
                m_order.isFinish = true;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                image.note = string.Format("{0} : {1} ", m_order.state!.name, m_order.code);
                image.images = new List<string>();*/

                string note = string.Format("{0} : {1} ", m_order.state!.name, m_order.code);
                bool flag = await setStateOrder(m_user.ID, m_order.code, note, "", "");
                if (flag)
                {
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
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> cancelOrder(string token,string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlState? m_state = context.states!.Where(s => s.isdeleted == false && s.code == 7).FirstOrDefault();
                if (m_state == null)
                {
                    return false;
                }

                if (m_user.role!.code.CompareTo("staff") == 0 )
                {
                    return false;
                }

                SqlOrder? m_order = null;

                if (m_user.role.code.CompareTo("manager") == 0 || m_user.role.code.CompareTo("survey") == 0)
                {
                    m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code > 3)
                    {
                        return false;
                    }
                }
                else
                {
                    m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code > 3)
                    {
                        return false;
                    }
                }

                m_order.isDelete = true;
                m_order.state = m_state;
                m_order.lastestTime = DateTime.Now.ToUniversalTime();

                /*ItemNote image = new ItemNote();
                image.note = string.Format("{0} : {1} ", m_order.state!.name, m_order.code);
                image.images = new List<string>();*/

                string note = string.Format("{0} : {1} ", m_order.state!.name, m_order.code);
                bool flag = await setStateOrder(m_user.ID, m_order.code, note, "", "");
                if (flag)
                {
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

        public class ItemOrderRequest
        {
            public ItemUser user { get; set; } = new ItemUser();
            public ItemAction action { get; set; } = new ItemAction();
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
            public string note { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
            public string time { get; set; } = "";
        }

        public ItemOrderRequest getLogOrder(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new ItemOrderRequest();
            }

            ItemOrderRequest info = new ItemOrderRequest();
            long id = long.Parse(code);
            if (id > 0)
            {

                List<ItemLog> logs = Program.api_log.getListLog();
                if (logs.Count > 0)
                {
                    ItemLog? m_log = logs.Where(s => s.order.id == id).OrderByDescending(s => s.time).FirstOrDefault();
                    if(m_log == null)
                    {
                        return new ItemOrderRequest();
                    }

                    info.user.user = m_log.user.user;
                    info.user.displayName = m_log.user.displayName;
                    info.user.numberPhone = m_log.user.numberPhone;

                    info.action.code = m_log.action.code;
                    info.action.name = m_log.action.name;
                    info.action.des = m_log.action.des;

                    info.longitude = m_log.longitude;
                    info.latitude = m_log.latitude;
                    info.note = m_log.note;
                    info.time = m_log.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                }
            }
            return info;
        }

        public List<ItemOrderRequest> getOrderLogs(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new List<ItemOrderRequest>();
            }

            List<ItemOrderRequest> infos = new List<ItemOrderRequest>();
            long id = long.Parse(code);
            if (id > 0)
            {

                List<ItemLog> logs = Program.api_log.getListLog();
                if (logs.Count > 0)
                {
                    List<ItemLog>? mLogs = logs.Where(s => s.order.id == id).OrderByDescending(s => s.time).ToList();
                    if(mLogs.Count > 0)
                    {
                        foreach(ItemLog m_log in mLogs)
                        {
                            ItemOrderRequest info = new ItemOrderRequest();
                            info.user.user = m_log.user.user;
                            info.user.displayName = m_log.user.displayName;
                            info.user.numberPhone = m_log.user.numberPhone;

                            info.action.code = m_log.action.code;
                            info.action.name = m_log.action.name;
                            info.action.des = m_log.action.des;

                            info.longitude = m_log.longitude;
                            info.latitude = m_log.latitude;
                            info.note = m_log.note;
                            info.images = m_log.images;
                            info.time = m_log.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                            infos.Add(info);
                        }
                    }
                    

                }
            }
            return infos;
        }


        public class ItemProfile
        {
            public string name { get; set; } = "";
            public string phone { get; set; } = "";
            public string addressCustomer { get; set; } = "";
            public string addressWater { get; set; } = "";
            public string addressContract { get; set; } = "";
        }

        public class ItemInfoOrder
        {
            public string code { get; set; } = "";
            public ItemProfile profile { get; set; } = new ItemProfile();
            public ItemUser receiver { get; set; } = new ItemUser();
            public ItemUser manager { get; set; } = new ItemUser();
            public ItemUser worker { get; set; } = new ItemUser();
            public ItemCustomer customer  { get; set; } = new ItemCustomer();
            public string note { get; set; } = "";
            public ItemType type { get; set; } = new ItemType();
            public ItemService service { get; set; } = new ItemService();
            public ItemStateOrder state { get; set; } = new ItemStateOrder();
            public List<ItemOrderRequest> logActions { get; set; } = new List<ItemOrderRequest>();
            public string createTime { get; set; } = "";
            public string lastestTime { get; set; } = "";

        }

        public ItemInfoOrder getInfoOrder(string token, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.receiver)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.manager)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.type)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.service)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.receiver)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.worker)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.type)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.service)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if(m_user == null)
                {
                    return new ItemInfoOrder();
                }

                SqlOrder? item = null;

                if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    item = m_user.workerOrders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if(item == null)
                    {
                        return new ItemInfoOrder();
                    }
                }
                else if (m_user.role!.code.CompareTo("manager") == 0)
                {
                    item = m_user.managerOrders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (item == null)
                    {
                        return new ItemInfoOrder();
                    }
                }
                else
                {
                    item = context.orders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0)
                                                       .Include(s => s.customer)
                                                       .Include(s => s.receiver)
                                                       .Include(s => s.manager)
                                                       .Include(s => s.worker)
                                                       .Include(s => s.service)
                                                       .Include(s => s.type)
                                                       .Include(s => s.state)
                                                       .FirstOrDefault();
                    if(item == null)
                    {
                        return new ItemInfoOrder();
                    }
                }

                ItemInfoOrder tmp = new ItemInfoOrder();

                tmp.code = item.code;
                tmp.profile.name = item.name;
                tmp.profile.phone = item.phone;
                tmp.profile.addressCustomer = item.addressCustomer;
                tmp.profile.addressWater = item.addressWater;
                tmp.profile.addressContract = item.addressContract;
                if (item.customer != null)
                {
                    if (item.customer.isdeleted == false)
                    {
                        ItemCustomer customer = new ItemCustomer();
                        customer.maDB = item.customer.code;
                        customer.sdt = item.customer.phone;
                        customer.tenkh = item.customer.name;
                        customer.diachi = item.customer.address;
                        customer.note = item.customer.note;
                        customer.x = item.customer.latitude;
                        customer.y = item.customer.longitude;
                        if (item.customer.images != null)
                        {
                            customer.images = item.customer.images;
                        }

                        tmp.customer = customer;
                    }

                }

                if (item.receiver != null)
                {
                    ItemUser receiver = new ItemUser();
                    receiver.user = item.receiver.user;
                    receiver.displayName = item.receiver.displayName;
                    receiver.numberPhone = item.receiver.phoneNumber;

                    tmp.receiver = receiver;
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

                //ItemOrderRequest m_log = getLogOrder(item.ID.ToString());
                //tmp.logActions.Add(m_log);
                tmp.logActions = getOrderLogs(item.ID.ToString());
                tmp.createTime = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                tmp.lastestTime = item.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                return tmp;
            }
        }

        public List<ItemInfoOrder> getListOrder(string token, DateTime begin, DateTime end)
        {
            DateTime m_begin = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0);
            DateTime m_end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            List<ItemInfoOrder> list = new List<ItemInfoOrder>();

            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.receiver)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.manager)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.type)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.service)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if (m_user == null)
                {
                    return new List<ItemInfoOrder>();
                }

                List<SqlOrder> orders = context.orders!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.createdTime) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.createdTime) > 0 && s.isDelete == false)
                                                       .Include(s => s.customer)
                                                       .Include(s => s.receiver)
                                                       .Include(s => s.manager)
                                                       .Include(s => s.worker)
                                                       .Include(s => s.service)
                                                       .Include(s => s.type)
                                                       .Include(s => s.state)
                                                       .OrderByDescending(s => s.createdTime)
                                                       .ToList();
                if (orders.Count < 1)
                {
                    return new List<ItemInfoOrder>();
                }
                //Console.WriteLine("Version : 22-05-2023");
                List<SqlOrder> mOrders = new List<SqlOrder>();
                if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    mOrders = m_user.workerOrders!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.createdTime) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.createdTime) > 0 && s.isDelete == false).OrderByDescending(s => s.createdTime).ToList();
                }
                else
                {
                    if (m_user.role!.code.CompareTo("manager") == 0)
                    {
                        mOrders = orders.Where(s => s.service!.code.CompareTo("LM") == 0 && s.state!.code != 0).ToList();

                    }
                    else if (m_user.role!.code.CompareTo("survey") == 0)
                    {
                        mOrders = orders.Where(s => s.state!.code != 0).ToList();

                    }
                    else
                    {
                        mOrders = orders;
                    }
                }

                /*  if (Program.api_user.checkSurvey(token) == 0)
                  {
                      m_order = orders.Where(s => s.state!.code != 0 && (s.service!.code.CompareTo("SC") == 0 || s.service!.code.CompareTo("TT") == 0)).ToList();
                  }
                  if (Program.api_user.checkCS(token) == 0)
                  {
                      m_order = orders.Where(s => s.state!.code == 0).ToList();
                  }
                  if (Program.api_user.checkAdmin(token) == 0)
                  {
                      m_order = orders;
                  }*/

                foreach (SqlOrder item in mOrders)
                {
                    ItemInfoOrder tmp = new ItemInfoOrder();

                    tmp.code = item.code;
                    tmp.profile.name = item.name;
                    tmp.profile.phone = item.phone;
                    tmp.profile.addressCustomer = item.addressCustomer;
                    tmp.profile.addressWater = item.addressWater;
                    tmp.profile.addressContract = item.addressContract;
                    if (item.customer != null)
                    {
                        if (item.customer.isdeleted == false)
                        {
                            ItemCustomer customer = new ItemCustomer();
                            customer.maDB = item.customer.code;
                            customer.sdt = item.customer.phone;
                            customer.tenkh = item.customer.name;
                            customer.diachi = item.customer.address;
                            customer.note = item.customer.note;
                            customer.x = item.customer.latitude;
                            customer.y = item.customer.longitude;
                            if (item.customer.images != null)
                            {
                                customer.images = item.customer.images;
                            }

                            tmp.customer = customer;
                        }

                    }

                    if (item.receiver != null)
                    {
                        ItemUser receiver = new ItemUser();
                        receiver.user = item.receiver.user;
                        receiver.displayName = item.receiver.displayName;
                        receiver.numberPhone = item.receiver.phoneNumber;

                        tmp.receiver = receiver;
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

                    //ItemOrderRequest m_log = getLogOrder(item.ID.ToString());
                    //tmp.logActions.Add(m_log);
                    tmp.logActions = getOrderLogs(item.ID.ToString());
                    tmp.createTime = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.lastestTime = item.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                    list.Add(tmp);
                }
                return list;
            }    
            
        }

        public class ItemOrder
        {
            public string code { get; set; } = "";
            public ItemProfile profile { get; set; } = new ItemProfile();
            public ItemUser receiver { get; set; } = new ItemUser();
            public ItemUser manager { get; set; } = new ItemUser();
            public ItemUser worker { get; set; } = new ItemUser();
            public ItemCustomer customer { get; set; } = new ItemCustomer();
            public string note { get; set; } = "";
            public ItemType type { get; set; } = new ItemType();
            public ItemService service { get; set; } = new ItemService();
            public ItemStateOrder state { get; set; } = new ItemStateOrder();
            public ItemOrderRequest logAction { get; set; } = new ItemOrderRequest();
            public string createTime { get; set; } = "";
            public string lastestTime { get; set; } = "";

        }

        public class ItemLogOrder
        {
            public ItemOrder order { get; set; } = new ItemOrder();
            public ItemOrderRequest logAction { get; set; } = new ItemOrderRequest();
            public string time { get; set; } = "";
        }

        public List<ItemLogOrder> getFindOrder(string code)
        {
            List<ItemLogOrder> list = new List<ItemLogOrder>();
            using (DataContext context = new DataContext())
            {

                List<SqlOrder>? items = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && (s.code.CompareTo(code) == 0 || s.phone.CompareTo(code) == 0 || s.addressWater.CompareTo(code) == 0))
                                                        .Include(s => s.type)
                                                        .Include(s => s.service)
                                                        .Include(s => s.state)
                                                        .Include(s => s.customer)
                                                        .Include(s => s.receiver)
                                                        .Include(s => s.manager)
                                                        .Include(s => s.worker)
                                                        .ToList();
                if (items.Count < 1)
                {
                    return new List<ItemLogOrder>();
                }
                foreach (SqlOrder item in items)
                {
                    ItemLogOrder m_info = new ItemLogOrder();

                    ItemOrder tmp = new ItemOrder();
                    tmp.code = item.code;
                    tmp.profile.name = item.name;
                    tmp.profile.phone = item.phone;
                    tmp.profile.addressCustomer = item.addressCustomer;
                    tmp.profile.addressWater = item.addressWater;
                    tmp.profile.addressContract = item.addressContract;
                    if (item.customer != null)
                    {
                        ItemCustomer customer = new ItemCustomer();
                        customer.maDB = item.customer.code;
                        customer.sdt = item.customer.phone;
                        customer.tenkh = item.customer.name;
                        customer.diachi = item.customer.address;
                        customer.note = item.customer.note;
                        customer.x = item.customer.latitude;
                        customer.y = item.customer.longitude;
                        if (item.customer.images != null)
                        {
                            customer.images = item.customer.images;
                        }

                        tmp.customer = customer;

                    }

                    if (item.receiver != null)
                    {
                        ItemUser receiver = new ItemUser();
                        receiver.user = item.receiver.user;
                        receiver.displayName = item.receiver.displayName;
                        receiver.numberPhone = item.receiver.phoneNumber;

                        tmp.receiver = receiver;
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

                    m_info.order = tmp;

                    m_info.logAction = getLogOrder(item.ID.ToString());

                    m_info.time = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    list.Add(m_info);
                }

                return list;
            }
        }

        public ItemCustomer getInfoCustmer(string code)
        {
            ItemCustomer info = new ItemCustomer();

            List<ItemCustomer> customers = Program.api_customer.listCustomer();

            if(customers.Count > 0)
            {
                ItemCustomer? tmp = customers.Where(s => s.maDB.CompareTo(code) == 0 || s.sdt.CompareTo(code) == 0 || s.diachi.CompareTo(code) == 0).FirstOrDefault();
                if(tmp == null)
                {
                    return new ItemCustomer();
                }

                info = tmp;
            }

            return info;
        }

    }
}
