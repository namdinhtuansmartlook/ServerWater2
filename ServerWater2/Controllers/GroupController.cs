using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;

        public GroupController(ILogger<GroupController> logger)
        {
            _logger = logger;
        }

        
        public class ItemHttpGroup
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        [HttpPost]
        [Route("createGroup")]
        public async Task<IActionResult> CreateGroupAsync([FromHeader] string token, ItemHttpGroup group)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_group.createAsync(group.code, group.name, group.des);
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
        [Route("editGroup")]
        public async Task<IActionResult> EditGroupAsync([FromHeader] string token, ItemHttpGroup group)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_group.editAsync(group.code, group.name, group.des);
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
        [Route("{code}/delete")]
        public async Task<IActionResult> DeleteGroupAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_group.deleteAsync(code);
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
        [Route("getListGroup")]
        public IActionResult GetListGroup([FromHeader] string token)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                return Ok(Program.api_group.getListGroup());
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{code}/getListArea")]
        public IActionResult GetListArea([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                return Ok(Program.api_group.getListArea(code));
            }
            else
            {
                return Unauthorized();
            }
        }

        public class ItemHttpArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public string group { get; set; } = "";
        }

        [HttpPost]
        [Route("createArea")]
        public async Task<IActionResult> CreateAreaAsync([FromHeader] string token, ItemHttpArea area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_area.createAsync(area.code, area.name, area.des, area.group);
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
        [Route("editArea")]
        public async Task<IActionResult> EditAreaAsync([FromHeader] string token, ItemHttpArea area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_area.editAsync(area.code, area.name, area.des, area.group);
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
        [Route("{code}/delete/{area}")]
        public async Task<IActionResult> DeleteAreaAsync([FromHeader] string token, string code, string area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_area.deleteAsync(area, code);
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
    }
}
