using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
                
        }
        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--->inbound post #command service");
            return Ok("inbound test from platform controller");
        }
    }
}
