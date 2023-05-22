using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            public int state { get; set; } = 0;
        }
        [HttpGet]
        [Route("{token}/callnotification")]
        public async Task callbackfaceAsync([FromRoute] string token, string state)
        {
            CancellationToken cancellationToken = HttpContext.RequestAborted;
            try
            {
                Response.Headers.Add("Content-Type", "text/event-stream");
                Response.Headers.Add("Cache-Control", "no-cache");
                Response.Headers.Add("Connection", "keep-alive");

                long ID = Program.api_user.checkUser(token);

                if (ID < 0)
                {
                    return;
                }

                string id = DateTime.Now.Ticks.ToString();
                Program.HttpNotification httpNotification = new Program.HttpNotification();
                httpNotification.id = id;
                httpNotification.state = state;
                httpNotification.messagers = new List<string>();
                Program.httpNotifications.Add(httpNotification);

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
                        Program.httpNotifications.Remove(notification);
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

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> test(ItemTest item)
        {


            bool order = await Program.api_order.testOrder(item.user, item.code, item.state);
            if (!order)
            {
                return BadRequest();
            }
            else
            {
                return Ok(order);
            }
        }
    }
}
