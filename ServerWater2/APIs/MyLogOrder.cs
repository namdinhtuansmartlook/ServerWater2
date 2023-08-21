using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServerWater2.Models;
using static ServerWater2.APIs.MyAction;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyOrder;
using static ServerWater2.APIs.MyService;
using static ServerWater2.APIs.MyState;
using static ServerWater2.APIs.MyType;
using static ServerWater2.Controllers.OrderController;

namespace ServerWater2.APIs
{
    public class MyLogOrder
    {
        public MyLogOrder()
        {
        }

        public class ItemLogOrder
        {
            public long id { get; set; } = -1;
            public ItemOrder order { get; set; } = new ItemOrder();

        }

        public class ItemLog
        {
            public long id { get; set; } = -1;
            public ItemUser user { get; set; } = new ItemUser();
            public ItemLogOrder order { get; set; } = new ItemLogOrder();
            public ItemAction action { get; set; } = new ItemAction();
            public DateTime time { get; set; }
            public string note { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
        }

        public List<ItemLog> getListLog()
        {
            List<ItemLog> list = new List<ItemLog>();

            using (DataContext context = new DataContext())
            {
                List<SqlLogOrder>? logs = context.logs!.Include(s => s.action)
                                                       .Include(s => s.user)
                                                       .Include(s => s.order!).ThenInclude(s => s.customer)
                                                       .Include(s => s.order!).ThenInclude(s => s.receiver)
                                                       .Include(s => s.order!).ThenInclude(s => s.manager)
                                                       .Include(s => s.order!).ThenInclude(s => s.worker)
                                                       .Include(s => s.order!).ThenInclude(s => s.type)
                                                       .Include(s => s.order!).ThenInclude(s => s.service)
                                                       .Include(s => s.order!).ThenInclude(s => s.state)
                                                       .Include(s => s.order).ThenInclude(s => s.group)
                                                       .Include(s => s.order).ThenInclude(s => s.area)
                                                       .OrderByDescending(s => s.time)
                                                       .ToList();

                if (logs.Count > 0)
                {
                    foreach (SqlLogOrder item in logs)
                    {
                        ItemLog itemLog = new ItemLog();

                        itemLog.id = item.ID;
                        if (item.user != null)
                        {
                            itemLog.user.user = item.user.user;
                            itemLog.user.displayName = item.user.displayName;
                            itemLog.user.numberPhone = item.user.phoneNumber;
                        }

                        if (item.order != null)
                        {
                            ItemLogOrder itemOrder = new ItemLogOrder();
                            itemOrder.id = item.order.ID;

                            ItemOrder m_order = new ItemOrder();
                            m_order.code = item.order.code;
                            m_order.profile.name = item.order.name;
                            m_order.profile.phone = item.order.phone;
                            m_order.profile.addressCustomer = item.order.addressCustomer;
                            m_order.profile.addressWater = item.order.addressWater;
                            m_order.profile.addressContract = item.order.addressContract;

                            if (item.order.customer != null)
                            {
                                ItemCustomer customer = new ItemCustomer();
                                customer.maDB = item.order.customer.code;
                                customer.sdt = item.order.customer.phone;
                                customer.tenkh = item.order.customer.name;
                                customer.diachi = item.order.customer.address;
                                customer.note = item.order.customer.note;
                                customer.x = item.order.customer.latitude;
                                customer.y = item.order.customer.longitude;
                                if (item.order.customer.images != null)
                                {
                                    customer.images = item.order.customer.images;
                                }

                                m_order.customer = customer;

                            }
                            if (item.order.receiver != null)
                            {
                                ItemUser receiver = new ItemUser();
                                receiver.user = item.order.receiver.user;
                                receiver.displayName = item.order.receiver.displayName;
                                receiver.numberPhone = item.order.receiver.phoneNumber;

                                m_order.receiver = receiver;
                            }
                            if (item.order.manager != null)
                            {
                                ItemUser manager = new ItemUser();
                                manager.user = item.order.manager.user;
                                manager.displayName = item.order.manager.displayName;
                                manager.numberPhone = item.order.manager.phoneNumber;

                                m_order.manager = manager;
                            }
                            if (item.order.worker != null)
                            {
                                ItemUser worker = new ItemUser();
                                worker.user = item.order.worker.user;
                                worker.displayName = item.order.worker.displayName;
                                worker.numberPhone = item.order.worker.phoneNumber;

                                m_order.worker = worker;
                            }
                            if (item.order.type != null)
                            {
                                ItemType type = new ItemType();

                                type.code = item.order.type.code;
                                type.name = item.order.type.name;
                                type.des = item.order.type.des;
                                m_order.type = type;
                            }
                            if (item.order.service != null)
                            {
                                ItemService service = new ItemService();
                                service.code = item.order.service.code;
                                service.name = item.order.service.name;
                                service.des = item.order.service.des;
                                m_order.service = service;
                            }
                            if (item.order.state != null)
                            {
                                ItemStateOrder state = new ItemStateOrder();
                                state.code = item.order.state.code;
                                state.name = item.order.state.name;
                                state.des = item.order.state.des;
                                m_order.state = state;
                            }

                            m_order.note = item.note;
                            m_order.createTime = item.order.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                            m_order.lastestTime = item.order.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                            itemOrder.order = m_order;

                            itemLog.order = itemOrder;

                        }

                        if (item.action != null)
                        {
                            ItemAction action = new ItemAction();
                            action.code = item.action.code;
                            action.name = item.action.name;
                            action.des = item.action.des;

                            itemLog.action = action;
                        }

                        itemLog.time = item.time;
                        itemLog.note = item.note;
                        if(item.images == null)
                        {
                            item.images = new List<string>();
                        }
                        itemLog.images = item.images;
                        itemLog.latitude = item.latitude;
                        itemLog.longitude = item.longitude;

                        list.Add(itemLog);
                    }
                }
                return list;
            }
        }

        public List<ItemLog> getDataRow(DateTime begin, DateTime end)
        {
            DateTime m_begin = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0);
            DateTime m_end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
            //end = m_end.AddDays(1);
            List<ItemLog> datas = getListLog();

            List<ItemLog> m_datas = datas.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.time) <= 0 && DateTime.Compare(end.ToUniversalTime(), s.time) > 0).ToList();
            if(m_datas.Count < 1)
            {
                return new List<ItemLog>();
            }
            return m_datas;
        }
        public class ItemHistoryForUser
        {
            public ItemAction action { get; set; } = new ItemAction();
            public string time { get; set; } = "";
            public string note { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";

        }
        public class ItemHistory
        {
            public string order { get; set; } = "";
            public string state { get; set; } = "";
            public List<ItemHistoryForUser> datas { get; set; } = new List<ItemHistoryForUser>();

        }

        public string getListHistoryOrderForUser(string token, DateTime begin, DateTime end, string code)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return "";
                }

                List<ItemLog> datas = getDataRow(begin, end).Where(s => s.order.order.code.CompareTo(code) == 0).ToList();

                if (datas.Count < 1)
                {
                    return "";
                }
                List<ItemHistoryForUser> m_datas = new List<ItemHistoryForUser>();

                foreach (ItemLog item in datas)
                {
                    ItemHistoryForUser tmp = new ItemHistoryForUser();

                    tmp.action.code = item.action.code;
                    tmp.action.name = item.action.name;
                    tmp.action.des = item.action.code;

                    tmp.time = item.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.note = item.note;
                    if(item.images == null)
                    {
                        item.images = new List<string>();
                    }
                    tmp.images = item.images;
                    tmp.latitude = item.latitude;
                    tmp.longitude = item.longitude;

                    m_datas.Add(tmp);
                }

                string result = "";
                if (m_datas.Count > 0)
                {
                    result = JsonConvert.SerializeObject(m_datas);
                }
                return result;
            }

        }
       /* public string getListLogOrder(string token, DateTime begin, DateTime end, string user)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(user))
            {
                return "";
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return "";
                }

                List<ItemLog> datas = getDataRow(begin, end).Where(s => s.user.user.CompareTo(user) == 0).ToList();

                if (datas.Count < 1)
                {
                    return "";
                }
                List<ItemHistoryForUser> m_datas = new List<ItemHistoryForUser>();

                foreach (ItemLog item in datas)
                {
                    ItemHistoryForUser tmp = new ItemHistoryForUser();

                    tmp.action.code = item.action.code;
                    tmp.action.name = item.action.name;
                    tmp.action.des = item.action.code;
                    tmp.order = item.order.order.code;
                    tmp.state = item.order.order.state.code.ToString();

                    tmp.time = item.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.note = item.note;
                    tmp.latitude = item.latitude;
                    tmp.longitude = item.longitude;

                    m_datas.Add(tmp);
                }

                string result = "";
                if (m_datas.Count > 0)
                {
                    result = JsonConvert.SerializeObject(m_datas);
                }
                return result;
            }

        }*/
        public string getListLogOrderForAdmin(string token, DateTime begin, DateTime end, string user)
        {
            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(user))
            {
                return "";
            }
            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if(m_user == null)
                {
                    return "";
                }

                SqlUser? tmp_user = context.users!.Where(s => s.isdeleted == false && s.user.CompareTo(user) == 0)
                                                  .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                  .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                  .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                  .Include(s => s.role)
                                                  .FirstOrDefault();
                if(tmp_user == null)
                {
                    return "";
                }
                List<SqlOrder> orders = new List<SqlOrder>();

                List<ItemLog> datas = getDataRow(begin, end);
                if (tmp_user.role!.code.CompareTo("receiver") == 0 || tmp_user.role!.code.CompareTo("admin") == 0)
                {
                    orders = tmp_user.receiverOrders!.ToList();
                }
                else if(tmp_user.role!.code.CompareTo("manager") == 0)
                {
                    orders = tmp_user.managerOrders!.ToList();

                }
                else if (tmp_user.role!.code.CompareTo("staff") == 0)
                {
                    orders = tmp_user.workerOrders!.ToList();
                }

                if (datas.Count < 1)
                {
                    return "";
                }
                List<ItemHistory> m_datas = new List<ItemHistory>();
                foreach (SqlOrder order in orders)
                {
                    ItemHistory tmp = new ItemHistory();
                    tmp.order = order.code;
                    tmp.state = order.state!.code.ToString();
                    List<ItemHistoryForUser>? tmps = JsonConvert.DeserializeObject<List<ItemHistoryForUser>>(getListHistoryOrderForUser(token, begin, end, order.code));
                    if (tmps != null)
                    {
                        tmp.datas = tmps;
                    }
                    m_datas.Add(tmp);

                }
         
                string result = "";
                if (m_datas.Count > 0)
                {
                    result = JsonConvert.SerializeObject(m_datas);
                }
                return result;
            }
            
        }
        
    }
}
