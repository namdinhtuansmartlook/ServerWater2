using Microsoft.AspNetCore.Mvc;
namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly ILogger<AreaController> _logger;

        public AreaController(ILogger<AreaController> logger)
        {
            _logger = logger;
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
                if (string.IsNullOrEmpty(area.code) || string.IsNullOrEmpty(area.name))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_area.createAreaAsync(area.code, area.name, area.des);
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
                if (string.IsNullOrEmpty(area.code) || string.IsNullOrEmpty(area.name))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_area.editAreaAsync(area.code, area.name, area.des);
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
        public async Task<IActionResult> DeleteAreaAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_area.deleteAreaAsync(code);
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
        [Route("{maDB}/addCustomer")]
        public async Task<IActionResult> AddCustomerAsync([FromHeader] string token, string area, string maDB)
        {

            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {

                if (string.IsNullOrEmpty(area) || string.IsNullOrEmpty(maDB))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_area.addCustomerAsync(area, maDB);
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
        [Route("{maDB}/removeCustomer")]
        public async Task<IActionResult> RemoveCustomerAsync([FromHeader] string token, string area, string maDB)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {

                if (string.IsNullOrEmpty(maDB) || string.IsNullOrEmpty(area))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_area.removeCustomerAsync(area, maDB);
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
        public IActionResult getListArea([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_area.getAllListArea());
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("getListFullInfoArea")]
        public IActionResult getListFullInfoArea([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_area.getAllListFullInfoArea(token));
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("{code}/getListUser")]
        public IActionResult getInfoUser([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {

                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                return Ok(Program.api_area.getListUserArea(code));

            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("{code}/getListCustomer")]
        public IActionResult getListCustomer([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {

                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                return Ok(Program.api_area.getListCustomerArea(code));

            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("{code}/getListPoint")]
        public IActionResult getListPointArea([FromHeader] string token, string code)
        {
                return Ok(Program.api_area.getListPointArea(token, code));         
        }

        [HttpGet]
        [Route("{code}/getListDevice")]
        public IActionResult getListDeviceArea([FromHeader] string token, string code)
        {
          
                return Ok(Program.api_area.getListDeviceArea(token,code));
           
        }

        [HttpGet]
        [Route("{code}/getListType")]
        public IActionResult getListTypeArea([FromHeader] string token, string code)
        {
           
                return Ok(Program.api_area.getListTypeForArea(token, code));
           
        }

        [HttpGet]
        [Route("{code}/getListSchedule")]
        public IActionResult getListScheduleArea([FromHeader] string token, string begin, string end, string code)
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
                return Ok(Program.api_area.getListScheduleForArea(token,time_begin, time_end, code));
           
        }

    }
}
