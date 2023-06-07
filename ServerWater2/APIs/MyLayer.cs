using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.APIs
{
    public class MyLayer
    {
        public MyLayer() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlLayer? layer = context.layers!.Where(s => s.code.CompareTo("lay1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (layer == null)
                {
                    SqlLayer item = new SqlLayer();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "lay1";
                    item.nameLayer = "layer 1";
                    item.des = "layer 1";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.layers!.Add(item);
                }

                layer = context.layers!.Where(s => s.code.CompareTo("lay2") == 0).FirstOrDefault();
                if (layer == null)
                {
                    SqlLayer item = new SqlLayer();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "lay2";
                    item.nameLayer = "layer 2";
                    item.des = "layer 2";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.layers!.Add(item);
                }             

                int rows = await context.SaveChangesAsync();
            }
        }
        public async Task<bool> createLayerAsync(string code, string name, string des)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0 || s.nameLayer.CompareTo(name) == 0)).ToList();
                if (layers.Count > 0)
                {
                    return false;
                }
                SqlLayer layer = new SqlLayer();
                layer.ID = DateTime.Now.Ticks;
                layer.code = code;
                layer.nameLayer = name;
                layer.des = des;
                layer.isdeleted = false;
                layer.createdTime = DateTime.Now.ToUniversalTime();
                layer.lastestTime = DateTime.Now.ToUniversalTime();
                context.layers!.Add(layer);
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

        public async Task<bool> editLayerAsync(string code, string des, string name)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).ToList();
                if (layers.Count <= 0)
                {
                    return false;
                }
                foreach (SqlLayer layer in layers)
                {
                    layer.nameLayer = name;
                    layer.des = des;
                    layer.lastestTime = DateTime.Now.ToUniversalTime();
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

        public async Task<bool> deleteLayerAsync(string code)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0)
                                                    .ToList();
                if (layers.Count <= 0)
                {
                    return false;
                }
                foreach (SqlLayer layer in layers)
                {
                    layer.isdeleted = true;
                    layer.lastestTime = DateTime.Now.ToUniversalTime();
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

       
        //public class ItemValue
        //{
        //    public string name { get; set; } = "";
        //    public string unit { get; set; } = "";

        //}

        //public class ItemStatus
        //{
        //    public string code { get; set; } = "";
        //    public string nameStatus { get; set; } = "";
        //    public bool isOnline { get; set; }
        //}

        //public class ItemType
        //{
        //    public string code { get; set; } = "";
        //    public string nameType { get; set; } = "";
        //    public List<ItemStatus> status { get; set; } = new List<ItemStatus>();
        //    public List<ItemValue> itemValue { get; set; } = new List<ItemValue>();
        //}

        public class ItemPointForDevice
        {
            public string id { get; set; } = "";
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string longitude { get; set; } = "";
            public string latitude { get; set; } = "";
        }

        public class ItemDeviceForLayer
        {
            public string code { get; set; } = "";
            public string nameDevice { get; set; } = "";
            public List<ItemPointForDevice> points { get; set; } = new List<ItemPointForDevice>();
        }


        public class ItemLayer
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public List<ItemDeviceForLayer> devices { get; set; } = new List<ItemDeviceForLayer>();
        }
        public List<ItemLayer> getListLayer()
        {

            using (DataContext context = new DataContext())
            {

                List<SqlLayer> layers = context.layers!.Where(s => s.isdeleted == false).Include(s => s.devices!).ThenInclude(s => s.points).ToList();
                List<ItemLayer> items = new List<ItemLayer>();
                foreach (SqlLayer layer in layers)
                {
                    if (layer.isdeleted == false)
                    {
                        ItemLayer itemLayer = new ItemLayer();
                        itemLayer.code = layer.code;
                        itemLayer.name = layer.nameLayer;
                        itemLayer.des = layer.des;
                        if (layer.devices != null)
                        {
                            foreach (SqlDevice device in layer.devices)
                            {
                                ItemDeviceForLayer itemDevice = new ItemDeviceForLayer();
                                itemDevice.code = device.code;
                                itemDevice.nameDevice = device.nameDevice;

                                if(device.points != null)
                                {
                                    foreach (SqlPoint item in device.points)
                                    {
                                        ItemPointForDevice itemPoint = new ItemPointForDevice();

                                        itemPoint.id = item.ID.ToString();
                                        itemPoint.code = item.code;
                                        itemPoint.name = item.namePoint;
                                        itemPoint.latitude = item.latitude;
                                        itemPoint.longitude = item.longitude;

                                        itemDevice.points.Add(itemPoint);

                                    }
                                }
                                else
                                {
                                    itemDevice.points = new List<ItemPointForDevice>();
                                }

                                itemLayer.devices.Add(itemDevice);
                            }
                        }
                        items.Add(itemLayer);
                    }
                }
                return items;
            }
        }



    }
}
