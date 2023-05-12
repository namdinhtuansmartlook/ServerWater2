using ServerWater2.Models;
using Newtonsoft.Json;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyState;
using Microsoft.EntityFrameworkCore;
using static ServerWater2.APIs.MyType;
using static ServerWater2.APIs.MyService;
using System.Numerics;

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
        public async Task<bool> setStateOrder(long idUser, string code, int state, string note, string latitude, string longitude, string action)
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
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false &&  s.isFinish == false && s.code.CompareTo(code) == 0)
                                                    .Include(s => s.state)
                                                    .FirstOrDefault();
                if (order == null)
                {
                    return false;
                }
                SqlAction? m_action = context.actions!.Where(s => s.isdeleted == false && s.code.CompareTo(action) == 0).FirstOrDefault();
                if(m_action == null)
                {
                    return false;
                }
                /*if(state == 0)
                {
                    order.receiver = user;
                    state += 1;
                }
                if(state == 1)
                {
                    state += 1;
                }
                else if (state == 2)
                {
                    state += 1;
                }
                else if (state == 6)
                {
                    order.isFinish = true;
                }*/
               
                order.state = context.states!.Where(s => s.isdeleted == false && s.code == state).FirstOrDefault();
                order.lastestTime = DateTime.Now.ToUniversalTime();

                SqlLogOrder log = new SqlLogOrder();
                log.ID = DateTime.Now.Ticks;
                log.order = order;
                log.note = note;
                log.time = order.lastestTime;
                log.user = user;
                log.latitude = latitude;
                log.longitude = longitude;
                log.action = m_action;
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

        public async Task<string> createUpdateOrderAsync(string code, string name, string customer, string phone, string addressCustomer, string addressOrder, string addressContract, string service, string type, string note)
        {
            using (DataContext context = new DataContext())
            {
                

                SqlOrder? m_order = context.orders!.Where(s => s.name.CompareTo(name) == 0 && s.phone.CompareTo(phone) == 0 && s.addressCustomer.CompareTo(addressCustomer) == 0 && s.isDelete == false).Include(s => s.type).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                if (m_order == null)
                {
                    m_order = new SqlOrder();
                    m_order.ID = DateTime.Now.Ticks;
                    m_order.code = generatorcode();
                    m_order.name = name;
                    m_order.phone = phone;
                    
                    SqlCustomer? m_customer = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.name.CompareTo(customer) == 0 && s.isdeleted == false).FirstOrDefault();
                    if (m_customer != null)
                    {
                        m_order.customer = m_customer;

                    }
                   
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

                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_order;
                    log.time = DateTime.Now.ToUniversalTime();
                    log.note = "New Order : " + m_order.type!.name;
                    context.logs!.Add(log);

                }
                else
                {
                    if (m_order.state!.code < 3)
                    {
                        m_order.addressWater = addressOrder;
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
                        return "";
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

        public async Task<string> createRequestOrder(string code, string name, string phone, string customer, string addressCustomer, string addressContract, string type, string note)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(addressCustomer) || string.IsNullOrEmpty(type))
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

                string order = await createUpdateOrderAsync(code, name, customer, phone, addressCustomer, "", addressContract, "", type, note);
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
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                if(order == null)
                {
                    return false;
                }
                if(order.state!.code != 0)
                {
                    return false;
                }
                order.receiver = m_user;

                bool flag = await setStateOrder(m_user.ID, order.code, 1, "Checked Order !!!", "", "", "action1");
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

                SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                if (m_order == null)
                {
                    return false;
                }
                if(m_order.state!.code < 1)
                {
                    return false;
                }

                m_order.customer = m_customer;

                bool flag = await setStateOrder(user.ID, m_order.code, 2, "Set Customer Order !!!", m_customer.latitude, m_customer.longitude, "action1");
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
                if(order.state!.code > 2)
                {
                    return false;
                }
                if (order.manager != null)
                {
                    return false;
                }
                order.manager = m_user;

                string note = string.Format("Received Order {0} From Manager : {1} ", order.code, m_user.user);
                bool flag = await setStateOrder(m_user.ID, order.code, 2, note, "", "", "action1");
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

                if (m_user.role!.code.CompareTo("admin") == 0 || m_user.role!.code.CompareTo("receiver") == 0)
                {
                    SqlOrder? order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if(order.customer == null)
                    {
                        return false;
                    }
                    if(order.state!.code > 3)
                    {
                        return false;
                    }
                    order.worker = worker;
                    string note = "";
                    if(order.manager == null)
                    {
                        note = string.Format("Received Order {0} From Manager : {1} ", order.code, m_user.user);
                        bool flag1 =  await setStateOrder(m_user.ID, order.code, 3, note, "", "", "action1");

                    } 

                    note = string.Format("Assigned Order {0} For Staff : {1} From Manager : {2} ", order.code, worker.user, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, order.code, 3, note, "", "", "action2");
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
                else
                {
                   
                    SqlOrder? order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    if (order.state!.code > 3)
                    {
                        return false;
                    }
                    order.worker = worker;

                    string note = string.Format("Assigned Order {0} For Staff : {1} From Manager : {2} ", order.code, worker.user, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, order.code, 3, note, "", "", "action2");
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
        }
        public async Task<bool> beginWorkOrder(string token, string code)
        {
            using(DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role)
                                                .Include(s => s.receiverOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.managerOrders!).ThenInclude(s => s.state)
                                                .Include(s => s.workerOrders!).ThenInclude(s => s.state)
                                                .FirstOrDefault();
                if(m_user == null)
                {
                    return false;
                }

                if(m_user.role!.code.CompareTo("admin") == 0 || m_user.role!.code.CompareTo("receiver") == 0)
                {
                    SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).Include(s => s.state).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if(m_order.state!.code != 3)
                    {
                        return false;
                    }

                    string note = string.Format("Starting work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "", "action3");
                    return flag;
                }
                else
                {
                    if(m_user.role.code.CompareTo("manager") == 0 || m_user.role.code.CompareTo("survey") == 0)
                    {
                        SqlOrder? m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 3)
                        {
                            return false;
                        }

                        string note = string.Format("Starting work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "", "action3");
                        return flag;
                    }    
                    else
                    {
                        SqlOrder? m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 3)
                        {
                            return false;
                        }

                        string note = string.Format("Starting work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "", "action3");
                        return flag;

                    }    
                }    
            }
        }

        public async Task<bool> finishWorkOrder(string token, string code)
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

                if (m_user.role!.code.CompareTo("admin") == 0 || m_user.role!.code.CompareTo("receiver") == 0)
                {
                    SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 4)
                    {
                        return false;
                    }

                    string note = string.Format("Finishing work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "", "action4");
                    return flag;
                }
                else
                {
                    if (m_user.role.code.CompareTo("manager") == 0 || m_user.role.code.CompareTo("survey") == 0)
                    {
                        SqlOrder? m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 4)
                        {
                            return false;
                        }

                        string note = string.Format("Starting work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "", "action4");
                        return flag;
                    }
                    else
                    {
                        SqlOrder? m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 4)
                        {
                            return false;
                        }

                        string note = string.Format("Starting work to Order {0} From Worker : {1} ", m_order.code, m_user.user);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "", "action4");
                        return flag;

                    }
                }
            }
        }

        public async Task<bool> finishOrder(string token, string code)
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

                if (m_user.role!.code.CompareTo("admin") == 0 || m_user.role!.code.CompareTo("receiver") == 0)
                {
                    SqlOrder? m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (m_order == null)
                    {
                        return false;
                    }
                    if (m_order.state!.code != 5)
                    {
                        return false;
                    }

                    string note = string.Format("Finished Order {0} ", m_order.code);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "", "action5");
                    return flag;
                }
                else
                {
                    if (m_user.role.code.CompareTo("manager") == 0|| m_user.role.code.CompareTo("survey") == 0)
                    {
                        SqlOrder? m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 5)
                        {
                            return false;
                        }

                        string note = string.Format("Finished Order {0} ", m_order.code);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "", "action5");
                        return flag;
                    }
                    else
                    {
                        SqlOrder? m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code != 5)
                        {
                            return false;
                        }

                        string note = string.Format("Finished Order {0} ", m_order.code);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "", "action5");
                        return flag;

                    }
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

                SqlOrder? m_order = null;
                if (m_user.role!.code.CompareTo("staff") != 0 )
                {
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
                        m_order = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                        if (m_order == null)
                        {
                            return false;
                        }
                        if (m_order.state!.code > 3)
                        {
                            return false;
                        }
                    }
                    string note = string.Format("Cancelled Order {0} ", m_order.code);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 7, note, "", "", "action5");
                    return flag;

                }
                else
                {
                    return false;
                }
                
                /* else
                 {
                     if (m_user.role.code.CompareTo("manager") == 0 || m_user.role.code.CompareTo("survey") == 0)
                     {
                         SqlOrder? m_order = m_user.managerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                         if (m_order == null)
                         {
                             return false;
                         }
                         if (m_order.state!.code != 6)
                         {
                             return false;
                         }

                         string note = string.Format("Cancelled Order {0} ", m_order.code);
                         bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "", "action5");
                         return flag;
                     }
                     else
                     {
                         SqlOrder? m_order = m_user.workerOrders!.Where(s => s.isDelete == false && s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                         if (m_order == null)
                         {
                             return false;
                         }
                         if (m_order.state!.code != 6)
                         {
                             return false;
                         }

                         string note = string.Format("Cancelled Order {0} ", m_order.code);
                         bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "", "action5");
                         return flag;

                     }
                 }*/
            }
        }

        public class ItemUser
        {
            public string user { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }
        
        public class ItemProfile
        {
            public string name { get; set; } = "";
            public string phone { get; set; } = "";
            public string addressCustomer { get; set; } = "";
            public string addressWater { get; set; } = "";
            public string addressContract { get; set; } = "";
        }

        public class ItemOrder
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
            public string createTime { get; set; } = "";
            public string lastestTime { get; set; } = "";

        }


        public List<ItemOrder> getListOrder(string token)
        {
            List<ItemOrder> list = new List<ItemOrder>();
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
                    return new List<ItemOrder>();
                }


                List<SqlOrder> m_order = new List<SqlOrder>();
                if (m_user.role!.code.CompareTo("manager") == 0)
                {
                    m_order = context.orders!.Include(s => s.service).Include(s => s.state).Where(s => s.isDelete == false && s.service!.code.CompareTo("LM") == 0 && s.state!.code != 0).Include(s => s.customer).Include(s => s.type).Include(s => s.receiver).Include(s => s.manager).Include(s => s.worker).OrderByDescending(s => s.createdTime).ToList();
                    
                }
                else if (m_user.role!.code.CompareTo("survey") == 0)
                {
                    m_order = context.orders!.Include(s => s.service).Include(s => s.state).Where(s => s.isDelete == false && s.state!.code != 0).Include(s => s.customer).Include(s => s.type).Include(s => s.receiver).Include(s => s.manager).Include(s => s.worker).OrderByDescending(s => s.createdTime).ToList();

                }
                else if (m_user.role!.code.CompareTo("staff") == 0)
                {
                    m_order = m_user.workerOrders!.Where(s => s.isDelete == false ).OrderByDescending(s => s.createdTime).ToList();

                }
                else
                {
                    m_order = context.orders!.Include(s => s.service).Include(s => s.state).Where(s => s.isDelete == false ).Include(s => s.customer).Include(s => s.type).Include(s => s.receiver).Include(s => s.manager).Include(s => s.worker).OrderByDescending(s => s.createdTime).ToList();
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

                foreach (SqlOrder item in m_order)
                {
                    ItemOrder tmp = new ItemOrder();
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
                            if(item.customer.images != null)
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
                    tmp.createTime = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                    tmp.lastestTime = item.lastestTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                    list.Add(tmp);
                }
            }
            return list;
        }

      
        
        public List<ItemOrder> getFindOrder(string code)
        {
            List<ItemOrder> list = new List<ItemOrder>();
            using (DataContext context = new DataContext())
            {
                
                List<SqlOrder>? orders = context.orders!.Where(s => s.isDelete == false && (s.code.CompareTo(code) == 0 || s.phone.CompareTo(code) == 0 || s.addressWater.CompareTo(code) == 0))
                                                        .Include(s => s.customer).Include(s => s.type).Include(s => s.service).Include(s => s.state).Include(s => s.receiver).Include(s => s.manager).Include(s => s.worker).ToList();
                if (orders == null)
                {
                    return new List<ItemOrder>();
                }

                foreach (SqlOrder item in orders)
                {
                    ItemOrder tmp = new ItemOrder();
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
