using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
       

        [HttpGet]
        [Route("getListService")]
        public IActionResult getListService([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_service.getListService());
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
