using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ServerWater2.APIs.MyCustomer;
using static ServerWater2.APIs.MyLogOrder;
using static ServerWater2.APIs.MyOrder;

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

        [HttpPut]
        [Route("setActionOrder")]
        public async Task<IActionResult> SetAction([FromHeader] string token, string id, string action)
        {
            bool flag = await Program.api_order.setAction(token, id, action);
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
        [Route("{code}/confirmOrder")]
        public async Task<IActionResult> ConfirmOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
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
            else
            {
                return Unauthorized();
            }

        }


        [HttpPut]
        [Route("{code}/setCustomer")]
        public async Task<IActionResult> SetCustomer([FromHeader] string token, string maDB, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
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
            else
            {
                return Unauthorized();
            }
           
        }
        
       
        [HttpPut]
        [Route("{code}/setConfirmedOrder")]
        public async Task<IActionResult> SetConfirmedOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.setConfirmedOrder(token, code);
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
        [Route("{code}/setAssginOrder")]
        public async Task<IActionResult> SetAssginOrder([FromHeader] string token, string code, string user)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.setAssginOrder(token, code, user);
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
        [Route("{code}/beginWorkOrder")]
        public async Task<IActionResult> BeginWorkOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.beginWorkOrder(token, code);
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
        [Route("{code}/finishWorkOrder")]
        public async Task<IActionResult> FinishWorkOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.finishWorkOrder(token, code);
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
        [Route("{code}/finishOrder")]
        public async Task<IActionResult> FinishOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkUser(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.finishOrder(token, code);
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
        [Route("{code}/cancelOrder")]
        public async Task<IActionResult> CancelOrder([FromHeader] string token, string code)
        {
            long id = Program.api_user.checkSystem(token);
            if (id >= 0)
            {
                bool flag = await Program.api_order.cancelOrder(token, code);
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

        [HttpGet]
        [Route("getFindOrder")]
        public IActionResult GetFindOrder(string code)
        {
            return Ok(Program.api_order.getFindOrder(code));
        }

        [HttpGet]
        [Route("getInfoCustomer")]
        public IActionResult GetInfoCustomer(string code)
        {
            ItemCustomer info = Program.api_order.getInfoCustmer(code);
            return Ok(info);
        }


    }
}
