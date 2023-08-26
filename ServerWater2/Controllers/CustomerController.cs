using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        public class ItemCustomer
        {
            public string maDB { get; set; } = "";
            public string sdt { get; set; } = "";
            public string tenkh { get; set; } = "";
            public string diachi { get; set; } = "";
            public string note { get; set; } = "";
            public string x { get; set; } = "";
            public string y { get; set; } = "";
        }
        
        [HttpPost]
        [Route("createCustomer")]
        public async Task<IActionResult> createCustomerAsync([FromHeader] string token, ItemCustomer customer)
        {

            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_customer.createCustomerAsync(customer.maDB, customer.sdt, customer.tenkh, customer.diachi,customer.note, customer.x, customer.y);
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
        [Route("editCustomer")]
        public async Task<IActionResult> editCustomerAsync([FromHeader] string token, ItemCustomer customer)
        {

            long id = Program.api_user.checkSystem(token);
            if(id >= 0)
            {
                bool flag = await Program.api_customer.editCustomerAsync(customer.maDB, customer.sdt, customer.tenkh, customer.diachi,customer.note,customer.x, customer.y);
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
        [Route("{maDB}/deleteCustomer")]
        public async Task<IActionResult> deleteCustomerAsync([FromHeader] string token, string maDB)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_customer.deleteCustomerAsync(maDB);
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
        [Route("{maDB}/addImageCustomer")]
        public async Task<IActionResult> addImageCustomer([FromHeader] string token, string maDB, IFormFile image)
        {
            long id = Program.api_user.checkAdmin(token);
            if(id >= 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    string temp = await Program.api_customer.addImageCustomer(token, maDB, ms.ToArray());
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
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{maDB}/removeImageCustomer")]
        public async Task<IActionResult> removeImageCustomer([FromHeader] string token, string maDB, string image)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_customer.removeImageCustomer(token, maDB, image);
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
        [Route("getListCustomer")]
        public IActionResult GetListCustomer([FromHeader] string token)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                return Ok(Program.api_customer.listCustomer());
            }
            else
            {
                return Unauthorized();
            }
        }

        public class ItemHttpCertificate
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";

        }

        [HttpGet]
        [Route("getListCertificate")]
        public IActionResult GetListCertificate()
        {
            return Ok(Program.api_certificate.getList());
        }

        [HttpPost]
        [Route("createCertificate")]
        public async Task<IActionResult> CreateCertificateAsync([FromHeader] string token, ItemHttpCertificate item)
        {

            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_certificate.createAsync(item.code, item.name, item.des);
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
        [Route("editCertificate")]
        public async Task<IActionResult> EditCertificateAsync([FromHeader] string token, ItemHttpCertificate item)
        {

            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_certificate.editAsync(item.code, item.name, item.des);
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
        [Route("{code}/deleteCertificate")]
        public async Task<IActionResult> DeleteCertificateAsync([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_certificate.deleteAsync(code);
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
