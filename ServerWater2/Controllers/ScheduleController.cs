using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }


        public class ItemHttpShedule
        {
            public string code { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
            public string period { get; set; } = "";
        }

        [HttpPost]
        [Route("createShedule")]
        public async Task<IActionResult> createShedule([FromHeader] string token, ItemHttpShedule shedule)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_schedule.createSchedule(shedule.code, shedule.des, shedule.note, shedule.period);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("editShedule")]
        public async Task<IActionResult> editShedule([FromHeader] string token, ItemHttpShedule shedule)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_schedule.editSchedule(shedule.code, shedule.des, shedule.note, shedule.period);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpDelete]
        [Route("{code}/deleteShedule")]
        public async Task<IActionResult> deleteShedule([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_schedule.deleteSchedule(code);
                if (flag)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpGet]
        [Route("getListSchedule")]
        public IActionResult getAllListSchedule([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_schedule.getListSchedule());
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPut]
        [Route("setTimeRef")]
        public async Task<IActionResult> setTimeRef([FromHeader] string token, string deivce, string point, string schedule)
        {

            bool flag = await Program.api_schedule.setTimeRef(token, deivce, point, schedule);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("addImageLog")]
        public async Task<IActionResult> addImageLogAsync([FromHeader] string token, string device, string point, string schedule, IFormFile image)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                image.CopyTo(ms);
                string code = await Program.api_schedule.addImage(token, device, point, schedule, ms.ToArray());
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(code);
                }
            }

        }


        [HttpDelete]
        [Route("removeImageLog")]
        public async Task<IActionResult> removeImageLog([FromHeader] string token, string device, string point, string schedule, string image)
        {

            bool flag = await Program.api_schedule.removeImage(token, device, point, schedule, image);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [Route("setTimeDo")]
        public async Task<IActionResult> setTimeDo([FromHeader] string token, string device, string point, string schedule)
        {
            bool flag = await Program.api_schedule.setTimeDo(token, device, point, schedule);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }



    }
}
