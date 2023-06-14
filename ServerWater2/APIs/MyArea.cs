using Azure.Identity;
using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;
using static ServerWater2.APIs.MySchedule;

namespace ServerWater2.APIs
{
    public class MyArea
    {
        public MyArea() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo("kv1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (area == null)
                {
                    SqlArea item = new SqlArea();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "kv1";
                    item.nameArea = "khu vực 1";
                    item.des = "khu vực 1";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.areas!.Add(item);
                }

                area = context.areas!.Where(s => s.code.CompareTo("kv2") == 0 && s.isdeleted == false).FirstOrDefault();
                if (area == null)
                {
                    SqlArea item = new SqlArea();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "kv2";
                    item.nameArea = "khu vực 2";
                    item.des = "khu vực 2";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.areas!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> createAreaAsync(string code, string name, string des)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0 || s.nameArea.CompareTo(name) == 0)).ToList();
                if (areas.Count > 0)
                {
                    return false;
                }
                SqlArea area = new SqlArea();
                area.ID = DateTime.Now.Ticks;
                area.code = code;
                area.nameArea = name;
                area.des = des;
                area.isdeleted = false;
                area.createdTime = DateTime.Now.ToUniversalTime();
                area.lastestTime = DateTime.Now.ToUniversalTime();
                context.areas!.Add(area);
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

        public async Task<bool> editAreaAsync(string code, string des, string name)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).ToList();
                if (areas.Count <= 0)
                {
                    return false;
                }
                foreach (SqlArea area in areas)
                {
                    area.nameArea = name;
                    area.des = des;
                    area.lastestTime = DateTime.Now.ToUniversalTime();
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

        public async Task<bool> deleteAreaAsync(string code)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0)
                                                    .Include(s => s.points)
                                                    .ToList();
                if (areas.Count <= 0)
                {
                    return false;
                }
                foreach (SqlArea area in areas)
                {
                    area.isdeleted = true;
                    area.points = null;
                    area.users = null;
                    area.lastestTime = DateTime.Now.ToUniversalTime();
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



        //=====================Area

        public class ItemCustomerArea
        {
            public string maDB { get; set; } = "";
            public string route { get; set; } = "";
            public string sdt { get; set; } = "";
            public string tenkh { get; set; } = "";
            public string diachi { get; set; } = "";
        }

        public class ItemUser
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
            public string avatar { get; set; } = "";
            public string des { get; set; } = "";
            public string role { get; set; } = "";
        }

        public List<ItemUser> getListUserArea(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new List<ItemUser>();
            }
            using (DataContext context = new DataContext())
            {
                SqlArea? m_area = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).Include(s => s.users).FirstOrDefault();
                if (m_area == null)
                {
                    return new List<ItemUser>();
                }

                List<ItemUser> item = new List<ItemUser>();
                if (m_area.users!.Count > 0)
                {
                    foreach (SqlUser user in m_area.users)
                    {
                        if (user.isdeleted == false)
                        {
                            ItemUser tmp = new ItemUser();
                            tmp.user = user.user;
                            tmp.username = user.username;
                            tmp.displayName = user.displayName;
                            tmp.numberPhone = user.phoneNumber;
                            tmp.avatar = user.avatar;
                            tmp.des = user.des;
                            tmp.role = user.role != null ? user.role!.name : "";


                            item.Add(tmp);
                        }
                    }
                }

                return item;
            }
        }

        public List<ItemCustomerArea> getListCustomerArea(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new List<ItemCustomerArea>();
            }
            using (DataContext context = new DataContext())
            {
                SqlArea? m_area = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).Include(s => s.customers).FirstOrDefault();
                if (m_area == null)
                {
                    return new List<ItemCustomerArea>();
                }

                List<ItemCustomerArea> item = new List<ItemCustomerArea>();
                if (m_area.customers!.Count > 0)
                {
                    foreach (SqlCustomer customer in m_area.customers)
                    {
                        if (customer.isdeleted == false)
                        {
                            ItemCustomerArea tmp = new ItemCustomerArea();
                            tmp.maDB = customer.code;
                            tmp.route = (string.IsNullOrEmpty(customer.route))?"Coming Soon!!!":customer.route;
                            tmp.tenkh = customer.name;
                            tmp.diachi = customer.address;
                            tmp.sdt = customer.phone;

                            item.Add(tmp);
                        }
                    }
                }

                return item;
            }
        }
        public class ItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            //public List<ItemPoint> points { get; set; } = new List<ItemPoint>();
            //public List<ItemUser> users { get; set; } = new List<ItemUser>();
        }



        public List<ItemArea> getAllListArea()
        {

            using (DataContext context = new DataContext())
            {

                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false).ToList();
                List<ItemArea> items = new List<ItemArea>();
                foreach (SqlArea area in areas)
                {
                    ItemArea itemArea = new ItemArea();
                    itemArea.code = area.code;
                    itemArea.name = area.nameArea;
                    itemArea.des = area.des;

                    items.Add(itemArea);

                }
                return items;
            }
        }

        public class ItemFullInfoArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";

            public List<string> layers { get; set; } = new List<string>();
            public List<ItemInfoPoint> points { get; set; } = new List<ItemInfoPoint>();
            public List<ItemCustomerArea> customers { get; set; } = new List<ItemCustomerArea>();
            public List<ItemInfoDeviceForDraw> devices { get; set; } = new List<ItemInfoDeviceForDraw>();
            public List<ItemListScheduleArea> schedulesInDay { get; set; } = new List<ItemListScheduleArea>();


        }

        public class ItemInfoPoint
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
            public List<string> layers { get; set; } = new List<string>();
            public List<ItemInfoDevice> devices { get; set; } = new List<ItemInfoDevice>();
        }

        public class ItemInfoStatus
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public bool isOnline { get; set; }
        }

        public class ItemInfoDevice
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public List<string> layers { get; set; } = new List<string>();
            public ItemInfoType? type { get; set; } = null;

            public List<ItemInfoValue> values { get; set; } = new List<ItemInfoValue>();
            public List<ItemInfoStatus> statuss { get; set; } = new List<ItemInfoStatus>();
        }
        public class ItemPointForDraw
        {
            public string code { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
        }

        public class ItemInfoDeviceForDraw
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public List<string> layers { get; set; } = new List<string>();
            public ItemInfoType? type { get; set; } = null;

            public List<ItemPointForDraw> points { get; set; } = new List<ItemPointForDraw>();
            public List<ItemInfoValue> values { get; set; } = new List<ItemInfoValue>();
            public List<ItemInfoStatus> statuss { get; set; } = new List<ItemInfoStatus>();
        }

        public class ItemInfoType
        {
            public string code { get; set; } = "";
            public string nameType { get; set; } = "";
        }

        public class ItemInfoValue
        {
            public string id { get; set; } = "";
            public string name { get; set; } = "";
            public string unit { get; set; } = "";
            public string value { get; set; } = "";
            public string time { get; set; } = "";

        }

        public List<ItemFullInfoArea> getAllListFullInfoArea(string token)
        {
            using (DataContext context = new DataContext())
            {
                try
                {
                    List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false)
                                                        .Include(s => s.customers)
                                                        .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.layers)
                                                        .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.type).ThenInclude(s => s!.values)
                                                        .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.type).ThenInclude(s => s!.statuss)
                                                        .ToList();

                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();

                    List<ItemFullInfoArea> items = new List<ItemFullInfoArea>();
                    foreach (SqlArea area in areas)
                    {
                        ItemFullInfoArea areaInfo = new ItemFullInfoArea();
                        areaInfo.code = area.code;
                        areaInfo.name = area.nameArea;
                        areaInfo.des = area.des;
                        areaInfo.layers = new List<string> { };
                        areaInfo.points = new List<ItemInfoPoint>();
                        areaInfo.devices = new List<ItemInfoDeviceForDraw>();
                        areaInfo.customers = new List<ItemCustomerArea>();

                        if (area.points == null)
                        {
                            items.Add(areaInfo);
                            continue;
                        }
                        foreach (SqlPoint point in area.points!)
                        {
                            ItemInfoPoint itemPoint = new ItemInfoPoint();
                            itemPoint.code = point.code;
                            itemPoint.name = point.namePoint;
                            itemPoint.des = point.des;
                            itemPoint.latitude = point.latitude;
                            itemPoint.longitude = point.longitude;
                            itemPoint.devices = new List<ItemInfoDevice>();
                            itemPoint.layers = new List<string>();

                            if (point.devices == null)
                            {
                                areaInfo.points.Add(itemPoint);
                                continue;
                            }

                            foreach (SqlDevice device in point.devices!)
                            {
                                ItemInfoDevice itemDevice = new ItemInfoDevice();
                                itemDevice.code = device.code;
                                itemDevice.nameDevice = device.nameDevice;

                                itemDevice.type = new ItemInfoType();
                                itemDevice.values = new List<ItemInfoValue>();
                                if (device.type != null)
                                {
                                    itemDevice.type.code = device.type.code;
                                    itemDevice.type.nameType = device.type.name;

                                    //  Values
                                    if (device.type.values != null)
                                    {
                                        foreach (SqlValue value in device.type.values)
                                        {
                                            ItemInfoValue infoValue = new ItemInfoValue();
                                            infoValue.id = value.ID.ToString();
                                            infoValue.name = value.nameValue;
                                            infoValue.unit = value.unit;
                                            SqlLogValue? logValue = context.logValues!.Include(s => s.device).Where(s => s.device == device).OrderByDescending(s => s.time).FirstOrDefault();
                                            if (logValue != null)
                                            {
                                                infoValue.value = logValue.value;
                                                infoValue.time = logValue.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                            }
                                            itemDevice.values.Add(infoValue);
                                        }
                                    }

                                    // Status
                                    if (device.type.statuss != null)
                                    {
                                        foreach (SqlStatus status in device.type.statuss)
                                        {
                                            ItemInfoStatus infoStatus = new ItemInfoStatus();
                                            infoStatus.code = status.code;
                                            infoStatus.name = status.nameStatus;
                                            infoStatus.isOnline = status.isOnline;
                                            itemDevice.statuss.Add(infoStatus);
                                        }
                                    }
                                }


                                // layers
                                itemDevice.layers = new List<string>();

                                if (device.layers == null)
                                {
                                    itemPoint.devices.Add(itemDevice);
                                }
                                else
                                {
                                    foreach (SqlLayer layer in device.layers)
                                    {
                                        string? tmp = areaInfo.layers.Where(s => s.CompareTo(layer.code) == 0).FirstOrDefault();
                                        if (tmp == null)
                                        {
                                            areaInfo.layers.Add(layer.code);
                                        }

                                        tmp = itemPoint.layers.Where(s => s.CompareTo(layer.code) == 0).FirstOrDefault();
                                        if (tmp == null)
                                        {
                                            itemPoint.layers.Add(layer.code);
                                        }

                                        tmp = itemDevice.layers.Where(s => s.CompareTo(layer.code) == 0).FirstOrDefault();
                                        if (tmp == null)
                                        {
                                            itemDevice.layers.Add(layer.code);
                                        }
                                    }
                                }

                                itemPoint.devices.Add(itemDevice);

                                ItemInfoDeviceForDraw? tempDevice = areaInfo.devices.Where(s => s.code.CompareTo(device.code) == 0).FirstOrDefault();

                                if (tempDevice == null)
                                {
                                    tempDevice = new ItemInfoDeviceForDraw();
                                    tempDevice.code = device.code;
                                    tempDevice.nameDevice = device.nameDevice;
                                    tempDevice.type = itemDevice.type;
                                    tempDevice.values = itemDevice.values;
                                    tempDevice.statuss = itemDevice.statuss;
                                    tempDevice.points = new List<ItemPointForDraw>();

                                    ItemPointForDraw tempPoint = new ItemPointForDraw();
                                    tempPoint.code = point.code;
                                    tempPoint.latitude = point.latitude;
                                    tempPoint.longitude = point.longitude;
                                    tempDevice.points.Add(tempPoint);
                                    tempDevice.layers = itemDevice.layers;
                                    areaInfo.devices.Add(tempDevice);
                                }
                                else
                                {
                                    ItemPointForDraw tempPoint = new ItemPointForDraw();
                                    tempPoint.code = point.code;
                                    tempPoint.latitude = point.latitude;
                                    tempPoint.longitude = point.longitude;
                                    tempDevice.points.Add(tempPoint);

                                }

                            }

                            areaInfo.points.Add(itemPoint);
                        }

                        //customer
                        if(area.customers != null)
                        {
                            foreach (SqlCustomer customer in area.customers)
                            {
                                ItemCustomerArea tmpCustomer = new ItemCustomerArea();

                                tmpCustomer.maDB = customer.code;
                                tmpCustomer.route = customer.route;
                                tmpCustomer.tenkh = customer.name;
                                tmpCustomer.diachi = customer.address;
                                tmpCustomer.sdt = customer.phone;

                                areaInfo.customers.Add(tmpCustomer);
                            }
                        }
                        

                        DateTime begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime end = begin.AddDays(1.0);

                        List<ItemListScheduleArea> temp_schedules = getListScheduleForArea(token, begin, end, area.code);
                        foreach (ItemListScheduleArea tmp in temp_schedules)
                        {
                            areaInfo.schedulesInDay.Add(tmp);
                        }
                        items.Add(areaInfo);
                    }



                    return items;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return new List<ItemFullInfoArea>();
                }
            }
        }

        public class ItemValueForDevice
        {
            public string id { get; set; } = "";
            public string name { get; set; } = "";
            public string unit { get; set; } = "";
            public string value { get; set; } = "";
            public string time { get; set; } = "";

        }
        public class ItemStatusForDevice
        {
            public string code { get; set; } = "";
            public string nameStatus { get; set; } = "";
            public bool isOnline { get; set; }
        }

        public class ItemTypeForDevice
        {
            public string code { get; set; } = "";
            public string nameType { get; set; } = "";


        }

        public class ItemDeviceForPoint
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public string startTimeSchedule { get; set; } = "";
            public ItemTypeForDevice? type { get; set; } = null;
            public List<ItemValueForDevice>? values { get; set; } = null;
            public List<ItemStatusForDevice>? status { get; set; } = null;
        }

        public class ItemPointForArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
            public List<ItemDeviceForPoint> devices { get; set; } = new List<ItemDeviceForPoint>();
        }

        public List<ItemPointForArea> getListPointArea(string token, string code)
        {
            using (DataContext context = new DataContext())
            {
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(token))
                {
                    return new List<ItemPointForArea>();
                }

                SqlArea? m_area = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0)
                                                .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.type).ThenInclude(s => s!.values)
                                                .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.type).ThenInclude(s => s!.statuss)
                                                .FirstOrDefault();

                if (m_area == null)
                {
                    return new List<ItemPointForArea>();
                }

                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.areas).FirstOrDefault();

                if (m_user == null)
                {
                    return new List<ItemPointForArea>();
                }


                List<ItemPointForArea> list = new List<ItemPointForArea>();

                if (m_area.points != null)
                {
                    foreach (SqlPoint item in m_area.points)
                    {
                        ItemPointForArea itemPoint = new ItemPointForArea();

                        itemPoint.code = item.code;
                        itemPoint.name = item.namePoint;
                        itemPoint.des = item.des;
                        itemPoint.note = item.note;
                        itemPoint.latitude = item.latitude;
                        itemPoint.longitude = item.longitude;
                        itemPoint.devices = new List<ItemDeviceForPoint>();
                        if (item.devices != null)
                        {
                            foreach (SqlDevice itemDevice in item.devices)
                            {
                                ItemDeviceForPoint tmp = new ItemDeviceForPoint();

                                tmp.code = itemDevice.code;
                                tmp.nameDevice = itemDevice.nameDevice;
                                tmp.startTimeSchedule = itemDevice.startTimeSChedule.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                tmp.type = new ItemTypeForDevice();
                                tmp.values = new List<ItemValueForDevice>();
                                tmp.status = new List<ItemStatusForDevice>();
                                if (itemDevice.type != null)
                                {
                                    tmp.type.code = itemDevice.type.code;
                                    tmp.type.nameType = itemDevice.type.name;

                                    if (itemDevice.type.statuss != null)
                                    {
                                        foreach (SqlStatus itemStatus in itemDevice.type.statuss)
                                        {
                                            ItemStatusForDevice status = new ItemStatusForDevice();

                                            status.code = itemStatus.code;
                                            status.nameStatus = itemStatus.nameStatus;
                                            status.isOnline = itemStatus.isOnline;

                                            tmp.status.Add(status);
                                        }
                                    }


                                    if (itemDevice.type.values != null)
                                    {
                                        foreach (SqlValue value in itemDevice.type.values)
                                        {
                                            ItemValueForDevice infoValue = new ItemValueForDevice();

                                            infoValue.id = value.ID.ToString();
                                            infoValue.name = value.nameValue;
                                            infoValue.unit = value.unit;
                                            SqlLogValue? logValue = context.logValues!.Include(s => s.device).Where(s => s.device == itemDevice).OrderByDescending(s => s.time).FirstOrDefault();
                                            if (logValue != null)
                                            {
                                                infoValue.value = logValue.value;
                                                infoValue.time = logValue.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                            }

                                            tmp.values.Add(infoValue);
                                        }
                                    }
                                }

                                itemPoint.devices.Add(tmp);
                            }
                            list.Add(itemPoint);
                        }

                    }
                }
                return list;
            }
        }

        //public List<ItemUser> getListUserInArea(string code)
        //{
        //    if (string.IsNullOrEmpty(code))
        //    {
        //        return new List<ItemUser>();
        //    }
        //    using (DataContext context = new DataContext())
        //    {

        //        List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0)
        //             .Include(s => s.users!).ThenInclude(s => s.role)
        //             .Include(s => s.points)
        //             .ToList();
        //        if (areas.Count == 0)
        //        {
        //            return new List<ItemUser>();
        //        }

        //        //List<SqlUser> users = context.users!.Include(s => s.areas).Where(s => s.isdeleted == false).Include(s => s.role).ToList();
        //        List<ItemUser> items = new List<ItemUser>();
        //        if (areas[0].users != null)
        //        {
        //            foreach (SqlUser user in areas[0].users!)
        //            {
        //                ItemUser item = new ItemUser();
        //                item.user = user.user;
        //                item.username = user.username;
        //                item.des = user.des;
        //                item.displayName = user.displayName;
        //                item.numberPhone = user.phoneNumber;
        //                item.avatar = user.avatar;
        //                item.role = user.role != null ? user.role!.name : "";
        //                items.Add(item);
        //            }
        //        }

        //        return items;
        //    }
        //}

        public class ItemPointForDevice
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";

        }

        public class ItemDeviceForArea
        {
            public ItemDeviceForPoint device { get; set; } = new ItemDeviceForPoint();

            public List<ItemPointForDevice>? points { get; set; } = null;

        }

        public List<ItemDeviceForArea> getListDeviceArea(string token, string area)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(area))
            {
                return new List<ItemDeviceForArea>();
            }

            List<ItemDeviceForArea> list = new List<ItemDeviceForArea>();

            List<ItemPointForArea> points = getListPointArea(token, area);

            foreach (ItemPointForArea item in points)
            {

                foreach (ItemDeviceForPoint itemDevice in item.devices)
                {


                    ItemDeviceForArea tmp = new ItemDeviceForArea();

                    tmp.device.code = itemDevice.code;
                    tmp.device.nameDevice = itemDevice.nameDevice;
                    tmp.device.startTimeSchedule = itemDevice.startTimeSchedule;
                    tmp.device.type = itemDevice.type;
                    tmp.device.values = itemDevice.values;
                    tmp.device.status = itemDevice.status;
                    tmp.points = new List<ItemPointForDevice>();

                    ItemDeviceForArea? deviceArea = list.Where(s => s.device.code.CompareTo(tmp.device.code) == 0).FirstOrDefault();

                    if (deviceArea == null)
                    {
                        ItemPointForDevice? pointDevice = tmp.points.Where(s => s.code.CompareTo(item.code) == 0).FirstOrDefault();

                        if (pointDevice == null)
                        {
                            ItemPointForDevice itemPointForDevice = new ItemPointForDevice();

                            itemPointForDevice.code = item.code;
                            itemPointForDevice.name = item.name;
                            itemPointForDevice.latitude = item.latitude;
                            itemPointForDevice.longitude = item.longitude;
                            tmp.points.Add(itemPointForDevice);
                        }
                        list.Add(tmp);
                    }
                    else
                    {
                        ItemPointForDevice? point = deviceArea.points!.Where(s => s.code.CompareTo(item.code) == 0).FirstOrDefault();
                        if (point == null)
                        {
                            ItemPointForDevice itemPointForDevice = new ItemPointForDevice();

                            itemPointForDevice.code = item.code;
                            itemPointForDevice.name = item.name;
                            itemPointForDevice.latitude = item.latitude;
                            itemPointForDevice.longitude = item.longitude;
                            deviceArea.points!.Add(itemPointForDevice);
                        }
                    }

                }


            }
            return list;
        }
        public class ItemDeviceForType
        {
            public string codeDevice { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public string timeStartSchedule { get; set; } = "";

            public List<ItemPointForDevice> points { get; set; } = new List<ItemPointForDevice>();
        }
        public class ItemTypeForArea
        {
            public string code { get; set; } = "";
            public string nameType { get; set; } = "";
            public List<ItemDeviceForType> devices { get; set; } = new List<ItemDeviceForType>();
        }

        public List<ItemTypeForArea> getListTypeForArea(string token, string area)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(area))
            {
                return new List<ItemTypeForArea>();
            }
            List<ItemTypeForArea> list = new List<ItemTypeForArea>();

            List<ItemDeviceForArea> listDevice = getListDeviceArea(token, area);


            foreach (ItemDeviceForArea itemDevice in listDevice)
            {
                ItemTypeForArea itemType = new ItemTypeForArea();

                itemType.code = itemDevice.device.type!.code;
                itemType.nameType = itemDevice.device.type!.nameType;


                ItemTypeForArea? tmp_type = list.Where(s => s.code.CompareTo(itemType.code) == 0).FirstOrDefault();

                if (tmp_type == null)
                {
                    ItemDeviceForType? tmp_device = itemType.devices.Where(s => s.codeDevice.CompareTo(itemDevice.device.code) == 0).FirstOrDefault();

                    if (tmp_device == null)
                    {
                        ItemDeviceForType itemDeviceForType = new ItemDeviceForType();

                        itemDeviceForType.codeDevice = itemDevice.device.code;
                        itemDeviceForType.nameDevice = itemDevice.device.nameDevice;
                        itemDeviceForType.timeStartSchedule = itemDevice.device.startTimeSchedule;
                        if (itemDevice.points != null)
                        {
                            itemDeviceForType.points = itemDevice.points;
                        }
                        itemType.devices.Add(itemDeviceForType);
                    }
                    list.Add(itemType);
                }
                else
                {
                    ItemDeviceForType? tmp_device = tmp_type.devices.Where(s => s.codeDevice.CompareTo(itemDevice.device.code) == 0).FirstOrDefault();

                    if (tmp_device == null)
                    {
                        ItemDeviceForType itemDeviceForType = new ItemDeviceForType();

                        itemDeviceForType.codeDevice = itemDevice.device.code;
                        itemDeviceForType.nameDevice = itemDevice.device.nameDevice;
                        itemDeviceForType.timeStartSchedule = itemDevice.device.startTimeSchedule;
                        if (itemDevice.points != null)
                        {
                            itemDeviceForType.points = itemDevice.points;
                        }
                        tmp_type.devices.Add(itemDeviceForType);
                    }
                }


            }
            return list;
        }

        public class ItemLogPoint
        {
            public string code { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
            public string note { get; set; } = "";
            public string timeRef { get; set; } = "";

        }



        public class ItemLogUser
        {
            public string user { get; set; } = "";
            public string name { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
            public string timeDo { get; set; } = "";
        }



        public class ItemLog
        {
            public ItemLogPoint? point { get; set; } = null;
            public string note { get; set; } = "";
            public List<ItemLogUser>? user { get; set; } = null;

        }

        public List<ItemLog> getListItemLogArea(string token, string schedule, string device)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(schedule))
            {
                return new List<ItemLog>();
            }
            using (DataContext context = new DataContext())
            {
                List<ItemLog> list = new List<ItemLog>();
                List<SqlLogDevice>? m_logs = context.logDevices!.Include(s => s.schedule).Include(s => s.device).Where(s => s.schedule!.code.CompareTo(schedule) == 0 && s.device!.code.CompareTo(device) == 0).Include(s => s.point).Include(s => s.device).Include(s => s.user).ToList();
                foreach (SqlLogDevice log in m_logs)
                {
                    if (log.point != null)
                    {
                        ItemLog? tmp = list.Where(s => s.point!.code.CompareTo(log.point.code) == 0).FirstOrDefault();
                        if (tmp == null)
                        {
                            ItemLog item = new ItemLog();

                            ItemLogPoint point = new ItemLogPoint();
                            point.code = log.point.code;
                            point.latitude = log.point.latitude;
                            point.longitude = log.point.longitude;
                            if (log.timeRef != DateTime.MinValue)
                            {
                                point.timeRef = log.timeRef.ToLocalTime().ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                point.timeRef = "";
                            }

                            item.point = point;
                            item.note = log.note;
                            item.user = new List<ItemLogUser>();
                            if (log.user != null)
                            {

                                if(log.timeDo != DateTime.MinValue)
                                {
                                    ItemLogUser user = new ItemLogUser();

                                    user.user = log.user.user;
                                    user.name = log.user.displayName;

                                    if (log.images == null)
                                    {
                                        user.images = new List<string>();
                                    }
                                    else
                                    {
                                        user.images = log.images;
                                    }

                                    if (log.timeDo != DateTime.MinValue)
                                    {
                                        user.timeDo = log.timeDo.ToLocalTime().ToString("dd-MM-yyyy");
                                    }
                                    else
                                    {
                                        user.timeDo = "";
                                    }
                                    item.user.Add(user);
                                }


                            }
                            list.Add(item);

                        }
                        else
                        {
                            tmp.note = log.note;

                            if (log.timeRef != DateTime.MinValue)
                            {
                                tmp.point!.timeRef = log.timeRef.ToLocalTime().ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                tmp.point!.timeRef = "";
                            }
                            if (log.user != null)
                            {
                                if(log.timeDo != DateTime.MinValue)
                                {
                                    ItemLogUser user = new ItemLogUser();

                                    user.user = log.user.user;
                                    user.name = log.user.displayName;

                                    if (log.images == null)
                                    {
                                        user.images = new List<string>();
                                    }
                                    else
                                    {
                                        user.images = log.images;
                                    }
                                    if (log.timeDo != DateTime.MinValue)
                                    {
                                        user.timeDo = log.timeDo.ToLocalTime().ToString("dd-MM-yyyy");
                                    }
                                    else
                                    {
                                        user.timeDo = "";
                                    }
                                    tmp.user!.Add(user);
                                }

                            }
                        }


                    }
                }
                return list;

            }

        }

        public class ItemInfoSchedule
        {
            public string code { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string period { get; set; } = "";
            public List<ItemLog> infoLogs { get; set; } = new List<ItemLog>();
            public string worked { get; set; } = "";
        }

        public class ItemInfoDeviceForSchedules
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public List<ItemPointForDevice> points { get; set; } = new List<ItemPointForDevice>();
            public List<ItemInfoSchedule> schedules { get; set; } = new List<ItemInfoSchedule>();
        }


        public class ItemListScheduleArea
        {
            public string time { get; set; } = "";
            public List<ItemInfoDeviceForSchedules> devices { get; set; } = new List<ItemInfoDeviceForSchedules>();
        }


        public List<ItemListScheduleArea> getListScheduleForArea(string token, DateTime begin, DateTime end, string area)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(area))
            {
                return new List<ItemListScheduleArea>();
            }
            List<ItemListScheduleArea> list = new List<ItemListScheduleArea>();
            List<ItemTypeForArea> types = getListTypeForArea(token, area);
            using (DataContext context = new DataContext())
            {
                foreach (ItemTypeForArea type in types)
                {
                    SqlType? m_type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type.code) == 0).Include(s => s.schedules).FirstOrDefault();

                    if (m_type == null)
                    {
                        return new List<ItemListScheduleArea>();
                    }
                    if (m_type.schedules != null)
                    {
                        foreach (ItemDeviceForType device in type.devices)
                        {
                            foreach (SqlSchedule schedule in m_type.schedules)
                            {
                                int day = int.Parse(schedule.period);
                                DateTime time = DateTime.ParseExact(device.timeStartSchedule, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                //Console.WriteLine(time.Date.AddDays(day) + " -- " + begin + " -- " + end);
                                if (DateTime.Compare(time.Date.AddDays(day), begin.Date) >= 0 && DateTime.Compare(time.Date.AddDays(day), end.Date) < 0)
                                {


                                    ItemListScheduleArea? temp = list.Where(s => s.time.CompareTo(time.Date.AddDays(day).ToString("dd-MM-yyyy")) == 0).FirstOrDefault();
                                    if (temp == null)
                                    {

                                        ItemListScheduleArea tmp = new ItemListScheduleArea();
                                        tmp.time = time.Date.AddDays(day).ToString("dd-MM-yyyy");

                                        ItemInfoDeviceForSchedules m_device = new ItemInfoDeviceForSchedules();
                                        m_device.code = device.codeDevice;
                                        m_device.name = device.nameDevice;
                                        m_device.points = device.points;

                                        ItemInfoSchedule m_item = new ItemInfoSchedule();
                                        m_item.code = schedule.code;
                                        m_item.des = schedule.des;
                                        m_item.period = schedule.period;
                                        m_item.note = schedule.note;

                                        m_item.infoLogs = getListItemLogArea(token, schedule.code, device.codeDevice);
                                        if (m_item.infoLogs.Count > 0)
                                        {
                                            foreach (ItemLog item in m_item.infoLogs)
                                            {
                                                if (!string.IsNullOrEmpty(item.point!.timeRef))
                                                {
                                                    if (item.user != null)
                                                    {
                                                        ItemLogUser? tempLog = item.user!.Where(s => !string.IsNullOrEmpty(s.timeDo)).FirstOrDefault();

                                                        if (tempLog != null)
                                                        {
                                                            m_item.worked = "Done";
                                                        }
                                                        else
                                                        {
                                                            m_item.worked = "Not Done";
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        m_device.schedules.Add(m_item);
                                        tmp.devices.Add(m_device);

                                        list.Add(tmp);
                                    }
                                    else
                                    {
                                        ItemInfoDeviceForSchedules m_device = new ItemInfoDeviceForSchedules();
                                        m_device.code = device.codeDevice;
                                        m_device.name = device.nameDevice;
                                        m_device.points = device.points;

                                        ItemInfoSchedule m_item = new ItemInfoSchedule();
                                        m_item.code = schedule.code;
                                        m_item.des = schedule.des;
                                        m_item.period = schedule.period;
                                        m_item.note = schedule.note;
                                        m_item.infoLogs = getListItemLogArea(token, schedule.code, device.codeDevice);
                                        if (m_item.infoLogs.Count > 0)
                                        {
                                            foreach (ItemLog item in m_item.infoLogs)
                                            {
                                                if (!string.IsNullOrEmpty(item.point!.timeRef))
                                                {
                                                    if (item.user != null)
                                                    {
                                                        ItemLogUser? tempLog = item.user!.Where(s => !string.IsNullOrEmpty(s.timeDo)).FirstOrDefault();

                                                        if (tempLog != null)
                                                        {
                                                            m_item.worked = "Done";
                                                        }
                                                        else
                                                        {
                                                            m_item.worked = "Not Done";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        m_device.schedules.Add(m_item);
                                        temp.devices.Add(m_device);
                                    }
                                }
                            }
                        }
                    }
                }
                return list;
            }
        }



        public async Task<bool> addCustomerAsync(string area, string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlArea? m_area = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(area) == 0).Include(s => s.users).FirstOrDefault();

                if (m_area == null)
                {
                    return false;
                }


                SqlCustomer? m_customer = context.customers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();

                if (m_customer == null)
                {
                    return false;
                }
                if (m_area.customers == null)
                {
                    m_area.customers = new List<SqlCustomer>();
                }
                m_area.lastestTime = DateTime.Now.ToUniversalTime();

                m_area.customers.Add(m_customer);

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

        public async Task<bool> removeCustomerAsync(string area, string code)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false && s.code.CompareTo(area) == 0).Include(s => s.users).ToList();
                if (areas.Count <= 0)
                {
                    return false;
                }
                foreach (SqlArea m_area in areas)
                {
                    List<SqlCustomer> customers = m_area.customers!.Where(s => s.code.CompareTo(code) == 0).ToList();
                    if (customers.Count <= 0)
                    {
                        continue;
                    }
                    foreach (SqlCustomer m_customer in customers)
                    {
                        m_area.customers!.Remove(m_customer);
                        m_area.lastestTime = DateTime.Now.ToUniversalTime();
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
        }



        /* public class ItemInfoLog
         {
             ItemLogDevice? device { get; set; } = null;
             ItemInfoSchedule? scheduleLog { get; set; } = null;

         }

         public class ItemInfoManagerSchedule
         {
             public string time { get; set; } = "";
             public List<ItemInfoLog> info { get; set; } = new List<ItemInfoLog>();
         }

         public List<ItemInfoManagerSchedule> getListManageSchedule(string token, DateTime begin, DateTime end, string area)
         {
             List<ItemInfoManagerSchedule> list = new List<ItemInfoManagerSchedule>();
             List<ItemListScheduleArea> schedules = getListScheduleForArea(token, begin, end, area);
             foreach (ItemListScheduleArea schedule in schedules)
             {
                 ItemInfoManagerSchedule item = new ItemInfoManagerSchedule();
                 item.time = schedule.time;
                 item.info = new List<ItemInfoLog>();
                 ItemInfoLog? tmp = schedule.schedules.Where(s => s.code.CompareTo)
             }    

             return list;
         }*/
    }
}
