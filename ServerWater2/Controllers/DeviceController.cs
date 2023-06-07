using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(ILogger<DeviceController> logger)
        {
            _logger = logger;
        }

        public class ItemHttpDevice
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string codeType { get; set; } = "";
        }

        [HttpPost]
        [Route("createDevice")]
        public async Task<IActionResult> CreateDeviceAsync([FromHeader] string token, ItemHttpDevice device)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_device.createDevice(device.code, device.name, device.codeType);
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
        [Route("editDevice")]
        public async Task<IActionResult> EditDeviceAsync([FromHeader] string token, ItemHttpDevice device)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_device.editDevice(device.code, device.name, device.codeType);
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
        [Route("{code}/deleteDevice")]
        public async Task<IActionResult> DeleteDeviceAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_device.deleteDevice(code);
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
        [Route("{code}/addLayer")]
        public async Task<IActionResult> addLayerDevice([FromHeader] string token, string code, string device)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_device.addLayerDevice(code, device);
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
        [Route("{code}/removeLayer")]
        public async Task<IActionResult> removeValueType([FromHeader] string token, string code, string device)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_device.removeLayerDevice(code, device);
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
        [Route("getListDevice")]
        public IActionResult getListDevice([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(JsonConvert.SerializeObject(Program.api_device.getListDevice()));
            }
            else
            {
                return Unauthorized();
            }
            
        }

        [HttpPost]
        [Route("getValuesForDevices")]
        public IActionResult getListValueDevice([FromHeader] string token, List<string> devices)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_device.getListValueDevice(devices));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{code}/setTimeScheduleDevice")]
        public async Task<IActionResult> setTimeScheduleDevice([FromHeader] string token, string code, string time)
        {
            DateTime m_time = DateTime.MinValue;
            try
            {
                m_time = DateTime.ParseExact(time, "dd-MM-yyyy HH:mm:ss", null);
            }
            catch (Exception e)
            {
                m_time = DateTime.MinValue; 
            }
            bool flag = await Program.api_device.setTimeScheduleDevice(token, code, m_time);
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
