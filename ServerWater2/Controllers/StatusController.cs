using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        public class ItemHttpStatus
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
        }

        [HttpPost]
        [Route("createStatus")]
        public async Task<IActionResult> createStatus([FromHeader] string token, ItemHttpStatus status)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_status.createStatus(token,status.code,status.name);
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
        [Route("editStatus")]
        public async Task<IActionResult> editStatus([FromHeader] string token, ItemHttpStatus status)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_status.editStatus(token,status.code,status.name);
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
        [Route("{code}/deleteStatus")]
        public async Task<IActionResult> deleteStatus([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_status.deleteStatus(token,code);
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
        [Route("getListStatus")]
        public IActionResult getListStatus([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_status.getListStatus());
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
