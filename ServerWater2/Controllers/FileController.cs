using Microsoft.AspNetCore.Mvc;
using static ServerWater2.APIs.MyViewForm;
using static ServerWater2.Controllers.ActionController;

namespace ServerWater2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("image/{code}")]
        public IActionResult GetImage(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest();
            }
            byte[]? data = Program.api_file.readFile(code);
            if (data == null)
            {
                return BadRequest(code);
            }
            return File(data!, "image/jpeg");
        }

        public class ItemHttpForm
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string type { get; set; } = "";
            public List<ItemJson> datas { get; set; } = new List<ItemJson>();
        }

        [HttpGet]
        [Route("getListForm")]
        public IActionResult GetListForm([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_viewform.getList());
            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpPost]
        [Route("createForm")]
        public async Task<IActionResult> CreateActionAsync([FromHeader] string token, ItemHttpForm form)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_viewform.createFormAsync(form.code, form.name, form.type, form.datas);
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
        [Route("editForm")]
        public async Task<IActionResult> EditFormAsync([FromHeader] string token, ItemHttpForm form)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_viewform.editFormAsync(form.code, form.name, form.datas);
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
        [Route("{code}/deleteForm")]
        public async Task<IActionResult> DeleteFormAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_viewform.deleteFormAsync(code);
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
        [Route("{form}/addFieldForm")]
        public async Task<IActionResult> AddFieldFormAsync([FromHeader] string token, string form, ItemJson data)
        {
            long ID = Program.api_user.checkAdmin(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_viewform.addFieldForm(form, data.key, data.field, data.label);
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
        [Route("{form}/removeFieldForm")]
        public async Task<IActionResult> RemoveFieldFormAync([FromHeader] string token, string form, string key, string field)
        {
            long ID = Program.api_user.checkAdmin(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_viewform.removeFieldForm(form, key, field);
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

        
        //[HttpPost]
        //[Route("{order}/testCreateSurveyOrder")]
        //public async Task<IActionResult> CreateSurveyOrderAsync([FromHeader] string token, string order, string form, string data)
        //{
        //    long id = Program.api_user.checkSystem(token);
        //    if (id >= 0)
        //    {
        //        bool flag = await Program.api_order.createSurveyOrderAsync(token, order, form, data);
        //        if (flag)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }

        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }

        //}

    }
}
