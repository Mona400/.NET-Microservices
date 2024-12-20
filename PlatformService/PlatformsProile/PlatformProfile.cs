using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.PlatformsProile
{
    public class PlatformProfile:Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformUpdateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
        }
    }
}
