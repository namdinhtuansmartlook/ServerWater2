using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.APIs
{
    public class MyDevice
    {
        public MyDevice() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlDevice? device = context.devices!.Where(s => s.code.CompareTo("dv1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (device == null)
                {
                    SqlDevice item = new SqlDevice();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "dv1";
                    item.nameDevice = "thiết bị 1";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    item.type = context.types!.Where(s => s.code.CompareTo("type1") == 0 && s.isdeleted == false).FirstOrDefault();
                    context.devices!.Add(item);
                }

                device = context.devices!.Where(s => s.code.CompareTo("dv2") == 0 && s.isdeleted == false).FirstOrDefault();
                if (device == null)
                {
                    SqlDevice item = new SqlDevice();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "dv2";
                    item.nameDevice = "thiết bị 2";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    item.type = context.types!.Where(s => s.code.CompareTo("type1") == 0 && s.isdeleted == false).FirstOrDefault();
                    context.devices!.Add(item);
                }
               

                int rows = await context.SaveChangesAsync();
            }
        }
        public async Task<bool> createDevice(string code, string name, string codeType)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(codeType))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlDevice? device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (device != null)
                {
                    return false;
                }
                SqlType? type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(codeType) == 0).FirstOrDefault();
                if (type == null)
                {
                    return false;
                }
                device = new SqlDevice();
                device.ID = DateTime.Now.Ticks;
                device.code = code;
                device.nameDevice = name;
                device.isdeleted = false;
                device.createdTime = DateTime.Now.ToUniversalTime();
                device.lastestTime = device.createdTime;
                device.type = type;
                context.devices!.Add(device);

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

        public async Task<bool> editDevice(string code, string name, string codeType)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(codeType))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlDevice? device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (device == null)
                {
                    return false;
                }
                SqlType? type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(codeType) == 0).FirstOrDefault();
                if (type == null)
                {
                    return false;
                }
                device.nameDevice = name;
                device.type = type;
                device.lastestTime = DateTime.Now.ToUniversalTime();

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

        public async Task<bool> deleteDevice(string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlDevice? device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (device == null)
                {
                    return false;
                }

                device.isdeleted = true;
                device.lastestTime = DateTime.Now.ToUniversalTime();

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

        public async Task<bool> addLayerDevice(string code, string device)
        {
            using (DataContext context = new DataContext())
            {
                //SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();

                //if(user == null)
                //{
                //    return false;
                //}

                SqlLayer? layer = context.layers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (layer == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.layers).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                m_device.layers!.Add(layer);
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

        public async Task<bool> removeLayerDevice(string code, string device)
        {
            using (DataContext context = new DataContext())
            {
                SqlLayer? layer = context.layers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (layer == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.layers).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                m_device.layers!.Remove(layer);
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
        public class ItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public List<ItemDevice> itemDevices { get; set; } = new List<ItemDevice>();
        }

        public async Task<bool> setTimeScheduleDevice(string token, string device, DateTime time)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }

                m_device.startTimeSChedule = time.ToUniversalTime();

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

        public class ItemValue
        {
            public string id { get; set; } = "";
            public string name { get; set; } = "";
            public string unit { get; set; } = "";
            public string value { get; set; } = "";
            public string time { get; set; } = "";

        }
        public class ItemStatus
        {
            public string code { get; set; } = "";
            public string nameStatus { get; set; } = "";
            public bool isOnline { get; set; }
        }

        public class ItemType
        {
            public string code { get; set; } = "";
            public string nameType { get; set; } = "";
            public List<ItemStatus> status { get; set; } = new List<ItemStatus>();
            public List<ItemValue> itemValue { get; set; } = new List<ItemValue>();
        }

        public class ItemSchedule
        {
            public string code { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string period { get; set; } = "";
        }

        public class ItemPoint
        {
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
        }
        public class ItemLayer
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
        }
        public class ItemDevice
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public string time { get; set; } = "";
            public ItemType? type { get; set; } = null;
            public List<ItemPoint> points { get; set; } = new List<ItemPoint>();
            public List<ItemLayer> layers { get; set; } = new List<ItemLayer>();
            public List<ItemSchedule> schedules { get; set; } = new List<ItemSchedule>();
        }

        public List<ItemDevice> getListDevice()
        {
            using (DataContext context = new DataContext())
            {
                List<ItemDevice> list = new List<ItemDevice>();
                List<SqlDevice> devices = context.devices!.Where(s => s.isdeleted == false)
                                                          .Include(s => s.type).ThenInclude(s => s!.values)
                                                          .Include(s => s.type).ThenInclude(s => s!.statuss)
                                                          .Include(s => s.type).ThenInclude(s => s!.schedules)
                                                          .Include(s => s.points)
                                                          .Include(s => s.layers)
                                                          .ToList();
                if (devices.Count > 0)
                {
                    foreach (SqlDevice device in devices)
                    {
                        ItemDevice item = new ItemDevice();
                        item.code = device.code;
                        item.nameDevice = device.nameDevice;
                        item.time = device.startTimeSChedule.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

                        if (device.type != null)
                        {
                            ItemType type = new ItemType();
                            type.code = device.type.code;
                            type.nameType = device.type.name;

                            item.type = type;
                            if (device.type.statuss != null)
                            {
                                foreach (SqlStatus status in device.type.statuss)
                                {
                                    ItemStatus itemStatus = new ItemStatus();

                                    itemStatus.code = status.code;
                                    itemStatus.nameStatus = status.nameStatus;
                                    itemStatus.isOnline = status.isOnline;

                                    type.status.Add(itemStatus);
                                }
                            }
                            if (device.type.values != null)
                            {
                                foreach (SqlValue value in device.type.values)
                                {
                                    ItemValue itemValue = new ItemValue();

                                    itemValue.id = value.ID.ToString();
                                    itemValue.name = value.nameValue;
                                    itemValue.unit = value.unit;                      
                                    SqlLogValue? logValue = context.logValues!.Include(s => s.valueConfig).Include(s => s.device).Where(s => s.valueConfig == value && s.device == device).OrderByDescending(s => s.time).FirstOrDefault();
                                    if (logValue != null)
                                    {
                                        itemValue.value = logValue.value;
                                        itemValue.time = logValue.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                    }
                                    type.itemValue.Add(itemValue);
                                }
                            }
                            if(device.type.schedules != null)
                            {
                                foreach (SqlSchedule m_schedule in device.type.schedules)
                                {
                                    ItemSchedule tmp = new ItemSchedule();
                                    tmp.code = m_schedule.code;
                                    tmp.des = m_schedule.des;
                                    tmp.note = m_schedule.note;
                                    tmp.period = m_schedule.period;
                                    item.schedules.Add(tmp);
                                }
                            }    
                        }
                        List<SqlPoint> points = device.points!.Where(s => s.isdeleted == false).ToList();
                        foreach (SqlPoint point in points)
                        {
                            ItemPoint itemPoint = new ItemPoint();
                            itemPoint.longitude = point.longitude;
                            itemPoint.latitude = point.latitude;
                            item.points.Add(itemPoint);
                        }
                        List<SqlLayer> layers = device.layers!.Where(s => s.isdeleted == false).ToList();
                        foreach (SqlLayer layer in layers)
                        {
                            ItemLayer itemLayer = new ItemLayer();
                            itemLayer.code = layer.code;
                            itemLayer.name = layer.nameLayer;
                            item.layers.Add(itemLayer);
                        }
                        
                        list.Add(item);
                    }
                }
                return list;
            }
        }

        public class ItemValueDevice
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public string nameValue { get; set; } = "";
            public string unit { get; set; } = "";
            public string value { get; set; } = "";
            public string timeSetValue { get; set; } = "";
        }

        public List<ItemValueDevice> getListValueDevice(List<string> devices)
        {
            List<ItemValueDevice> list = new List<ItemValueDevice>();
            using (DataContext context = new DataContext())
            {
                List<SqlDevice> m_devices = new List<SqlDevice>();
                foreach (string tmp in devices)
                {
                    SqlDevice? device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(tmp) == 0).FirstOrDefault();
                    if (device != null)
                    {
                        m_devices.Add(device);
                    }
                }
                foreach (SqlDevice device in m_devices)
                {
                    ItemValueDevice itemValueDevice = new ItemValueDevice();
                    itemValueDevice.code = device.code;
                    itemValueDevice.nameDevice = device.nameDevice;
                    SqlLogValue? logValue = context.logValues!.Include(s => s.device).Where(s => s.device == device).Include(s => s.valueConfig).OrderByDescending(s => s.time).FirstOrDefault();
                    if (logValue != null)
                    {
                        itemValueDevice.nameValue = logValue.valueConfig!.nameValue;
                        itemValueDevice.unit = logValue.valueConfig.unit;
                        itemValueDevice.value = logValue.value;
                        itemValueDevice.timeSetValue = logValue.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                        list.Add(itemValueDevice);
                    }
                }
                return list;
            }
        }
    }
}
