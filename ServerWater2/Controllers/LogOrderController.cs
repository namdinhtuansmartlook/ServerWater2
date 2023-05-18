﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("{code}/getListHistoryForUser")]
        public IActionResult getListHistoryForUser([FromHeader] string token, string begin, string end, string code)
        {
            DateTime time_begin = DateTime.MinValue;
            try
            {
                time_begin = DateTime.ParseExact(begin, "dd-MM-yyyy", null);
            }
            catch (Exception e)
            {
                time_begin = DateTime.MinValue;
            }
            DateTime time_end = DateTime.MaxValue;
            try
            {
                time_end = DateTime.ParseExact(end, "dd-MM-yyyy", null);
            }
            catch (Exception e)
            {
                time_end = DateTime.MaxValue;
            }
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                return Ok(Program.api_log.getListHistoryOrderForUser(token, time_begin, time_end, code));
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("{user}/getListHistoryForAdmin")]
        public IActionResult getListHistoryForAdmin([FromHeader] string token, string begin, string end, string user)
        {
            DateTime time_begin = DateTime.MinValue;
            try
            {
                time_begin = DateTime.ParseExact(begin, "dd-MM-yyyy", null);
            }
            catch (Exception e)
            {
                time_begin = DateTime.MinValue;
            }
            DateTime time_end = DateTime.MaxValue;
            try
            {
                time_end = DateTime.ParseExact(end, "dd-MM-yyyy", null);
            }
            catch (Exception e)
            {
                time_end = DateTime.MaxValue;
            }
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_log.getListLogOrderForAdmin(token, time_begin, time_end, user));
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
