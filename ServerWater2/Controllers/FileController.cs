using Microsoft.AspNetCore.Mvc;

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
    }
}
