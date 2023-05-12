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
            public long id { get; set; } = -1;
            public ItemOrder order { get; set; } = new ItemOrder();

        }

        public class ItemLog
        {
            public string id { get; set; } = "";
            public ItemUser user { get; set; } = new ItemUser();
            public ItemLogOrder order { get; set; } = new ItemLogOrder();
            public ItemAction action { get; set; } = new ItemAction();
            public string time { get; set; } = "";
            public string note { get; set; } = "";
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
                                                       .OrderByDescending(s => s.time)
                                                       .ToList();

                if (logs.Count > 0)
                {
                    foreach (SqlLogOrder item in logs)
                    {
                        ItemLog itemLog = new ItemLog();

                        itemLog.id = item.ID.ToString();
                        if (item.user != null)
                        {
                            itemLog.user.user = item.user.user;
                            itemLog.user.displayName = item.user.displayName;
                            itemLog.user.numberPhone = item.user.phoneNumber;
                        }

                        if (item.order != null)
                        {
                            ItemLogOrder tmp = new ItemLogOrder();
                            tmp.id = item.order.ID;

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

                            tmp.order = m_order;

                            itemLog.order = tmp;
                        }

                        if (item.action != null)
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
