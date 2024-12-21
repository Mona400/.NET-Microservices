using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataService;
using PlatformService.Dtos;
using PlatformService.Interfaces;
using PlatformService.Models;
using PlatformService.Services;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatform _platform;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(IPlatform platform, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _platform = platform;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
        {
            var platformItem = _platform.GetAllPlatform();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }
        [HttpGet("GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platform.GetPlatformById(id);
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }
        [HttpPost("CreatePlatform")]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
        {
            if (platformCreateDto == null)
            {
                return BadRequest("Platform data is null");
            }

            //send sync messaage
            var platformEntity = _mapper.Map<Platform>(platformCreateDto);
            var createdPlatform = _platform.CreatePlateform(platformEntity);
            var platformReadDto = _mapper.Map<PlatformReadDto>(createdPlatform);
            try
            {
                await _commandDataClient.SendPlatformCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"could not send message synchronously{ex.Message}");
            }
            //send async message
            try
            {
                var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
                platformPublishDto.Event = "Platform_Publish";
                _messageBusClient.PublishNewPlatform(platformPublishDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"could not send message asynchronously{ex.Message}");
            }
            return Ok(_mapper.Map<PlatformReadDto>(platformReadDto));
        }
        // PUT: api/platforms/{id}
        [HttpPut("{id}")]
        public ActionResult<PlatformReadDto> UpdatePlatform(int id, [FromForm] PlatformUpdateDto platformUpdateDto)
        {
            if (platformUpdateDto == null)
            {
                return BadRequest("Platform data is null");
            }

            // Find the platform by id
            var platformFromDb = _platform.GetPlatformById(id);

            if (platformFromDb == null)
            {
                return NotFound($"Platform with id {id} not found.");
            }
            _mapper.Map(platformUpdateDto, platformFromDb);
            _platform.UpdatePlatform(platformFromDb, id);
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformFromDb);
            return Ok(platformReadDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlatform(int id)
        {

            var platformFromDb = _platform.GetPlatformById(id);

            if (platformFromDb == null)
            {
                return NotFound($"Platform with id {id} not found.");
            }
            _platform.DeletePlatform(id);

            return NoContent(); // 204 No Content status code for successful deletion
        }

    }
}
