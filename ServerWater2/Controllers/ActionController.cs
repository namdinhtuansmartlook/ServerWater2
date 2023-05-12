using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;

        public ActionController(ILogger<ActionController> logger)
        {
            _logger = logger;
        }

        public class ItemHttpAction
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        [HttpPost]
        [Route("createAction")]
        public async Task<IActionResult> CreateActionAsync([FromHeader] string token, ItemHttpAction action)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                bool flag = await Program.api_action.createAsync(action.code, action.name, action.des);
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
        [Route("editAction")]
        public async Task<IActionResult> EditActionAsync([FromHeader] string token, ItemHttpAction action)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                bool flag = await Program.api_action.editAsync(action.code, action.name, action.des);
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
        [Route("{code}/deleteAction")]
        public async Task<IActionResult> DeleteActionAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                bool flag = await Program.api_action.deleteAsync(code);
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
        [Route("getListAction")]
        public IActionResult getListAction([FromHeader] string token)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                return Ok(Program.api_action.getListAction());
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
