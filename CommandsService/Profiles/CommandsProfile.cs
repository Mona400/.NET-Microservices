using AutoMapper;
using CommandsService.Dto;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile:Profile
    {
        public CommandsProfile()
        {
            CreateMap<Commands, CommandCreateDto>();
            CreateMap<Commands, CommandReadDto>();
            CreateMap<Platform, PlatformReadDto>();
        }
    }
}
