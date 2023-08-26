using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using static ServerWater2.Program;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public class ItemTest
        {
            public string user { get; set; } = "";
            public string code { get; set; } = "";
            public string action { get; set; } = "";
        }

        [HttpGet]
        [Route("{token}/callnotification")]
        public async Task callbackfaceAsync([FromRoute] string token)
        {
            CancellationToken cancellationToken = HttpContext.RequestAborted;
            try
            {
                Response.Headers.Add("Content-Type", "text/event-stream");
                Response.Headers.Add("Cache-Control", "no-cache");
                Response.Headers.Add("Connection", "keep-alive");

                string tmp = Program.api_user.checkUserNotification(token);
                if (string.IsNullOrEmpty(tmp))
                {
                    return;
                }

                string id = DateTime.Now.Ticks.ToString();
                Program.HttpNotification httpNotification = new Program.HttpNotification();
                httpNotification.id = id;
                httpNotification.token = token;
                httpNotification.messagers = new List<string>();
                httpNotification.person = tmp;
                Program.httpNotifications.Add(httpNotification);

                if (!httpNotification.isOffline)
                {
                    bool flag = await Program.api_user.checkNotification(token, !httpNotification.isOffline);
                    if (flag)
                    {
                        await Task.Delay(100);
                    }
                }

                while (true)
                {
                    await Task.Delay(1000);
                    
                    Program.HttpNotification? notification = Program.httpNotifications.Where(s => s.id.CompareTo(id) == 0).FirstOrDefault();
                    if (notification == null)
                    {
                        break;
                    }
                    List<string> list = notification.messagers.ToList();
                    notification.messagers.Clear();

                    if (list.Count == 0)
                    {
                        string msg = string.Format("data: {0}\r\r", DateTime.Now);
                        await Response.WriteAsync(msg);
                        await Response.Body.FlushAsync();
                    }
                    foreach (string s in list)
                    {
                        Log.Information(s);
                        await Response.WriteAsync(string.Format("data: {0}\r\r", s));
                        await Response.Body.FlushAsync();
                        await Task.Delay(100);
                    }
                    
                    if (cancellationToken.IsCancellationRequested)
                    {
                        bool flag = await Program.api_user.checkNotification(token, notification.isOffline);
                        if (flag)
                        {
                            Program.httpNotifications.Remove(notification);
                        }
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //[HttpPost]
        //[Route("test")]
        //public async Task<IActionResult> test(ItemTest item)
        //{


        //    bool order = await Program.api_order.testNotificationOrder(item.user, item.code, item.action);
        //    if (!order)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return Ok(order);
        //    }
        //}
    }
}
