using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LayerController : ControllerBase
    {
        private readonly ILogger<LayerController> _logger;

        public LayerController(ILogger<LayerController> logger)
        {
            _logger = logger;
        }

        public class ItemHttpLayer
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        [HttpPost]
        [Route("createLayer")]
        public async Task<IActionResult> CreateLayerAsync([FromHeader] string token, ItemHttpLayer layer)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                if (string.IsNullOrEmpty(layer.code) || string.IsNullOrEmpty(layer.name))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_layer.createLayerAsync(layer.code, layer.name, layer.des);
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
        [Route("editLayer")]
        public async Task<IActionResult> EditLayerAsync([FromHeader] string token, ItemHttpLayer layer)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                if (string.IsNullOrEmpty(layer.code) || string.IsNullOrEmpty(layer.name))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_layer.editLayerAsync(layer.code, layer.name, layer.des);
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
        [Route("{code}/deleteLayer")]
        public async Task<IActionResult> DeleteAreaAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                bool flag = await Program.api_layer.deleteLayerAsync(code);
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
        [Route("getListLayer")]
        public IActionResult getListLayer([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_layer.getListLayer());
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
