using ServerWater2.Models;
using Newtonsoft.Json;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyState;
using Microsoft.EntityFrameworkCore;
using static ServerWater2.APIs.MyType;
using static ServerWater2.APIs.MyService;
using static ServerWater2.APIs.MyAction;
using static ServerWater2.APIs.MyLogOrder;
using Serilog;
using static ServerWater2.Program;
using static ServerWater2.APIs.MyGroup;
using static ServerWater2.APIs.MyViewForm;
using static ServerWater2.APIs.MyArea;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using FirebaseAdmin.Messaging;

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

        //public async Task<bool> testNotificationOrder(string user, string code, string action)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.code.CompareTo(code) == 0).Include(s => s.group).ThenInclude(s => s!.areas).Include(s => s.area).Include(s => s.state).Include(s => s.service).Include(s => s.type).Include(s => s.worker).FirstOrDefault();
        //        if (order == null)
        //        {
        //            return false;
        //        }
        //        if (order.group!.areas == null)
        //        {
        //            return false;
        //        }

        //        SqlUser? muser = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0).FirstOrDefault();
        //        if (muser == null)
        //        {
        //            return false;
        //        }

        //        if (action.CompareTo("Moi") == 0)
        //        {
        //            string tmp = await createNewOrderAsync(order.name, order.phone, order.persons, order.addressCustomer, order.group.code, order.area!.code, order.addressWater, order.addressContract, order.service!.code, order.type!.code, order.certificates, order.note);

        //        }
        //        if (action.CompareTo("TN") == 0)
        //        {
        //            await setAction(muser.token, code, "XN", "", "", "");
        //            await setAction(muser.token, code, "DTN", "", "", "");
        //        }
        //        if (action.CompareTo("KS") == 0)
        //        {
        //            bool check = await setAssginSurveyOrder(muser.token, code, "CT");
        //        }
        //        if (action.CompareTo("DL") == 0)
        //        {
        //            await setAction(muser.token, code, "HTKS", "", "", "");
        //            bool check = await setAssginWorkerOrder(muser.token, code, "staff");
        //        }
        //        return true;
        //    }
        //}


        public class ItemNotifyOrder
        {
            public string order { get; set; } = "";
            public string service { get; set; } = "";
            public string customer { get; set; } = "";
        }

        public async Task<bool> saveNotification(string notification, string token)
        {
            if (string.IsNullOrEmpty(notification) || string.IsNullOrEmpty(token))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                try
                {
                    ItemNotifyOrder? item = JsonConvert.DeserializeObject<ItemNotifyOrder>(notification);
                    if (item == null)
                    {
                        return false;
                    }
                    SqlUser? m_user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false && s.isClear == false).FirstOrDefault();
                    if (m_user == null)
                    {
                        return false;
                    }
                    Log.Information(String.Format("{0} is offline. Saved notification {1} :", m_user.user, notification));
                    if (string.IsNullOrEmpty(m_user.notification))
                    {
                        List<ItemNotifyOrder> items = new List<ItemNotifyOrder>();
                        items.Add(item);
                        m_user.notification = JsonConvert.SerializeObject(items);
                    }
                    else
                    {
                        List<ItemNotifyOrder>? items = JsonConvert.DeserializeObject<List<ItemNotifyOrder>>(m_user.notification);
                        if (items != null)
                        {
                            items.Add(item);
                        }
                        m_user.notification = JsonConvert.SerializeObject(items);
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
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    return false;
                }

                //if (string.IsNullOrEmpty(user))
                //{
                //    List<SqlUser>? users = context.users!.Where(s => s.isdeleted == false && s.isClear == false).Include(s => s.role).ToList();
                //    if (users.Count > 0)
                //    {
                //        if (state == 0)
                //        {
                //            users = users.Where(s => s.role!.code.CompareTo("admin") == 0 || s.role!.code.CompareTo("receiver") == 0).ToList();

                //        }
                //        else
                //        {
                //            users = users.Where(s => s.role!.code.CompareTo("manager") == 0).ToList();

                //        }
                //        foreach (SqlUser m_user in users)
                //        {
                //            if (string.IsNullOrEmpty(m_user.notification))
                //            {
                //                List<ItemNotifyOrder> items = new List<ItemNotifyOrder>();
                //                items.Add(item);
                //                m_user.notification = JsonConvert.SerializeObject(items);
                //            }
                //            else
                //            {
                //                List<ItemNotifyOrder>? items = JsonConvert.DeserializeObject<List<ItemNotifyOrder>>(m_user.notification);
                //                if (items != null)
                //                {
                //                    items.Add(item);
                //                }
                //                m_user.notification = JsonConvert.SerializeObject(items);
                //            }
                //        }
                //    }
                //}
                //else
                //{

                //}
            }
        }

        public async Task<string> setAction(string token, string order, string action, string note, string latitude, string longitude)
        {

            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return "";
                }

                SqlAction? m_action = context.actions!.Where(s => s.code.CompareTo(action) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_action == null)
                {
                    return "";
                }

                SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(order) == 0)
                                                   .Include(s => s.state)
                                                   .Include(s => s.service)
                                                   .FirstOrDefault();
                if (m_order == null)
                {
                    return "";
                }
                if (m_order.state!.code < 5 && m_user.role!.code.CompareTo("staff") == 0)
                {
                    return "";
                }
                if (m_order.state!.code > 6 && m_user.role!.code.CompareTo("survey") == 0)
                {
                    return "";
                }

                List<SqlState> states = context.states!.Where(s => s.isdeleted == false).ToList();
                bool writeFlag = false;
                string person = "";
                switch (action)
                {
                    case "XN":
                        if (m_order.state!.code != 0)
                        {
                            return "";
                        }

                        if (m_user.receiverOrders != null)
                        {
                            return "";
                        }    

                        m_order.receiver = m_user;
                        m_order.state = states.Where(s => s.code == 1).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "1";

                        break;
                    case "DTN":

                        if (m_order.state!.code != 1)
                        {
                            return "";
                        }
                        if(m_order.manager != null)
                        {
                            return "";
                        }    
                        
                        m_order.manager = m_user;
                        m_order.state = states.Where(s => s.code == 2).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();

                        break;
                    case "PCKS":

                        if (m_order.state!.code != 2)
                        {
                            return "";
                        }
                        
                        m_order.survey = m_user;
                        m_order.state = states.Where(s => s.code == 3).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "2";

                        break;
                    case "PCKSL":

                        if (m_order.state!.code > 3)
                        {
                            return "";
                        }

                        m_order.survey = m_user;
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "2";

                        break;
                    case "KS":

                        if (m_order.state!.code > 3)
                        {
                            return "";
                        }
                        
                        m_order.state = states.Where(s => s.code == 4).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        break;
                    case "HTKS":

                        if (m_order.state!.code > 4)
                        {
                            return "";
                        }
                        
                        m_order.state = states.Where(s => s.code == 5).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "0";

                        break;
                    case "DPC":

                        if(m_order.state!.code > 5)
                        {
                            return "";
                        }    
                        
                        m_order.worker = m_user;
                        m_order.state = states.Where(s => s.code == 6).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "3";

                        break;
                    case "PCCVL":

                        if (m_order.state!.code < 5)
                        {
                            return "";
                        }

                        m_order.worker = m_user;
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        writeFlag = true;
                        person = "3";

                        break;
                    case "ABDCV":

                        if (m_order.state!.code != 6)
                        {
                            return "";
                        }

                        m_order.state = states.Where(s => s.code == 7).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        break;
                    case "AKTCV":

                        if (m_order.state!.code != 7)
                        {
                            return "";
                        }

                        m_order.state = states.Where(s => s.code == 8).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        break;
                    case "NT":

                        if (m_order.state!.code != 8)
                        {
                            return "";
                        }

                        m_order.state = states.Where(s => s.code == 9).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        break;
                    case "KT":

                        if (m_order.state!.code != 9)
                        {
                            return "";
                        }

                        m_order.state = states.Where(s => s.code == 10).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        m_order.isFinish = true;
                        break;
                    case "DH":

                        if (m_order.state!.code > 3)
                        {
                            return "";
                        }

                        m_order.state = states.Where(s => s.code == 11).FirstOrDefault();
                        m_order.lastestTime = DateTime.Now.ToUniversalTime();
                        m_order.isDelete = true;

                        break;

                }

                if(writeFlag)
                {
                    ItemNotifyOrder item = new ItemNotifyOrder();
                    item.order = order;
                    item.service = m_order.service!.code;
                    item.customer = m_order.name;
                    string data = JsonConvert.SerializeObject(item);
                    if(m_user.role!.code.CompareTo("survey") == 0 || m_user.role!.code.CompareTo("staff") == 0)
                    {
                        bool flag = await saveNotification(data, m_user.token);
                        if (!flag)
                        {
                            Log.Debug(string.Format("Save Notification : {0} for user : {1}", flag, m_user.displayName));
                        }
                    }

                    List<HttpNotification> notifications = Program.httpNotifications.Where(s => s.person.CompareTo(person) == 0).ToList();
                    foreach (HttpNotification notification in notifications)
                    {
                        Log.Debug("Order : {0} - Customer : {1}", order, item.customer);
                        notification.messagers.Add(data);
                    }
                }

                SqlLogOrder log = new SqlLogOrder();
                log.ID = DateTime.Now.Ticks;
                log.action = m_action;
                log.order = m_order;
                log.user = m_user;
                log.latitude = latitude;
                log.longitude = longitude;
                log.note = note;
                log.time = DateTime.Now.ToUniversalTime();
                context.logs!.Add(log);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return log.ID.ToString();
                }
                else
                {
                    return "";
                }

            }
        }

        public class HttpItemOrder
        {
            public string code { get; set; } = "";
            public List<string> certificates { get; set; } = new List<string>();
        }

        public async Task<string> createUpdateOrderAsync(string code, string name, string customer, string phone, string personNumbers, string addressCustomer, string group, string area, string addressWater, string addressContract, string service, string type, List<string> certificates, string note)
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
                    m_order.persons = personNumbers;
                    m_order.addressCustomer = addressCustomer;
                    m_order.group = context.groups!.Where(s => s.isdeleted == false && s.code.CompareTo(group) == 0).Include(s => s.areas).FirstOrDefault();
                    if(m_order.group!.areas != null)
                    {
                        SqlArea? m_area = m_order.group.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                        if(m_area == null)
                        {
                            return "";
                        }
                        m_order.area = m_area;
                    }    
                    m_order.addressContract = addressContract;
                    m_order.addressWater = addressWater;
                    m_order.note = note;
                    m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                    m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                    m_order.certificates = new List<string>();
                    foreach (string tmp in certificates)
                    {
                        SqlCertificate? m_certificate = context.certificates!.Where(s => s.isdeleted == false && s.code.CompareTo(tmp) == 0).FirstOrDefault();
                        if(m_certificate == null)
                        {
                            Log.Debug("Error certificate !!!");
                            return "";
                        }
                        
                        m_order.certificates.Add(tmp);
                    }
                    m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    m_order.createdTime = DateTime.Now.ToUniversalTime();
                    m_order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                    context.orders!.Add(m_order);



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
                    m_order.addressContract = addressContract;

                    m_order.note = note;
                    m_order.service = context.services!.Where(s => s.isdeleted == false && s.code.CompareTo(service) == 0).FirstOrDefault();
                    m_order.type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                    m_order.lastestTime = DateTime.Now.ToUniversalTime();
                    m_order.createdTime = DateTime.Now.ToUniversalTime();
                    m_order.state = context.states!.Where(s => s.isdeleted == false && s.code == 0).FirstOrDefault();

                    context.orders!.Add(m_order);



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

        public async Task<string> createNewOrderAsync(string name, string phone, string personNumbers, string addressCustomer, string group, string area,string addressWater, string addressContract, string service, string type, List<string> certificates, string note)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(type) || certificates == null)
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {
                SqlType? m_type = context.types!.Where(s => s.code.CompareTo(type) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_type == null)
                {
                    return "";
                }

                SqlService? m_service = context.services!.Where(s => s.code.CompareTo(service) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_service == null)
                {
                    return "";
                }

                string order = await createUpdateOrderAsync("", name, "", phone, personNumbers, addressCustomer, group, area, addressWater, addressContract, service, type, certificates, note);

                if (!string.IsNullOrEmpty(order))
                {
                    string action = await setAction("1234567890", order, "DXN", "", "", "");
                    if (string.IsNullOrEmpty(action))
                    {
                        return "";
                    }
                    else
                    {
                        ItemNotifyOrder item = new ItemNotifyOrder();
                        item.order = order;
                        item.service = m_service.name;
                        item.customer = name;
                        string m_item = JsonConvert.SerializeObject(item);
                        
                        List<HttpNotification>? notifications = Program.httpNotifications.Where(s => s.person.CompareTo("0") == 0).ToList();
                        foreach (HttpNotification notification in notifications)
                        {
                            Log.Debug("Order : {0} - Customer : {1}", order, item.customer);
                            notification.messagers.Add(m_item);
                        }
                    }
                    HttpItemOrder temp = new HttpItemOrder();
                    temp.code = order;
                    temp.certificates = certificates;

                    string data = JsonConvert.SerializeObject(temp);
                    return data;
                }
                else
                {
                    return "";
                }
            }
        }

        public async Task<string> uploadImageFileAsync(string code, string certificate, byte[] data)
        {
            try
            {
                string codefile = "";

                using (DataContext context = new DataContext())
                {
                    //Console.WriteLine(data.Length);
                    codefile = await Program.api_file.saveFileAsync(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.image"), data);
                    if (string.IsNullOrEmpty(codefile))
                    {
                        return "";
                    }

                    SqlCertificate? m_certificate = context.certificates!.Where(s => s.code.CompareTo(certificate) == 0 && s.isdeleted == false).FirstOrDefault();
                    if (m_certificate == null)
                    {
                        return "";
                    }

                    SqlOrder? m_order = context.orders!.Where(s => s.code.CompareTo(code) == 0 && s.isDelete == false && s.isFinish == false).FirstOrDefault();
                    if (m_order == null)
                    {
                        return "";
                    }

                    string? tmp = m_order.certificates!.Where(s => s.CompareTo(certificate) == 0).FirstOrDefault();
                    if (tmp == null)
                    {
                        return "";
                    }
                    List<ItemCertificate> items = new List<ItemCertificate>();
                    if (string.IsNullOrEmpty(m_order.document))
                    {
                        ItemCertificate item = new ItemCertificate();
                        item.code = m_certificate.code;
                        item.name = m_certificate.name;
                        item.images.Add(codefile);

                        items.Add(item);
                        m_order.document = JsonConvert.SerializeObject(items);
                    }
                    else
                    {
                        List<ItemCertificate>? mItems = JsonConvert.DeserializeObject<List<ItemCertificate>>(m_order.document);
                        if (mItems != null)
                        {
                            ItemCertificate? m_item = mItems.Where(s => s.code.CompareTo(certificate) == 0).FirstOrDefault();
                            if (m_item == null)
                            {
                                ItemCertificate item = new ItemCertificate();
                                item.code = m_certificate.code;
                                item.name = m_certificate.name;
                                item.images.Add(codefile);

                                mItems.Add(item);
                            }
                            else
                            {
                                m_item.images.Add(codefile);
                            }

                        }
                        m_order.document = JsonConvert.SerializeObject(mItems);
                    }



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

            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async Task<string> createRequestOrderAsync(string code, string name, string phone, string customer, string addressCustomer, string addressContract, string service, string note)
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

                string order = await createUpdateOrderAsync(code, name, customer, phone, "", addressCustomer, "", "", "", addressContract, service, "", new List<string>(), note);
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

        public async Task<bool> setCustomerAsync(string token, string maDB, string code)
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


                SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                if (m_order == null)
                {
                    return false;
                }

                if (m_order.state!.code < 1)
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

        public async Task<bool> setAssginSurveyOrder(string token, string code, string user)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.managerOrders!).ThenInclude(s => s.state).Include(s => s.receiverOrders!).ThenInclude(s => s.state).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlOrder? order = null;


                if (m_user.role!.code.CompareTo("admin") == 0)
                {
                    order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if (order.manager == null)
                    {
                        await setAction(token, code, "DTN", "", "", "");
                    }


                }
                else if (m_user.role!.code.CompareTo("receiver") == 0)
                {

                    order = m_user.receiverOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    

                }
                else
                {
                    order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    
                }

                SqlUser? servey = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0).Include(s => s.role).FirstOrDefault();
                if (servey == null || servey.role!.code.CompareTo("staff") == 0)
                {
                    return false;
                }
                string tmp = await setAction(servey.token, code, "PCKS", "", "", "");
                if(string.IsNullOrEmpty(tmp))
                {
                    return false;
                }

                return true;
            }
        }

        public class ItemMyJson
        {
            public string code { get; set; } = "";
            public List<ItemJson> datas { get; set; } = new List<ItemJson>();
        }

        public async Task<bool> createSurveyOrderAsync(string token, string order, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            try
            {
                ItemMyJson? m_json = null;
                try
                {
                    m_json = JsonConvert.DeserializeObject<ItemMyJson>(data);
                    if (m_json == null)
                    {
                        return false;
                    }
                }
                catch(Exception e)
                {
                    Log.Debug(string.Format("Data of form : {0}", data));
                    return false;
                    //string m_data = data.Replace("\\", string.Empty);
                    //m_json = JsonConvert.DeserializeObject<ItemMyJson>(m_data);
                    //if (m_json == null)
                    //{
                    //    return false;
                    //}
                }

                long id = long.Parse(order);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return false;
                        }

                        SqlViewForm? m_form =  context.forms!.Where(s => s.code.CompareTo(m_json.code) == 0 && s.isdeleted == false).FirstOrDefault();
                        if(m_form == null)
                        {
                            return false;
                        }
                        
                        SqlLogOrder? m_log = context.logs!.Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).FirstOrDefault();
                        if (m_log == null)
                        {
                            return false;
                        }
                       
                        m_log.note = JsonConvert.SerializeObject(m_json.datas);
                        m_log.time = DateTime.Now.ToUniversalTime();

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
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> addImageSerrveyAsync(string token, string code, byte[] data)
        {
            try
            {
                string codefile = "";
                long id = long.Parse(code);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return "";
                        }

                        SqlLogOrder? m_log = context.logs!.Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).FirstOrDefault();
                        if (m_log == null)
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
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> removeImageServeyAsync(string token, string order, string code)
        {
            try
            {
                long id = long.Parse(order);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return false;
                        }

                        SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                        if (m_log == null)
                        {
                            return false;
                        }
                        
                        if (string.IsNullOrEmpty(code))
                        {
                            return false;
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
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> removeImageFinishServeyAsync(string token, string order, string code)
        {
            try
            {
                long id = long.Parse(order);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return false;
                        }

                        SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                        if (m_log == null)
                        {
                            return false;
                        }
                        if (m_log.order!.state!.code != 4)
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
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> replaceAssignOrderAsync(string token, string code, string user)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.managerOrders!).ThenInclude(s => s.state).Include(s => s.receiverOrders!).ThenInclude(s => s.state).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlOrder? order = null;


                if (m_user.role!.code.CompareTo("admin") == 0)
                {
                    order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                }
                else if (m_user.role!.code.CompareTo("receiver") == 0)
                {

                    order = m_user.receiverOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                }
                else
                {
                    order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                }

                SqlUser? staff = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0).Include(s => s.role).FirstOrDefault();
                if (staff == null)
                {
                    return false;
                }

                if (order.state!.code < 4)
                {
                    if (staff.role!.code.CompareTo("staff") == 0)
                    {
                        return false;
                    }

                    string tmp = await setAction(staff.token, code, "PCKSL", "", "", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return false;
                    }
                }
                else
                {
                    if (staff.role!.code.CompareTo("survey") == 0)
                    {
                        return false;
                    }

                    string tmp = await setAction(staff.token, code, "PCCVL", "", "", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return false;
                    }
                }

                    

                return true;
            }

        }

        public async Task<bool> setAssginWorkerOrder(string token, string code, string user)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.managerOrders!).ThenInclude(s => s.state).Include(s => s.receiverOrders!).ThenInclude(s => s.state).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                SqlOrder? order = null;


                if (m_user.role!.code.CompareTo("admin") == 0)
                {
                    order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if (order.manager == null)
                    {
                        await setAction(token, code, "DTN", "", "", "");
                    }


                }
                else if (m_user.role!.code.CompareTo("receiver") == 0)
                {

                    order = m_user.receiverOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                }
                else
                {
                    order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if (order.state!.code > 5)
                    {
                        return false;
                    }
                }

                SqlUser? worker = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0).Include(s => s.role).FirstOrDefault();
                if (worker == null || worker.role!.code.CompareTo("survey") == 0)
                {
                    return false;
                }

                string tmp = await setAction(worker.token, code, "DPC", "", "", "");
                if(string.IsNullOrEmpty(tmp))
                {
                    return false;
                }    

                return true;
            }
        }

        public async Task<string> addImageAsync(string token, string code, byte[] data)
        {
            try
            {
                string codefile = "";
                SqlUser? user = null;
                SqlLogOrder? m_log = null;
                long id = long.Parse(code);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return "";
                        }

                        m_log = context.logs!.Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).FirstOrDefault();
                        if (m_log == null)
                        {
                            return "";
                        }
                        if (m_log.order!.state!.code > 5)
                        {
                            return "";
                        }
                        //Console.WriteLine(data.Length);
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
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> removeImageAsync(string token, string order, string code)
        {
            try
            {
                long id = long.Parse(order);
                if (id > 0)
                {
                    using (DataContext context = new DataContext())
                    {
                        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                        if (user == null)
                        {
                            return false;
                        }

                        SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.ID == id).Include(s => s.order).ThenInclude(s => s!.state).OrderByDescending(s => s.time).FirstOrDefault();
                        if (m_log == null)
                        {
                            return false;
                        }
                        if (m_log.order!.state!.code > 5)
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
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
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
            public string id { get; set; } = "";
            public ItemUser user { get; set; } = new ItemUser();
            public ItemAction action { get; set; } = new ItemAction();
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
            public string note { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
            public string time { get; set; } = "";
        }

        public ItemAction getActionUpdate(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new ItemAction();
            }

            ItemAction info = new ItemAction();
            using (DataContext context = new DataContext())
            {
                SqlLogOrder? m_log = context.logs!.Include(s => s.order).Where(s => s.order!.code.CompareTo(code) == 0).OrderByDescending(s => s.time).Include(s => s.action).FirstOrDefault();
                if (m_log == null)
                {
                    return new ItemAction();
                }

                info.code = m_log.action!.code;
                info.name = m_log.action!.name;
            }
            return info;
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
                    if (m_log == null)
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
                    if (mLogs.Count > 0)
                    {
                        foreach (ItemLog m_log in mLogs)
                        {
                            ItemOrderRequest info = new ItemOrderRequest();
                            info.id = m_log.id.ToString();
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

        public class ItemCertificate
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
        }
        public class ItemProfile
        {
            public string name { get; set; } = "";
            public string phone { get; set; } = "";
            public string persons { get; set; } = "";
            public string addressCustomer { get; set; } = "";
            public string addressWater { get; set; } = "";
            public string addressContract { get; set; } = "";
        }

        public class ItemInfoOrder
        {
            public string code { get; set; } = "";
            public MyItemGroup group { get; set; } = new MyItemGroup();
            public MyItemArea area { get; set; } = new MyItemArea();
            public ItemProfile profile { get; set; } = new ItemProfile();
            public List<ItemCertificate> filesCustomer { get; set; } = new List<ItemCertificate>();
            public ItemUser receiver { get; set; } = new ItemUser();
            public ItemUser manager { get; set; } = new ItemUser();
            public ItemUser survey { get; set; } = new ItemUser();
            public ItemUser worker { get; set; } = new ItemUser();
            public ItemCustomer customer { get; set; } = new ItemCustomer();
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
                List<SqlCertificate>? sqlCertificates = context.certificates!.Where(s => s.isdeleted == false).ToList();

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).AsNoTracking().FirstOrDefault();
                if (m_user == null)
                {
                    return new ItemInfoOrder();
                }

                SqlOrder? item = context.orders!.Where(s => s.code.CompareTo(code) == 0)
                                                       .Include(s => s.customer)
                                                       .Include(s => s.receiver)
                                                       .Include(s => s.manager)
                                                       .Include(s => s.survey)
                                                       .Include(s => s.worker)
                                                       .Include(s => s.service)
                                                       .Include(s => s.type)
                                                       .Include(s => s.state)
                                                       .Include(s => s.group)
                                                       .Include(s => s.area)
                                                       .FirstOrDefault();
                if (item == null)
                {
                    return new ItemInfoOrder();
                }


                ItemInfoOrder tmp = new ItemInfoOrder();

                tmp.code = item.code;
                tmp.group.code = item.group!.code;
                tmp.group.name = item.group!.name;
                tmp.area.code = item.area!.code;
                tmp.area.name = item.area!.name;
                tmp.profile.name = item.name;
                tmp.profile.phone = item.phone;
                tmp.profile.persons = item.persons;
                tmp.profile.addressCustomer = item.addressCustomer;
                tmp.profile.addressWater = item.addressWater;
                tmp.profile.addressContract = item.addressContract;
                if (item.customer != null)
                {
                    if (item.customer.isdeleted == false)
                    {
                        ItemCustomer customer = new ItemCustomer();
                        customer.maDB = item.customer.code;
                        customer.route = string.IsNullOrEmpty(item.customer.route) ? "Coming Soon !!!" : item.customer.route;
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

                if (item.certificates != null)
                {
                    if (!string.IsNullOrEmpty(item.document))
                    {
                        List<ItemCertificate>? certificates = JsonConvert.DeserializeObject<List<ItemCertificate>>(item.document);
                        if (certificates != null)
                        {
                            foreach (ItemCertificate m_item in certificates)
                            {
                                tmp.filesCustomer.Add(m_item);
                            }
                        }
                    }
                    else
                    {
                        if (sqlCertificates.Count > 0)
                        {
                            foreach (string m_item in item.certificates)
                            {
                                SqlCertificate? temp = sqlCertificates.Where(s => s.code.CompareTo(m_item) == 0).FirstOrDefault();
                                if (temp == null)
                                {
                                    continue;
                                }

                                ItemCertificate myItem = new ItemCertificate();
                                myItem.code = temp.code;
                                myItem.name = temp.name;

                                tmp.filesCustomer.Add(myItem);
                            }
                        }
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

                if (item.survey != null)
                {
                    ItemUser survey = new ItemUser();
                    survey.user = item.survey.user;
                    survey.displayName = item.survey.displayName;
                    survey.numberPhone = item.survey.phoneNumber;

                    tmp.survey = survey;
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

        public class ItemOrder
        {
            public string code { get; set; } = "";
            public MyItemGroup group { get; set; } = new MyItemGroup();
            public MyItemArea area { get; set; } = new MyItemArea();
            public ItemProfile profile { get; set; } = new ItemProfile();
            public List<ItemCertificate> filesCustomer { get; set; } = new List<ItemCertificate>();
            public ItemUser receiver { get; set; } = new ItemUser();
            public ItemUser manager { get; set; } = new ItemUser();
            public ItemUser survey { get; set; } = new ItemUser();
            public ItemUser worker { get; set; } = new ItemUser();
            public ItemCustomer customer { get; set; } = new ItemCustomer();
            public string note { get; set; } = "";
            public ItemType type { get; set; } = new ItemType();
            public ItemService service { get; set; } = new ItemService();
            public ItemStateOrder state { get; set; } = new ItemStateOrder();
            public ItemAction action { get; set; } = new ItemAction();
            public string createTime { get; set; } = "";
            public string lastestTime { get; set; } = "";

        }

        public List<ItemOrder> getListOrder(string token, DateTime begin, DateTime end)
        {
            DateTime m_begin = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0);
            DateTime m_end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            List<ItemOrder> list = new List<ItemOrder>();

            using (DataContext context = new DataContext())
            {
                List<SqlCertificate> sqlCertificates = context.certificates!.Where(s => s.isdeleted == false).ToList();

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0)
                                                .Include(s => s.role)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.receiver)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.manager)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.type)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.service)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.group)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.area)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.receiver)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.manager)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.customer)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.type)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.service)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.group)
                                                .Include(s => s.surveyOrders!).ThenInclude(s => s.area)
                                                .FirstOrDefault();
                if (m_user == null)
                {
                    return new List<ItemOrder>();
                }

                
                //C)
                List<SqlOrder> mOrders = new List<SqlOrder>();
                if (m_user.role!.code.CompareTo("staff") == 0 || m_user.role.code.CompareTo("survey") == 0)
                {
                    if(m_user.role!.code.CompareTo("staff") == 0)
                    {
                        mOrders = m_user.workerOrders!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.createdTime) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.createdTime) > 0 && s.isDelete == false).OrderByDescending(s => s.createdTime).ToList();
                    }
                    else
                    {
                        mOrders = m_user.surveyOrders!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.createdTime) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.createdTime) > 0 && s.isDelete == false).OrderByDescending(s => s.createdTime).ToList();
                    }

                }
                else
                {
                    List<SqlOrder> orders = context.orders!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.createdTime) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.createdTime) > 0)
                                                       .Include(s => s.customer)
                                                       .Include(s => s.receiver)
                                                       .Include(s => s.manager)
                                                       .Include(s => s.survey)
                                                       .Include(s => s.worker)
                                                       .Include(s => s.service)
                                                       .Include(s => s.type)
                                                       .Include(s => s.state)
                                                       .Include(s => s.group)
                                                       .Include(s => s.area)
                                                       .OrderByDescending(s => s.createdTime)
                                                       .ToList();
                    if (orders.Count < 1)
                    {
                        return new List<ItemOrder>();
                    }

                    if (m_user.role!.code.CompareTo("manager") == 0)
                    {
                        mOrders = orders.Where(s => s.service!.code.CompareTo("LM") == 0 && s.state!.code > 0).ToList();
                    }
                    else
                    {
                        mOrders = orders;
                    }
                }

                foreach (SqlOrder item in mOrders)
                {
                    ItemOrder tmp = new ItemOrder();

                    tmp.code = item.code;
                    tmp.group.code = item.group!.code;
                    tmp.group.name = item.group!.name;
                    tmp.area.code = item.area!.code;
                    tmp.area.name = item.area!.name;
                    tmp.profile.name = item.name;
                    tmp.profile.phone = item.phone;
                    tmp.profile.persons = item.persons;
                    tmp.profile.addressCustomer = item.addressCustomer;
                    tmp.profile.addressWater = item.addressWater;
                    tmp.profile.addressContract = item.addressContract;
                    if (item.customer != null)
                    {
                        if (item.customer.isdeleted == false)
                        {
                            ItemCustomer customer = new ItemCustomer();
                            customer.maDB = item.customer.code;
                            customer.route = string.IsNullOrEmpty(item.customer.route) ? "Coming Soon !!!" : item.customer.route;
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

                    if(item.certificates != null)
                    {
                        if (!string.IsNullOrEmpty(item.document))
                        {
                            List<ItemCertificate>? certificates = JsonConvert.DeserializeObject<List<ItemCertificate>>(item.document);
                            if (certificates != null)
                            {
                                foreach (ItemCertificate m_item in certificates)
                                {
                                    tmp.filesCustomer.Add(m_item);
                                }
                            }
                        }
                        else
                        {
                            if (sqlCertificates.Count > 0)
                            {
                                foreach (string m_item in item.certificates)
                                {
                                    SqlCertificate? temp = sqlCertificates.Where(s => s.code.CompareTo(m_item) == 0).FirstOrDefault();
                                    if (temp == null)
                                    {
                                        continue;
                                    }

                                    ItemCertificate myItem = new ItemCertificate();
                                    myItem.code = temp.code;
                                    myItem.name = temp.name;

                                    tmp.filesCustomer.Add(myItem);
                                }
                            }
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

                    if (item.survey != null)
                    {
                        ItemUser survey = new ItemUser();
                        survey.user = item.survey.user;
                        survey.displayName = item.survey.displayName;
                        survey.numberPhone = item.survey.phoneNumber;

                        tmp.survey = survey;
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

                    tmp.action = getActionUpdate(item.code);
                    //tmp.logActions.Add(m_log);
                    tmp.createTime = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.lastestTime = item.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                    list.Add(tmp);
                }
                return list;
            }

        }
        public class ItemLogOrder
        {
            public ItemInfoOrder order { get; set; } = new ItemInfoOrder();
            public ItemAction action { get; set; } = new ItemAction();
            public string time { get; set; } = "";
        }

        public List<ItemLogOrder> getFindOrder(string code)
        {
            List<ItemLogOrder> list = new List<ItemLogOrder>();
            using (DataContext context = new DataContext())
            {
                List<SqlCertificate> sqlCertificates = context.certificates!.Where(s => s.isdeleted == false).ToList();

                List<SqlOrder>? items = context.orders!.Include(s => s.customer).Where(s => s.isFinish == false && (s.code.CompareTo(code) == 0 || s.phone.CompareTo(code) == 0 || s.addressCustomer.CompareTo(code) == 0) || s.customer!.code.CompareTo(code) == 0)
                                                       .Include(s => s.type)
                                                       .Include(s => s.service)
                                                       .Include(s => s.state)
                                                       .Include(s => s.receiver)
                                                       .Include(s => s.manager)
                                                       .Include(s => s.survey)
                                                       .Include(s => s.worker)
                                                       .Include(s => s.group)
                                                       .Include(s => s.area)
                                                       .ToList();
                if (items.Count < 1)
                {
                    return new List<ItemLogOrder>();
                }
                foreach (SqlOrder item in items)
                {
                    ItemLogOrder m_info = new ItemLogOrder();

                    ItemInfoOrder tmp = new ItemInfoOrder();
                    tmp.code = item.code;
                    tmp.group.code = item.group!.code;
                    tmp.group.name = item.group!.name;
                    tmp.area.code = item.area!.code;
                    tmp.area.name = item.area!.name;
                    tmp.profile.name = item.name;
                    tmp.profile.phone = item.phone;
                    tmp.profile.persons = item.persons;
                    tmp.profile.addressCustomer = item.addressCustomer;
                    tmp.profile.addressWater = item.addressWater;
                    tmp.profile.addressContract = item.addressContract;
                    if (item.customer != null)
                    {
                        ItemCustomer customer = new ItemCustomer();
                        customer.maDB = item.customer.code;
                        customer.route = string.IsNullOrEmpty(item.customer.route) ? "Coming Soon !!!" : item.customer.route;
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

                    if (item.certificates != null)
                    {
                        if (!string.IsNullOrEmpty(item.document))
                        {
                            List<ItemCertificate>? certificates = JsonConvert.DeserializeObject<List<ItemCertificate>>(item.document);
                            if (certificates != null)
                            {
                                foreach (ItemCertificate m_item in certificates)
                                {
                                    tmp.filesCustomer.Add(m_item);
                                }
                            }
                        }
                        else
                        {
                            if (sqlCertificates.Count > 0)
                            {
                                foreach (string m_item in item.certificates)
                                {
                                    SqlCertificate? temp = sqlCertificates.Where(s => s.code.CompareTo(m_item) == 0).FirstOrDefault();
                                    if (temp == null)
                                    {
                                        continue;
                                    }

                                    ItemCertificate myItem = new ItemCertificate();
                                    myItem.code = temp.code;
                                    myItem.name = temp.name;

                                    tmp.filesCustomer.Add(myItem);
                                }
                            }
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

                    if (item.survey != null)
                    {
                        ItemUser survey = new ItemUser();
                        survey.user = item.survey.user;
                        survey.displayName = item.survey.displayName;
                        survey.numberPhone = item.survey.phoneNumber;

                        tmp.survey = survey;
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

                    m_info.action = getActionUpdate(item.code);

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

            if (customers.Count > 0)
            {
                ItemCustomer? tmp = customers.Where(s => s.maDB.CompareTo(code) == 0 || s.sdt.CompareTo(code) == 0 || s.diachi.CompareTo(code) == 0).FirstOrDefault();
                if (tmp == null)
                {
                    return new ItemCustomer();
                }

                info = tmp;
            }

            return info;
        }
    }
}
