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
                SqlOrder? order = context.orders!.Where(s => s.isDelete == false &&  s.isFinish == false && s.code.CompareTo(code) == 0)
                                                    .Include(s => s.state)
                                                    .FirstOrDefault();
                if (order == null)
                {
                    return false;
                }

                if(state == 7)
                {
                    order.isDelete = true;
                }
                
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

        public async Task<bool> setAction(string token, string order, string action)
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
                m_log.time = DateTime.Now.ToUniversalTime();
                int rows = await context.SaveChangesAsync();

                return true;
            }
        }


        public async Task<string> createUpdateOrderAsync(string code, string name, string customer, string phone, string addressCustomer, string addressOrder, string addressContract, string service, string type, string note)
        {
            using (DataContext context = new DataContext())
            {
                SqlOrder? m_order = null;

                if (string.IsNullOrEmpty(code))
                {
                    m_order = context.orders!.Where(s => s.name.CompareTo(name) == 0 && s.phone.CompareTo(phone) == 0 && s.addressCustomer.CompareTo(addressCustomer) == 0 && s.isDelete == false).Include(s => s.customer).Include(s => s.type).Include(s => s.state).Include(s => s.service).FirstOrDefault();
                    if (m_order == null)
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

                        SqlLogOrder log = new SqlLogOrder();
                        log.ID = DateTime.Now.Ticks;
                        log.order = m_order;
                        log.time = DateTime.Now.ToUniversalTime();
                        log.note = "New Order : " + m_order.service!.name;
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

                    SqlLogOrder log = new SqlLogOrder();
                    log.ID = DateTime.Now.Ticks;
                    log.order = m_order;
                    log.time = DateTime.Now.ToUniversalTime();
                    log.note = "New Order : " + m_order.service!.name;
                    context.logs!.Add(log);
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

                bool flag = await setStateOrder(m_user.ID, order.code, 1, "Checked Order !!!", "", "");
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

                bool flag = await setStateOrder(user.ID, m_order.code, m_order.state!.code, "Set Customer Order !!!", m_customer.latitude, m_customer.longitude);
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
                order.manager = m_user;

                string note = string.Format("Received Order {0} From Manager : {1} ", order.code, m_user.user);
                bool flag = await setStateOrder(m_user.ID, order.code, 2, note, "", "");
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
                    if(order.state!.code > 2)
                    {
                        return false;
                    }

                    order.worker = worker;
                    string note = "";
                    if(order.manager == null)
                    {
                        note = string.Format("Received Order {0} From Manager : {1} ", order.code, m_user.user);
                        bool flag1 =  await setStateOrder(m_user.ID, order.code, 3, note, "", "");

                    } 

                    note = string.Format("Assigned Order {0} For Staff : {1} From Manager : {2} ", order.code, worker.user, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, order.code, 3, note, "", "");
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
                    if (order.state!.code > 2)
                    {
                        return false;
                    }
                    order.worker = worker;

                    string note = string.Format("Assigned Order {0} For Staff : {1} From Manager : {2} ", order.code, worker.user, m_user.user);
                    bool flag = await setStateOrder(m_user.ID, order.code, 3, note, "", "");
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
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "");
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
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "");
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
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 4, note, "", "");
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
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "");
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
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "");
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
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 5, note, "", "");
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
                    if(m_order.customer == null)
                    {
                        return false;
                    }

                    string note = string.Format("Finished Order {0} ", m_order.code);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "");
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
                        if (m_order.customer == null)
                        {
                            return false;
                        }

                        string note = string.Format("Finished Order {0} ", m_order.code);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "");
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
                        if (m_order.customer == null)
                        {
                            return false;
                        }

                        string note = string.Format("Finished Order {0} ", m_order.code);
                        bool flag = await setStateOrder(m_user.ID, m_order.code, 6, note, "", "");
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
                    string note = string.Format("Cancelled Order {0} ", m_order.code);
                    bool flag = await setStateOrder(m_user.ID, m_order.code, 7, note, "", "");
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

        public class ItemOrderRequest
        {
            public ItemUser user { get; set; } = new ItemUser();
            public ItemAction action { get; set; } = new ItemAction();
            public string latitude { get; set; } = "";
            public string longitude { get; set; } = "";
            public string note { get; set; } = "";
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
            public ItemOrderRequest logAction { get; set; } = new ItemOrderRequest();
            public DateTime createTime { get; set; } 
            public DateTime lastestTime { get; set; }

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
                if(orders.Count < 1)
                {
                    return new List<ItemInfoOrder>();
                }    

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

                    tmp.logAction = getLogOrder(item.ID.ToString());

                    tmp.createTime = item.createdTime;
                    tmp.lastestTime = item.lastestTime;

                    list.Add(tmp);
                }
            }
            return list;
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

        public ItemLogOrder getFindOrder(string code)
        {
            using (DataContext context = new DataContext())
            {
                ItemLogOrder m_info = new ItemLogOrder();

                SqlOrder? item = context.orders!.Where(s => s.isDelete == false && s.isFinish == false && (s.code.CompareTo(code) == 0 || s.phone.CompareTo(code) == 0 || s.addressWater.CompareTo(code) == 0))
                                                        .Include(s => s.type)
                                                        .Include(s => s.service)
                                                        .Include(s => s.state)
                                                        .Include(s => s.customer)
                                                        .Include(s => s.receiver)
                                                        .Include(s => s.manager)
                                                        .Include(s => s.worker)
                                                        .FirstOrDefault();
                if (item == null)
                {
                    return new ItemLogOrder();
                }


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

                return m_info;
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
