using Microsoft.EntityFrameworkCore;
using ServerWater2.Models;
using static ServerWater2.APIs.MyAction;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyOrder;
using static ServerWater2.APIs.MyService;
using static ServerWater2.APIs.MyState;
using static ServerWater2.APIs.MyType;

namespace ServerWater2.APIs
{
    public class MyLogOrder
    {
        public MyLogOrder()
        {
        }

        public class ItemLogOrder
        {
            public ItemUser user { get; set; } = new ItemUser();
            public ItemOrder order { get; set; } = new ItemOrder();
            public ItemAction action { get; set; } = new ItemAction();
            public string time { get; set; } = "";
            public string note { get; set; } = "";
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
        }

        public List<ItemLogOrder> getListLog(string token, DateTime begin, DateTime end)
        {
            DateTime m_begin = new DateTime(begin.Year, begin.Month, begin.Day, 0, 0, 0);
            DateTime m_end = new DateTime(end.Year, end.Month, end.Day, 0, 0, 0);
            m_end = m_end.AddDays(1);
            List<ItemLogOrder> list = new List<ItemLogOrder>();

            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if(m_user == null)
                {
                    return new List<ItemLogOrder>();
                }

                List<SqlLogOrder>? logs = context.logs!.Where(s => DateTime.Compare(m_begin.ToUniversalTime(), s.time) <= 0 && DateTime.Compare(m_end.ToUniversalTime(), s.time) > 0).OrderBy(s => s.time)
                                                       .Include(s => s.action)
                                                       .Include(s => s.user)
                                                       .Include(s => s.order!).ThenInclude(s => s.customer)
                                                       .Include(s => s.order!).ThenInclude(s => s.receiver)
                                                       .Include(s => s.order!).ThenInclude(s => s.manager)
                                                       .Include(s => s.order!).ThenInclude(s => s.worker)
                                                       .Include(s => s.order!).ThenInclude(s => s.type)
                                                       .Include(s => s.order!).ThenInclude(s => s.service)
                                                       .Include(s => s.order!).ThenInclude(s => s.state)
                                                       .ToList();

                if(logs.Count > 0)
                {
                    foreach (SqlLogOrder item in logs)
                    {
                        ItemLogOrder itemLog = new ItemLogOrder();

                        if(item.user != null)
                        {
                            itemLog.user.user = item.user.user;
                            itemLog.user.displayName = item.user.displayName;
                            itemLog.user.numberPhone = item.user.phoneNumber;
                        }

                        if(item.order != null)
                        {
                            ItemOrder tmp = new ItemOrder();
                            tmp.code = item.order.code;
                            tmp.profile.name = item.order.name;
                            tmp.profile.phone = item.order.phone;
                            tmp.profile.addressCustomer = item.order.addressCustomer;
                            tmp.profile.addressWater = item.order.addressWater;
                            tmp.profile.addressContract = item.order.addressContract;
                            if (item.order.customer != null)
                            {
                                if (item.order.customer.isdeleted == false)
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

                                    tmp.customer = customer;
                                }

                            }

                            if (item.order.receiver != null)
                            {
                                ItemUser receiver = new ItemUser();
                                receiver.user = item.order.receiver.user;
                                receiver.displayName = item.order.receiver.displayName;
                                receiver.numberPhone = item.order.receiver.phoneNumber;

                                tmp.receiver = receiver;
                            }

                            if (item.order.manager != null)
                            {
                                ItemUser manager = new ItemUser();
                                manager.user = item.order.manager.user;
                                manager.displayName = item.order.manager.displayName;
                                manager.numberPhone = item.order.manager.phoneNumber;

                                tmp.manager = manager;
                            }

                            if (item.order.worker != null)
                            {
                                ItemUser worker = new ItemUser();
                                worker.user = item.order.worker.user;
                                worker.displayName = item.order.worker.displayName;
                                worker.numberPhone = item.order.worker.phoneNumber;

                                tmp.worker = worker;
                            }

                            tmp.note = item.note;
                            if (item.order.type != null)
                            {
                                ItemType type = new ItemType();

                                type.code = item.order.type.code;
                                type.name = item.order.type.name;
                                type.des = item.order.type.des;
                                tmp.type = type;
                            }

                            if (item.order.service != null)
                            {
                                ItemService service = new ItemService();
                                service.code = item.order.service.code;
                                service.name = item.order.service.name;
                                service.des = item.order.service.des;
                                tmp.service = service;
                            }
                            if (item.order.state != null)
                            {
                                ItemStateOrder state = new ItemStateOrder();
                                state.code = item.order.state.code;
                                state.name = item.order.state.name;
                                state.des = item.order.state.des;
                                tmp.state = state;
                            }
                            tmp.createTime = item.order.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                            tmp.lastestTime = item.order.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                            itemLog.order = tmp;
                        }

                        if(item.action != null)
                        {
                            ItemAction action = new ItemAction();
                            action.code = item.action.code;
                            action.name = item.action.name;
                            action.des = item.action.des;
                            
                            itemLog.action = action;
                        }

                        itemLog.time = item.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                        itemLog.note = item.note;
                        itemLog.latitude = item.latitude;
                        itemLog.longitude = item.longitude;

                        list.Add(itemLog);
                    }
                }
                return list;
            }
        }
    }
}
