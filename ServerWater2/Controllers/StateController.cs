using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static ServerWater2.APIs.MyState;
using static ServerWater2.Controllers.TypeController;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        [HttpGet]
        [Route("getListState")]
        public IActionResult GetListState([FromHeader] string token)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                return Ok(Program.api_state.getList());

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("editState")]
        public async Task<IActionResult> EditTypeAsync([FromHeader] string token, ItemStateOrder state)
        {
            long id = Program.api_user.checkCS(token);
            if (id >= 0)
            {
                bool flag = await Program.api_state.editAsync(state.code, state.name, state.des);
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
