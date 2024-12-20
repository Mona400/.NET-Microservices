using AutoMapper;
using CommandsService.Dto;
using CommandsService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommand _command;
        private readonly IMapper _mapper;
        public PlatformsController(ICommand command, IMapper mapper)
        {
            _command = command;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult <IEnumerable<PlatformReadDto>>GetAllPlatform()
        {
            var platforms= _command.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--->inbound post #command service");
            return Ok("inbound test from platform controller");
        }
    }
}
