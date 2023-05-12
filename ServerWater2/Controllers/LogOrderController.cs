using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogOrderController : ControllerBase
    {
        private readonly ILogger<LogOrderController> _logger;

        public LogOrderController(ILogger<LogOrderController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //[Route("getListHistoryOrder")]
        //public IActionResult GetListNewOrder([FromHeader] string token, string begin, string end)
        //{
        //    DateTime time_begin = DateTime.MinValue;
        //    try
        //    {
        //        time_begin = DateTime.ParseExact(begin, "dd-MM-yyyy", null);
        //    }
        //    catch (Exception e)
        //    {
        //        time_begin = DateTime.MinValue;
        //    }
        //    DateTime time_end = DateTime.MinValue;
        //    try
        //    {
        //        time_end = DateTime.ParseExact(end, "dd-MM-yyyy", null);
        //    }
        //    catch (Exception e)
        //    {
        //        time_end = DateTime.MinValue;
        //    }
        //    long id = Program.api_user.checkSystem(token);
        //    if (id >= 0)
        //    {
        //        return Ok(Program.api_log.getListLog(token, time_begin, time_end));
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
           
        //}
    }
}
