using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

                if(state.CompareTo("0") == 0)
                {
                    long ID = Program.api_user.checkAdmin(token);

                    if (ID < 0)
                    {
                        return;
                    }
                }    
                else if(state.CompareTo("1") == 0)
                {
                    long ID = Program.api_user.checkSystem(token);

                    if (ID < 0)
                    {
                        return;
                    }
                }   
                else
                {
                    long ID = Program.api_user.checkUser(token);

                    if (ID < 0)
                    {
                        return;
                    }
                }                   

                string id = DateTime.Now.Ticks.ToString();
                Program.HttpNotification httpNotification = new Program.HttpNotification();
                httpNotification.id = id;
                httpNotification.state = state;
               // httpNotification.isRequest = false;
                Program.httpNotifications.Add(httpNotification);

                while (true)
                {
                    await Task.Delay(1000);
                    Program.HttpNotification? notification = Program.httpNotifications.Where(s => s.id.CompareTo(id) == 0).FirstOrDefault();
                    if (notification == null)
                    {
                        break;
                    }
                    notification.datas = Program.dataNotifications.Where(s => s.state.CompareTo(notification.state) == 0 && s.isRequest == false).ToList();

                    
                    if (notification.datas.Count == 0)
                    {
                        string msg = string.Format("data: {0}\r\r", DateTime.Now);
                        await Response.WriteAsync(msg);
                        await Response.Body.FlushAsync();
                    }
                    foreach (DataNotification s in notification.datas)
                    {
                        if(s.messagers.Count > 0)
                        {
                            for (int i = 0; i < s.messagers.Count; i++)
                            {
                                Log.Information(s.state +":" + s.messagers[i]);
                                await Response.WriteAsync(string.Format("data: {0}\r\r", s.messagers[i]));
                                await Response.Body.FlushAsync();
                                await Task.Delay(100);
                                s.messagers.RemoveAt(0);
                                i--;
                                s.isRequest = true;
                            }
                          
                        }
                        else
                        {
                            string msg = string.Format("data: {0}\r\r", DateTime.Now);
                            await Response.WriteAsync(msg);
                            await Response.Body.FlushAsync();
                        }
                       
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
