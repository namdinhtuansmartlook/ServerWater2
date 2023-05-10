using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWater2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }
        public class ItemOrder
        {
            public string customer { get; set; } = "";
            public string phone { get; set; } = "";
            public string addressCustomer { get; set; } = "";
            public string addressWater { get; set; } = "";
            public string addressContract { get; set; } = "";
            public string service { get; set; } = "";
            public string type { get; set; } = "";
            public string note { get; set; } = "";

        }
        [HttpPost]
        [Route("createOrder")]
        public async Task<IActionResult> CreateOrderAsync(ItemOrder item)
        {
           

            string order = await Program.api_order.createNewOrder(item.customer, item.phone, item.addressCustomer, item.addressWater, item.addressContract, item.service, item.type,item.note);
            if (string.IsNullOrEmpty(order))
            {
                return BadRequest();
            }
            else
            {
                return Ok(order);
            }
        }

        public class ItemHttpRequest
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string customer { get; set; } = "";
            public string phone { get; set; } = "";
            public string address { get; set; } = "";
            public string addressContract { get; set; } = "";
            public string type { get; set; } = "";
            public string note { get; set; } = "";
        }

        [HttpPost]
        [Route("createRequestOrder")]
        public async Task<IActionResult> CreateRequestOrderAsync(ItemHttpRequest item)
        {


            string order = await Program.api_order.createRequestOrder(item.code, item.name, item.phone, item.customer, item.address, item.addressContract, item.type, item.note);
            if (string.IsNullOrEmpty(order))
            {
                return BadRequest();
            }
            else
            {
                return Ok(order);
            }
        }

        [HttpGet]
        [Route("getListService")]
        public IActionResult GetListService()
        {
            return Ok(Program.api_service.getListService());
        }

        [HttpGet]
        [Route("getListOrder")]
        public IActionResult GetListNewOrder([FromHeader] string token)
        {
            return Ok(Program.api_order.getListOrder(token));
        }

        [HttpPut]
        [Route("{code}/confirmOrder")]
        public async Task<IActionResult> ReceiveOrder([FromHeader] string token, string code)
        {
            bool flag = await Program.api_order.confirmOrder(token, code);
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
        [Route("{code}/setCustomer")]
        public async Task<IActionResult> SetCustomer([FromHeader] string token, string maDB, string code)
        {
            bool flag = await Program.api_order.setCustomer(token, maDB, code);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

       /* [HttpPut]
        [Route("{code}/receiveOrderManager")]
        public async Task<IActionResult> recieveOrderByManager([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkManager(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.managerReceiveOrder(token, code);
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
        [Route("{code}/setWorker")]
        public async Task<IActionResult> setWorkerOrder([FromHeader] string token, string code, string user)
        {
            long id = Program.api_user.checkManager(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.setWorkerOrder(token, code, user);
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

        }*/
        
    }
}
