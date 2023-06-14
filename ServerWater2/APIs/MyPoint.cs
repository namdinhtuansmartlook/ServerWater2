using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.APIs
{
    public class MyPoint
    {
        public MyPoint()
        {
        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlPoint? point = context.points!.Where(s => s.code.CompareTo("p1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (point == null)
                {
                    SqlPoint item = new SqlPoint();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "p1";
                    item.namePoint = "point 1";
                    item.des = "point 1";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.points!.Add(item);
                }

                point = context.points!.Where(s => s.code.CompareTo("p2") == 0 && s.isdeleted == false).FirstOrDefault();
                if (point == null)
                {
                    SqlPoint item = new SqlPoint();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "p2";
                    item.namePoint = "point 2";
                    item.des = "point 2";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.points!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }
        public async Task<bool> createPoint(string token, string code, string name, string des, string longi, string lati, string note)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlPoint? point = context.points!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (point != null)
                {
                    return false;
                }
                point = new SqlPoint();
                point.ID = DateTime.Now.Ticks;
                point.code = code;
                point.namePoint = name;
                point.des = des;
                point.longitude = longi;
                point.latitude = lati;
                point.note = note;
                point.isdeleted = false;
                point.createdTime = DateTime.Now.ToUniversalTime();
                point.lastestTime = DateTime.Now.ToUniversalTime();
                context.points!.Add(point);
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

        public async Task<string> addImagePoint(string token, string point, byte[] image)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return "";
                }
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_point == null)
                {
                    return "";
                }
                string m_file = "";
                byte[]? tmp = await Program.api_file.getImageChanged(image);
                if (tmp != null)
                {
                    m_file = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), tmp);
                    if (string.IsNullOrEmpty(m_file))
                    {
                        return "";
                    }
                    if (m_point.images == null)
                    {
                        m_point.images = new List<string>();
                    }

                    m_point.images.Add(m_file);
                }
                m_point.lastestTime = DateTime.Now.ToUniversalTime();
                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return m_file;
                }
                else
                {
                    return "";
                }
            }
        }

        public async Task<bool> removeImagePoint(string token, string point, string image)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                if (m_point.images != null)
                {
                    m_point.images.Remove(image);
                }
                if (m_point.imageShow.CompareTo(image) == 0)
                {
                    m_point.imageShow = "";
                }
                m_point.lastestTime = DateTime.Now.ToUniversalTime();
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
        public async Task<bool> editPoint(string token, string code, string name, string des, string longi, string lati, string note)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlPoint? point = context.points!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (point == null)
                {
                    return false;
                }
                point.namePoint = name;
                point.des = des;
                point.longitude = longi;
                point.latitude = lati;
                point.note = note;
                point.lastestTime = DateTime.Now.ToUniversalTime();
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
        public class ItemValue
        {
            public string name { get; set; } = "";
            public string unit { get; set; } = "";

        }
        public class ItemDevice
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public ItemType? type { get; set; } = null;
        }
        public class ItemPoint
        {
            public long ID { get; set; } = 0;
            public string code { get; set; } = "";
            public string namePoint { get; set; } = "";
            public string des { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
            public string note { get; set; } = "";
            public List<ItemDevice> device { get; set; } = new List<ItemDevice>();
            public List<string> images { get; set; } = new List<string>();
        }

        
        public class ItemListPoint
        {
            public long ID { get; set; } = 0;
            public string code { get; set; } = "";
            public string namePoint { get; set; } = "";
            public string des { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
            public string note { get; set; } = "";
            public List<ItemDevicePoint> ItemDevice { get; set; } = new List<ItemDevicePoint>();
            public List<ItemArea> ItemArea { get; set; } = new List<ItemArea>();
            public List<string> images { get; set; } = new List<string>();
        }
        public class ItemDevicePoint
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
        }
        public class ItemArea
        {
            public string area { get; set; } = "";
            public string nameArea { get; set; } = "";
        }
        public class ItemLayer
        {
            public string layer { get; set; } = "";
            public string nameLayer { get; set; } = "";
        }
        public class ItemPointArea
        {
            public string layer { get; set; } = "";
            public string nameLayer { get; set; } = "";
            public List<ItemPoint> itemPoints { get; set; } = new List<ItemPoint>();
        }

        public class ItemPointLayer
        {
            public string area { get; set; } = "";
            public string nameArea { get; set; } = "";
            public List<ItemPoint> itemPoints { get; set; } = new List<ItemPoint>();
        }

        //public List<ItemPointArea> getListPointArea(string token, string area)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        List<ItemPointArea> list = new List<ItemPointArea>();
        //        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.areas!).ThenInclude(s => s.points!).FirstOrDefault();
        //        if (user == null)
        //        {
        //            return new List<ItemPointArea>();
        //        }

        //        SqlArea? m_area = context.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false)
        //                                                                                .Include(s => s.points!).ThenInclude(s => s.devices!).ThenInclude(s => s.type).ThenInclude(s => s!.statuss).FirstOrDefault();
        //        if (m_area == null)
        //        {
        //            return new List<ItemPointArea>();
        //        }
        //        List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false).ToList();
        //        List<SqlPoint> points = m_area.points!.Where(s => s.isdeleted == false).ToList();
        //        foreach (SqlLayer layer in layers)
        //        {
        //            ItemPointArea itemPointArea = new ItemPointArea();
        //            itemPointArea.layer = layer.code;
        //            itemPointArea.nameLayer = layer.nameLayer;
        //            foreach (SqlPoint point in points)
        //            {
                       
        //                    ItemPoint item = new ItemPoint();
        //                    item.ID = point.ID;
        //                    item.code = point.code;
        //                    item.namePoint = point.namePoint;
        //                    item.des = point.des;
        //                    item.longitude = point.longitude;
        //                    item.latitude = point.latitude;
        //                    item.note = point.note;
        //                    if (point.images == null)
        //                    {
        //                        item.images = new List<string>();
        //                    }
        //                    else
        //                    {
        //                        item.images = point.images;
        //                    }
        //                    if (point.devices != null)
        //                    {
        //                        foreach (SqlDevice device in point.devices)
        //                        {
        //                            ItemDevice itemDevice = new ItemDevice();
        //                            itemDevice.code = device.code;
        //                            itemDevice.nameDevice = device.nameDevice;
        //                            if (device.type != null)
        //                            {
        //                                ItemType type = new ItemType();
        //                                type.code = device.type.code;
        //                                type.nameType = device.type.nameType;
        //                                itemDevice.type = type;

        //                                if (device.type.statuss != null)
        //                                {
        //                                    foreach (SqlStatus status in device.type.statuss)
        //                                    {
        //                                        ItemStatus itemStatus = new ItemStatus();

        //                                        itemStatus.code = status.code;
        //                                        itemStatus.nameStatus = status.nameStatus;
        //                                        itemStatus.nameStatus = status.nameStatus;
        //                                        itemStatus.isOnline = status.isOnline;

        //                                        type.status.Add(itemStatus);
        //                                    }
        //                                }
        //                            }
        //                            item.device.Add(itemDevice);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        item.device = new List<ItemDevice>();
        //                    }

        //                    itemPointArea.itemPoints.Add(item);
                        
        //            }
        //            list.Add(itemPointArea);
        //        }
        //        foreach (ItemPointArea item in list.ToList())
        //        {
        //            if (item.itemPoints.Count < 1)
        //            {
        //                list.Remove(item);
        //            }
        //        }
        //        return list;
        //    }
        //}

        //public List<ItemPointLayer> getListPointLayer(string token, string layer)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        List<ItemPointLayer> list = new List<ItemPointLayer>();
        //        SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.areas!).ThenInclude(s => s.points!).FirstOrDefault();
        //        if (user == null)
        //        {
        //            return new List<ItemPointLayer>();
        //        }
        //        SqlLayer? m_layer = context.layers!.Where(s => s.isdeleted == false && s.code.CompareTo(layer) == 0).Include(s => s.points!).ThenInclude(s => s.areas).FirstOrDefault();
        //        if (m_layer == null)
        //        {
        //            return new List<ItemPointLayer>();
        //        }
        //        List<SqlPoint> points = context.points!.Where(s => s.isdeleted == false).Include(s => s.areas).Include(s => s.devices).ToList();
        //        List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false).ToList();
        //        foreach (SqlArea area in areas)
        //        {
        //            ItemPointLayer itemPointLayer = new ItemPointLayer();
        //            itemPointLayer.area = area.code;
        //            itemPointLayer.nameArea = area.nameArea;
        //            foreach (SqlPoint point in points)
        //            {
        //                if (point.areas!.Where(s => s.code.CompareTo(area.code) == 0).FirstOrDefault() != null)
        //                {
        //                    ItemPoint item = new ItemPoint();
        //                    item.ID = point.ID;
        //                    item.code = point.code;
        //                    item.namePoint = point.namePoint;
        //                    item.des = point.des;
        //                    item.longitude = point.longitude;
        //                    item.latitude = point.latitude;
        //                    item.note = point.note;
        //                    if (point.images == null)
        //                    {
        //                        item.images = new List<string>();
        //                    }
        //                    else
        //                    {
        //                        item.images = point.images;
        //                    }
        //                    if (point.devices != null)
        //                    {
        //                        foreach (SqlDevice device in point.devices)
        //                        {
        //                            ItemDevice itemDevice = new ItemDevice();
        //                            itemDevice.code = device.code;
        //                            itemDevice.nameDevice = device.nameDevice;
        //                            if (device.type != null)
        //                            {
        //                                ItemType type = new ItemType();
        //                                type.code = device.type.code;
        //                                type.nameType = device.type.nameType;
        //                                //type.value = device.type.value;


        //                                itemDevice.type = type;

        //                                if (device.type.statuss != null)
        //                                {
        //                                    foreach (SqlStatus status in device.type.statuss)
        //                                    {
        //                                        ItemStatus itemStatus = new ItemStatus();

        //                                        itemStatus.code = status.code;
        //                                        itemStatus.nameStatus = status.nameStatus;
        //                                        itemStatus.nameStatus = status.nameStatus;
        //                                        itemStatus.isOnline = status.isOnline;

        //                                        type.status.Add(itemStatus);
        //                                    }
        //                                }
        //                            }


        //                            item.device.Add(itemDevice);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        item.device = new List<ItemDevice>();
        //                    }
        //                    itemPointLayer.itemPoints.Add(item);
        //                }
        //            }
        //            list.Add(itemPointLayer);
        //        }
        //        foreach (ItemPointLayer item in list.ToList())
        //        {
        //            if (item.itemPoints.Count < 1)
        //            {
        //                list.Remove(item);
        //            }
        //        }
        //        return list;
        //    }
        //}
        public class ItemPointDeviceLayer
        {
            public string code { get; set; } = "";
            public string nameLayer { get; set; } = "";
            public List<ItemPointDevice> itemPointDevices { get; set; } = new List<ItemPointDevice>();
        }
        public class ItemPointDevice
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public List<InfoPoint> infoPoints { get; set; } = new List<InfoPoint>();
        }
        public class InfoPoint
        {
            public long ID { get; set; } = 0;
            public string code { get; set; } = "";
            public string namePoint { get; set; } = "";
            public string longtitude { get; set; } = "";
            public string latitude { get; set; } = "";
        }

        //public List<ItemPointDeviceLayer> getAllPointDeviceLayer(string token)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        List<ItemPointDeviceLayer> result = new List<ItemPointDeviceLayer>();
        //        SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
        //        if (user == null)
        //        {
        //            return new List<ItemPointDeviceLayer>();
        //        }
        //        List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false).ToList();
        //        if (layers.Count > 0)
        //        {
        //            foreach (SqlLayer layer in layers)
        //            {
        //                ItemPointDeviceLayer itemPointDeviceLayer = new ItemPointDeviceLayer();
        //                itemPointDeviceLayer.code = layer.code;
        //                itemPointDeviceLayer.nameLayer = layer.nameLayer;
        //                List<SqlDevice> devices = context.devices!.Where(s => s.isdeleted == false).ToList();
        //                if (devices.Count > 0)
        //                {
        //                    foreach (SqlDevice device in devices)
        //                    {
        //                        ItemPointDevice itemPointDevice = new ItemPointDevice();
        //                        itemPointDevice.code = device.code;
        //                        itemPointDevice.nameDevice = device.nameDevice;
        //                        List<SqlPoint> points = context.points!.Include(s => s.layers).Where(s => s.isdeleted == false && s.layers!.Where(s => s.code.CompareTo(layer.code) == 0).FirstOrDefault() != null).Include(s => s.devices).ToList();
        //                        if (points.Count > 0)
        //                        {
        //                            foreach (SqlPoint point in points)
        //                            {
        //                                if (point.devices!.Where(s => s.code.CompareTo(device.code) == 0).FirstOrDefault() != null)
        //                                {
        //                                    InfoPoint infoPoint = new InfoPoint();
        //                                    infoPoint.ID = point.ID;
        //                                    infoPoint.code = point.code;
        //                                    infoPoint.namePoint = point.namePoint;
        //                                    infoPoint.longtitude = point.longitude;
        //                                    infoPoint.latitude = point.latitude;
        //                                    itemPointDevice.infoPoints.Add(infoPoint);
        //                                }
        //                            }
        //                        }
        //                        itemPointDeviceLayer.itemPointDevices.Add(itemPointDevice);
        //                    }
        //                }
        //                result.Add(itemPointDeviceLayer);
        //            }
        //        }
        //        return result;
        //    }
        //}



        public async Task<bool> addPointArea(string point, string area)
        {
            using (DataContext context = new DataContext())
            {
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlArea? m_area = context.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_area == null)
                {
                    return false;
                }

                SqlArea? tmp = m_point.areas!.Where(s => s.ID == m_area.ID).FirstOrDefault();
                if(tmp == null)
                {
                    m_point.areas!.Add(m_area);
                    m_point.lastestTime = DateTime.Now.ToUniversalTime();
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

        public async Task<bool> removePointArea(string point, string area)
        {
            using (DataContext context = new DataContext())
            {
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).Include(s => s.areas).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlArea? m_area = m_point.areas!.Where(s => s.code.CompareTo(area) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_area == null)
                {
                    return false;
                }
                m_point.areas!.Remove(m_area);
                m_point.lastestTime = DateTime.Now.ToUniversalTime();
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
        public class ListItemPointArea
        {
            public string area { get; set; } = "";
            public string nameArea { get; set; } = "";
            public List<ItemPointArea> itemPointAreas { get; set; } = new List<ItemPointArea>();
        }
        //public List<ListItemPointArea> getAllPointArea(string token)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        List<ListItemPointArea> result = new List<ListItemPointArea>();
        //        SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
        //        if (user == null)
        //        {
        //            return new List<ListItemPointArea>();
        //        }
        //        List<SqlArea> areas = context.areas!.Where(s => s.isdeleted == false).ToList();
        //        foreach (SqlArea area in areas)
        //        {
        //            ListItemPointArea listItemPointArea = new ListItemPointArea();
        //            listItemPointArea.area = area.code;
        //            listItemPointArea.nameArea = area.nameArea;
        //            listItemPointArea.itemPointAreas = getListPointArea(token, area.code);
        //            result.Add(listItemPointArea);
        //        }
        //        return result;
        //    }
        //}

        public async Task<bool> addDevicePoint(string token, string device, string point)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).Include(s => s.devices).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.code.CompareTo(device) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }

                SqlDevice? tmp = m_point.devices!.Where(s => s.ID == m_device.ID).FirstOrDefault();

                if(tmp == null)
                {
                    m_point.devices!.Add(m_device);
                    SqlLogDevice logDevice = new SqlLogDevice();
                    logDevice.ID = DateTime.Now.Ticks;
                    logDevice.device = m_device;
                    logDevice.point = m_point;
                    logDevice.user = user;
                    logDevice.note = string.Format("Add device: {0} to point: {1}", device, point);

                    context.logDevices!.Add(logDevice);
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

        public async Task<bool> removeDevicePoint(string token, string device, string point)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlPoint? m_point = context.points!.Where(s => s.code.CompareTo(point) == 0 && s.isdeleted == false).Include(s => s.devices).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.code.CompareTo(device) == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }

                m_point.devices!.Remove(m_device);
                SqlLogDevice logDevice = new SqlLogDevice();
                logDevice.ID = DateTime.Now.Ticks;
                logDevice.device = m_device;
                logDevice.point = m_point;
                logDevice.user = user;
                logDevice.note = string.Format("Remove device: {0} from point: {1}", device, point);
                context.logDevices!.Add(logDevice);

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

        public async Task<bool> deletePoint(string token, string code)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }

                SqlPoint? point = context.points!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (point == null)
                {
                    return false;
                }

                if (point.areas != null)
                {
                    foreach (SqlArea m_area in point.areas)
                    {
                        await removePointArea(point.code, m_area.code);
                    }
                }
                if (point.devices != null)
                {
                    foreach (SqlDevice m_device in point.devices)
                    {
                        await removeDevicePoint(token, m_device.code, point.code);
                    }
                }

                point.isdeleted = true;

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

        public List<ItemListPoint> getListInfoPoint(string token)
        {
            using (DataContext context = new DataContext())
            {
                List<ItemListPoint> list = new List<ItemListPoint>();
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.areas!).ThenInclude(s => s.points!).FirstOrDefault();
                if (user == null)
                {
                    return new List<ItemListPoint>();
                }

                List<SqlPoint>? points = context.points!.Where(s => s.isdeleted == false).Include(s => s.devices!).Include(s => s.areas).ToList();
                if (points.Count > 0)
                {
                    foreach (SqlPoint point in points)
                    {
                        ItemListPoint itemInfoPoint = new ItemListPoint();
                        itemInfoPoint.ID = point.ID;
                        itemInfoPoint.code = point.code;
                        itemInfoPoint.namePoint = point.namePoint;
                        itemInfoPoint.des = point.des;
                        itemInfoPoint.longitude = point.longitude;
                        itemInfoPoint.latitude = point.latitude;
                        itemInfoPoint.note = point.note;
                        if(point.images != null)
                        {
                            foreach (string item in point.images)
                            {
                                itemInfoPoint.images = new List<string>();

                                itemInfoPoint.images.Add(item);

                            }
                            
                        }
                       
                        List<SqlArea> areas = point.areas!.Where(s => s.isdeleted == false).ToList();
                        foreach (SqlArea area in areas)
                        {
                            ItemArea itemArea = new ItemArea();
                            itemArea.area = area.code;
                            itemArea.nameArea = area.nameArea;
                            itemInfoPoint.ItemArea.Add(itemArea);
                        }
                       
                        List<SqlDevice> devices = point.devices!.Where(s => s.isdeleted == false).ToList();
                        foreach (SqlDevice device in devices)
                        {
                            ItemDevicePoint itemDevice = new ItemDevicePoint();
                            itemDevice.code = device.code;
                            itemDevice.nameDevice = device.nameDevice;
                            itemInfoPoint.ItemDevice.Add(itemDevice);
                        }
                        list.Add(itemInfoPoint);
                    }
                }
                return list;
            }
        }
    }
}
