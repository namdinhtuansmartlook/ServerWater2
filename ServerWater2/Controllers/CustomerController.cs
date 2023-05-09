using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ServerWater2.APIs.MyCustomer;

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

        [HttpPut]
        [Route("editCustomer")]
        public async Task<IActionResult> editCustomerAsync([FromHeader] string token, ItemCustomer customer)
        {

            long id = Program.api_user.checkAdmin(token);
            if(id >= 0)
            {
                bool flag = await Program.api_customer.editCustomerAsync(customer.idkh,customer.danhbo, customer.sdt, customer.tenkh, customer.diachiTT, customer.diachiLH, customer.diachiLD, customer.latidude, customer.longitude);
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
        [Route("{idkh}/deleteCustomer")]
        public async Task<IActionResult> deleteCustomerAsync([FromHeader] string token, string idkh)
        {
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                bool flag = await Program.api_customer.deleteCustomerAsync(idkh);
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
            long id = Program.api_user.checkAdmin(token);
            if (id >= 0)
            {
                return Ok(Program.api_customer.listCustomer());
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
