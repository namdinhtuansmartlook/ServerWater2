using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static ServerWater2.Controllers.ActionController;

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
            public string group { get; set; } = "";
        }

        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUserAsync([FromHeader] string token, ItemUser user)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.createUserAsync(token, user.user, user.username, user.password, user.displayName, user.numberPhone, user.des, user.role, user.group);
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
        public async Task<IActionResult> editUserAsync([FromHeader] string token, ItemUser user)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.editUserAsync(token, user.user, user.password, user.displayName, user.numberPhone, user.des, user.role, user.group);
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
        [Route("{group}/delete/{code}")]
        public async Task<IActionResult> deleteUserAsync([FromHeader] string token, string code, string group)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_user.deleteUserAsync(token, code, group);
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
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword([FromHeader] string token, string oldPassword, string newPassword)
        {
            long ID = Program.api_user.checkUser(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_user.changePassword(token, oldPassword, newPassword);
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
        [Route("changeGroup")]
        public async Task<IActionResult> ChangeGroup([FromHeader] string token, string group)
        {
            long ID = Program.api_user.checkSystem(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_user.changeGroup(token, group);
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
        [Route("setListUserInGroup")]
        public async Task<IActionResult> SetGroup([FromHeader] string token, string group, List<string> users)
        {
            long ID = Program.api_user.checkSystem(token);
            if (ID >= 0)
            {
                bool flag = await Program.api_user.setGroup(token, group, users);
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

        public class ItemHttpRole
        {
            public string name { get; set; } = "";
            public string des { get; set; } = "";
            public string note { get; set; } = "";
        }

        [HttpGet]
        [Route("getListRole")]
        public IActionResult getListRole()
        {
            return Ok(Program.api_role.getListRole());
        }

        [HttpPost]
        [Route("editRole")]
        public async Task<IActionResult> EditRoleAsync([FromHeader] string token, ItemHttpRole role)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_action.editAsync(role.name, role.des, role.note);
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
        [Route("getListUser")]
        public IActionResult GetListUser([FromHeader] string token)
        {
            long id = Program.api_user.checkSystem(token);
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
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                return Ok(Program.api_user.getInfoUser(token));
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
