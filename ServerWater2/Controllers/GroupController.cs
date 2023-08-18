using Microsoft.AspNetCore.Mvc;

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
        [Route("{code}/deleteGroup")]
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

        [HttpPut]
        [Route("{code}/addArea")]
        public async Task<IActionResult> AddArea([FromHeader] string token, string code, string area)
        {
            long ID = Program.api_user.checkAdmin(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_group.addArea(code, area);
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

        [HttpPut]
        [Route("{code}/removeArea")]
        public async Task<IActionResult> RemoveArea([FromHeader] string token, string code, string area)
        {
            long ID = Program.api_user.checkAdmin(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_group.RemoveArea(code, area);
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
        public IActionResult GetListGroup()
        {
            return Ok(Program.api_group.getListGroup());
        }

        [HttpGet]
        [Route("{group}/getListArea")]
        public IActionResult GetListAreaForGroup(string group)
        {
            return Ok(Program.api_group.getListArea(group));
        }
        public class ItemHttpArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        [HttpPost]
        [Route("createArea")]
        public async Task<IActionResult> CreateAreaAsync([FromHeader] string token, ItemHttpArea area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_area.createAsync(area.code, area.name, area.des);
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
                bool flag = await Program.api_area.editAsync(area.code, area.name, area.des);
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
        [Route("{code}/deleteArea")]
        public async Task<IActionResult> DeleteAreaAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_area.deleteAsync(code);
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
        [Route("getListArea")]
        public IActionResult GetListArea()
        {
            return Ok(Program.api_area.getListArea());
        }
    }
}
