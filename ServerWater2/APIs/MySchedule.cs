using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.APIs
{
    public class MySchedule
    {
        public MySchedule() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlSchedule? schedule = context.schedules!.Where(s => s.code.CompareTo("sch1") == 0 && s.isdeleted == false).FirstOrDefault();
                if (schedule == null)
                {
                    SqlSchedule item = new SqlSchedule();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "sch1";
                    item.des = "schedule 1";
                    item.note = "schedule 1";
                    item.period = "1";
                    item.createdTime = DateTime.Now.ToUniversalTime();
                    item.lastestTime = item.createdTime;
                    item.isdeleted = false;
                    context.schedules!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }
        }


        public async Task<bool> createSchedule(string code, string des, string note, string period)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                //SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                //if (user == null)
                //{
                //    return false;
                //}
                SqlSchedule? schedule = context.schedules!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (schedule != null)
                {
                    return false;
                }
                schedule = new SqlSchedule();
                schedule.ID = DateTime.Now.Ticks;
                schedule.code = code;
                schedule.des = des;
                schedule.note = note;
                schedule.period = period;
                schedule.isdeleted = false;
                schedule.createdTime = DateTime.Now.ToUniversalTime();
                schedule.lastestTime = DateTime.Now.ToUniversalTime();

                context.schedules!.Add(schedule);
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
        public async Task<bool> editSchedule(string code, string des, string note, string period)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                //SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                //if (user == null)
                //{
                //    return false;
                //}
                SqlSchedule? schedule = context.schedules!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (schedule == null)
                {
                    return false;
                }
                schedule.des = des;
                schedule.note = note;
                schedule.period = period;
                schedule.lastestTime = DateTime.Now.ToUniversalTime();

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
        public async Task<bool> deleteSchedule(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                //SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).FirstOrDefault();
                //if (user == null)
                //{
                //    return false;
                //}
                SqlSchedule? schedule = context.schedules!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (schedule == null)
                {
                    return false;
                }
                schedule.isdeleted = true;
                schedule.lastestTime = DateTime.Now.ToUniversalTime();

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

        public class ItemType
        {
            public string code { get; set; } = "";
            public string nameType { get; set; } = "";
          
        }
        public class ItemUser
        {
            public string user { get; set; } = "";
            public string displayName { get; set; } = "";
            public string avatar { get; set; } = "";
        }
        public class ItemSchedule
        {
            public string code { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string period { get; set; } = "";
        }

        public List<ItemSchedule> getListSchedule()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlSchedule> schedules = context.schedules!.Where(s => s.isdeleted == false)
                                                               .ToList();
                if(schedules == null)
                {
                    return new List<ItemSchedule>();
                }
                List<ItemSchedule> items = new List<ItemSchedule>();
                foreach (SqlSchedule schedule in schedules)
                {
                    ItemSchedule itemSchedule = new ItemSchedule();
                    itemSchedule.code = schedule.code;
                    itemSchedule.des = schedule.des;
                    itemSchedule.note = schedule.note;
                    //itemSchedule.startTime = schedule.startTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
                    itemSchedule.period = schedule.period;
                    items.Add(itemSchedule);
                }
                return items;
            }
        }

        

       /* public List<ItemSchedule> getListSchedule()
        {

            using (DataContext context = new DataContext())
            {
                List<SqlSchedule>? schedules = context.schedules!.Where(s => s.isdeleted == false).ToList();

                if (schedules == null)
                {
                    return new List<ItemSchedule>();
                }
                List<ItemSchedule> items = new List<ItemSchedule>();
                if (schedules.Count > 0)
                {
                    foreach (SqlSchedule schedule in schedules)
                    {
                        if (schedule.isdeleted == false)
                        {
                            ItemSchedule itemSchedule = new ItemSchedule();
                            itemSchedule.code = schedule.code;
                            itemSchedule.des = schedule.des;
                            itemSchedule.note = schedule.note;
                            itemSchedule.period = schedule.period;

                            items.Add(itemSchedule);
                        }
                    }

                }
                return items;
            }
        }*/
       

      /*  public async Task<bool> setAlertJob (string token,string device)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                SqlType? type = context.types!.Where(s => s.isdeleted == false && s.ID == m_device.type!.ID).Include(s => s.schedules).FirstOrDefault();
                if (type == null)
                {
                    return false;
                }
                if (type.schedules == null)
                {
                    return false;
                }
                
                foreach (SqlSchedule schedule in type.schedules)
                {
                    DateTime tmp = m_device.startTimeSChedule.AddDays(int.Parse(schedule.period));
                    if (DateTime.Compare(tmp.ToUniversalTime(),DateTime.Now.ToUniversalTime()) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
*/

       /* public List<ItemSchedule> getListScheduleDevice(DateTime time, string device)
        {
            using (DataContext context = new DataContext())
            {
                List<ItemSchedule> list = new List<ItemSchedule>();
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return new List<ItemSchedule>();
                }
                List<SqlSchedule> schedules = m_device.type!.schedules!.Where(s => s.isdeleted == false).ToList();
                if (schedules.Count > 0)
                {
                    foreach (SqlSchedule schedule in schedules)
                    {
                        int day = int.Parse(schedule.period);
                        if (DateTime.Compare(m_device.startTimeSChedule.Date.AddDays(day), time.Date) == 0)
                        {
                            ItemSchedule scheduleDevice = new ItemSchedule();
                            scheduleDevice.period = schedule.period;
                            scheduleDevice.code = schedule.code;
                            scheduleDevice.des = scheduleDevice.des;
                            scheduleDevice.note = scheduleDevice.note;
                            list.Add(scheduleDevice);
                        }
                    }
                }
                return list;
            }
        }*/

        /*public List<ItemSchedule> getListScheduleDevice(DateTime begin, DateTime end, string device)
        {
            using (DataContext context = new DataContext())
            {
                List<ItemSchedule> list = new List<ItemSchedule>();
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return new List<ItemSchedule>();
                }
                List<SqlSchedule> schedules = m_device.type!.schedules!.Where(s => s.isdeleted == false).ToList();
                if (schedules.Count > 0)
                {
                    foreach (SqlSchedule schedule in schedules)
                    {
                        int day = int.Parse(schedule.period);
                        if (DateTime.Compare(m_device.startTimeSChedule.Date.AddDays(day), begin.Date.ToUniversalTime()) > 0 && DateTime.Compare(m_device.startTimeSChedule.Date.AddDays(day), end.Date.ToUniversalTime()) <= 0)
                        {
                            ItemSchedule scheduleDevice = new ItemSchedule();
                            scheduleDevice.period = schedule.period;
                            scheduleDevice.code = schedule.code;
                            scheduleDevice.des = scheduleDevice.des;
                            scheduleDevice.note = scheduleDevice.note;
                            list.Add(scheduleDevice);
                        }
                    }
                }
                return list;
            }
        }*/

        public async Task<bool> setTimeRef (string token,string device,string point,string schedule)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device)==0).Include(s => s.points).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                SqlPoint? m_point = m_device.points!.Where(s => s.isdeleted == false && s.code.CompareTo(point) == 0).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlSchedule? m_schedule = m_device.type!.schedules!.Where(s => s.isdeleted == false && s.code.CompareTo(schedule) == 0).FirstOrDefault();
                if (m_schedule == null)
                {
                    return false;
                }
                int day = int.Parse(m_schedule.period);
                SqlLogDevice logDevice = new SqlLogDevice();
                logDevice.ID = DateTime.Now.Ticks;
                logDevice.device = m_device;
                logDevice.point = m_point;
                logDevice.user = user;
                logDevice.schedule = m_schedule;
                logDevice.note = string.Format("Set TimeRef by {0}",user.user);
                logDevice.timeRef = m_device.startTimeSChedule.AddDays(day).ToUniversalTime();

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

        public async Task<bool> setTimeDo(string token, string device, string point, string schedule)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.points).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                SqlPoint? m_point = m_device.points!.Where(s => s.isdeleted == false && s.code.CompareTo(point) == 0).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlSchedule? m_schedule = m_device.type!.schedules!.Where(s => s.isdeleted == false && s.code.CompareTo(schedule) == 0).FirstOrDefault();
                if (m_schedule == null)
                {
                    return false;
                }

                SqlLogDevice? m_log = context.logDevices!.Include(s => s.point).Include(s => s.device).Include(s => s.schedule).Where(s => s.point == m_point && s.device == m_device && s.schedule == m_schedule && s.timeRef != DateTime.MinValue).FirstOrDefault();
                if(m_log == null)
                {
                    return false;
                }
                SqlLogDevice logDevice = new SqlLogDevice();
                logDevice.ID = DateTime.Now.Ticks;
                logDevice.device = m_device;
                logDevice.point = m_point;
                logDevice.user = user;
                logDevice.schedule = m_schedule;
                logDevice.note = string.Format("Set TimeDo by {0}", user.user);
                logDevice.timeDo = DateTime.Now.ToUniversalTime();
                logDevice.timeRef = m_log.timeRef.ToUniversalTime();
                logDevice.images = m_log.images;

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

        public async Task<string> addImage(string token, string device, string point, string schedule, byte[] image)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return "";
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.points).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return "";
                }
                SqlPoint? m_point = m_device.points!.Where(s => s.isdeleted == false && s.code.CompareTo(point) == 0).FirstOrDefault();
                if (m_point == null)
                {
                    return "";
                }
                SqlSchedule? m_schedule = m_device.type!.schedules!.Where(s => s.isdeleted == false && s.code.CompareTo(schedule) == 0).FirstOrDefault();
                if (m_schedule == null)
                {
                    return "";
                }

                SqlLogDevice? m_log = context.logDevices!.Include(s => s.point).Include(s => s.device).Include(s => s.schedule).Include(s => s.user).Where(s => s.point == m_point && s.device == m_device && s.schedule == m_schedule && s.user == user && s.timeDo == DateTime.MinValue).FirstOrDefault();
                if (m_log == null)
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
                    
                    

                    SqlLogDevice logDevice = new SqlLogDevice();
                    logDevice.ID = DateTime.Now.Ticks;
                    logDevice.device = m_device;
                    logDevice.point = m_point;
                    logDevice.user = user;
                    logDevice.schedule = m_schedule;
                    logDevice.note = string.Format("Add Images: {0} by {1}",m_file, user.user);
                    logDevice.timeRef = m_log.timeRef.ToUniversalTime();
                    if (m_log.images == null)
                    {
                        m_log.images = new List<string>();
                    }
                    m_log.images.Add(m_file);
                    logDevice.images = m_log.images;

                    context.logDevices!.Add(logDevice);
                }
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
        public async Task<bool> removeImage(string token, string device, string point, string schedule, string image)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlDevice? m_device = context.devices!.Where(s => s.isdeleted == false && s.code.CompareTo(device) == 0).Include(s => s.points).Include(s => s.type).ThenInclude(s => s!.schedules).FirstOrDefault();
                if (m_device == null)
                {
                    return false;
                }
                SqlPoint? m_point = m_device.points!.Where(s => s.isdeleted == false && s.code.CompareTo(point) == 0).FirstOrDefault();
                if (m_point == null)
                {
                    return false;
                }
                SqlSchedule? m_schedule = m_device.type!.schedules!.Where(s => s.isdeleted == false && s.code.CompareTo(schedule) == 0).FirstOrDefault();
                if (m_schedule == null)
                {
                    return false;
                }

                SqlLogDevice? m_log = context.logDevices!.Include(s => s.point).Include(s => s.device).Include(s => s.schedule).Include(s => s.user).Where(s => s.point == m_point && s.device == m_device && s.schedule == m_schedule && s.user == user && s.timeDo == DateTime.MinValue).FirstOrDefault();
                if (m_log == null)
                {
                    return false;
                }
                
                if(m_log.images != null)
                {
                    m_log.images.Remove(image);
                }

                SqlLogDevice logDevice = new SqlLogDevice();
                logDevice.ID = DateTime.Now.Ticks;
                logDevice.device = m_device;
                logDevice.point = m_point;
                logDevice.user = user;
                logDevice.schedule = m_schedule;
                logDevice.note = string.Format("Remove Images: {0} by {1}", image, user.user);
                logDevice.timeRef = m_log.timeRef.ToUniversalTime();
                logDevice.images = m_log.images;

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
    }
}
