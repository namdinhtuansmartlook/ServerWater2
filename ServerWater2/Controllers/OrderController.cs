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
           

            string order = await Program.api_order.createOrderAsync(item.customer, item.phone, item.addressCustomer, item.addressWater, item.addressContract, item.service, item.type,item.note);
            if (string.IsNullOrEmpty(order))
            {
                return BadRequest();
            }
            else
            {
                return Ok(order);
            }
        }
    }
}
