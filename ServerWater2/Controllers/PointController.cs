using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServerWater2.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PointController : ControllerBase
    {
        private readonly ILogger<PointController> _logger;

        public PointController(ILogger<PointController> logger)
        {
            _logger = logger;
        }
        public class ItemHttpPoint
        {
            public string code { get; set; } = "";
            public string namePoint { get; set; } = "";
            public string des { get; set; } = "";
            public string longi { get; set; } = "";
            public string lati { get; set; } = "";
            public string note { get; set; } = "";

        }

        [HttpPost]
        [Route("createPoint")]
        public async Task<IActionResult> createPointAsync([FromHeader] string token, ItemHttpPoint point)
        {
            bool flag = await Program.api_point.createPoint(token, point.code, point.namePoint, point.des, point.longi, point.lati, point.note);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("editPoint")]
        public async Task<IActionResult> editPointAsync([FromHeader] string token, ItemHttpPoint point)
        {
            bool flag = await Program.api_point.editPoint(token, point.code, point.namePoint, point.des, point.longi, point.lati, point.note);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{code}/delete")]
        public async Task<IActionResult> deletePointAsync([FromHeader] string token, string code)
        {
            bool flag = await Program.api_point.deletePoint(token, code);
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
        [Route("{code}/addImage")]
        public async Task<IActionResult> addImagePoint([FromHeader] string token, string code, IFormFile image)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                image.CopyTo(ms);
                string temp = await Program.api_point.addImagePoint(token, code, ms.ToArray());
                if (!string.IsNullOrEmpty(temp))
                {
                    return Ok(temp);
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("{code}/removeImage")]
        public async Task<IActionResult> removeImagePoint([FromHeader] string token, string code, string image)
        {
            bool flag = await Program.api_point.removeImagePoint(token, code, image);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getListPoint")]
        public IActionResult getListInfoPoint([FromHeader] string token)
        {
            return Ok(JsonConvert.SerializeObject(Program.api_point.getListInfoPoint(token)));
        }

        [HttpPut]
        [Route("{code}/addArea")]
        public async Task<IActionResult> addPointArea([FromHeader] string token, string code, string area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_point.addPointArea(code, area);
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
        [Route("{code}/removeArea")]
        public async Task<IActionResult> removePointArea([FromHeader] string token, string code, string area)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_point.removePointArea(code, area);
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
        [Route("{code}/addDevice")]
        public async Task<IActionResult> addDevicePoint([FromHeader] string token, string code, string device)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_point.addDevicePoint(token,device,code);
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
        [Route("{code}/removeDevice")]
        public async Task<IActionResult> removeDevicePoint([FromHeader] string token, string code, string device)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_point.removeDevicePoint(token, device,code);
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
