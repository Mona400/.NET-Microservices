using AutoMapper;
using CommandsService.Dto;
using CommandsService.Interfaces;
using CommandsService.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommand _command;
        private readonly IMapper _mapper;

        public CommandsController(ICommand command, IMapper mapper)
        {
            _command = command;
            _mapper = mapper;
        }
        [HttpGet("GetCommandForPlatform/{platformId}")]
        public ActionResult <IEnumerable<CommandReadDto>>GetCommandForPlatform(int platformId)
        {
            var commands = _command.GetCommandsForPlatform(platformId);
            return Ok( _mapper.Map<CommandReadDto>(commands));
        }
        [HttpGet("GetCommand{commandId}For{platformId}")]
        public ActionResult <CommandReadDto> GetCommand(int commandId,int platformId)
        {
            
           var command= _command.GetCommand(platformId, commandId);
            return Ok( command );
        }
        [HttpPost("CreateCommand{platformId}")]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandDto)
        {
            var commandEntity = new Commands
            {
                PlatformId = platformId,
                CommandLine= commandDto.CommandLine,
                HowTo= commandDto.HowTo,
                

            };
            _command.CreateCommand(platformId, commandEntity);
            _command.SaveChanges();

            var commandReadDto=_mapper.Map<CommandReadDto>(commandEntity);
            return Ok(commandReadDto);
        }
    }
}
