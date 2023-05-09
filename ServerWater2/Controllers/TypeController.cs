using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TypeController> _logger;

        public TypeController(ILogger<TypeController> logger)
        {
            _logger = logger;
        }

        public class ItemHttpType
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        [HttpPost]
        [Route("createType")]
        public async Task<IActionResult> CreateTypeAsync([FromHeader] string token, ItemHttpType type)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_type.createAsync(type.code, type.name, type.des);
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
        [Route("editType")]
        public async Task<IActionResult> EditTypeAsync([FromHeader] string token, ItemHttpType service)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_type.editAsync(service.code, service.name, service.des);
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
        [Route("{code}/deleteType")]
        public async Task<IActionResult> DeleteTypeAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_type.deleteAsync(code);
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
        [Route("getListType")]
        public IActionResult getListType([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_type.getListType());
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
