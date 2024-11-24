using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Interfaces;
using PlatformService.Models;
using PlatformService.Services;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatform _platform;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatform platform, IMapper mapper)
        {
            _platform = platform;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult <IEnumerable<PlatformReadDto>>GetAll()
        {
         var platformItem= _platform.GetAllPlatform();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }
        [HttpGet("GetPlatformById")]
        public ActionResult <PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platform.GetPlatformById(id);
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }
		[HttpPost("CreatePlatform")]
		public ActionResult<PlatformReadDto> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
		{
			if (platformCreateDto == null)
			{
				return BadRequest("Platform data is null");
			}

			var platformEntity = _mapper.Map<Platform>(platformCreateDto);
			var createdPlatform = _platform.CreatePlateform(platformEntity);
			var platformReadDto = _mapper.Map<PlatformReadDto>(createdPlatform);
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
			_platform.UpdatePlatform(platformFromDb,id);
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
