using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public class ItemLoginUser
        {
            public string username { get; set; } = "";
            public string password { get; set; } = "";
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(ItemLoginUser item)
        {
            return Ok(Program.api_user.login(item.username, item.password));
        }

        public class ItemUser
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string password { get; set; } = "";
            public string des { get; set; } = "";
            public string role { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }

        public class ItemUserV2
        {
            public string user { get; set; } = "";
            public string password { get; set; } = "";
            public string des { get; set; } = "";
            public string role { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
        }

        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUserAsync([FromHeader] string token, ItemUser user)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.createUserAsync(token, user.user, user.username, user.password, user.displayName, user.numberPhone, user.des, user.role);
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
        [Route("editUser")]
        public async Task<IActionResult> editUserAsync([FromHeader] string token, ItemUserV2 user)
        {

            bool flag = await Program.api_user.editUserAsync(token, user.user, user.password, user.displayName, user.numberPhone, user.des, user.role);
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
        [Route("{code}/deleteUser")]
        public async Task<IActionResult> deleteUserAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.deleteUserAsync(token, code);
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
        [Route("setAvatar")]
        public async Task<IActionResult> setAvatarAsync([FromHeader] string token, IFormFile image)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                image.CopyTo(ms);
                string code = await Program.api_user.setAvatar(token, ms.ToArray());
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(code);
                }
            }

        }

        [HttpGet]
        [Route("getListUser")]
        public IActionResult GetListUser([FromHeader] string token)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_user.listUser(token));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("getInfoUser")]
        public IActionResult getInfoUser([FromHeader] string token)
        {
            return Ok(Program.api_user.getInfoUser(token));
        }
    }
}
