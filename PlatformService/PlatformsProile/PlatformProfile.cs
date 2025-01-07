using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.PlatformsProile
{
    public class PlatformProfile:Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dist=>dist.PlatformId,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dist=>dist.Publisher,opt=>opt.MapFrom(src=>src.Publisher))
                .ForMember(dist=>dist.Name,opt=>opt.MapFrom(src=>src.Name));
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformUpdateDto, Platform>();

            CreateMap<PlatformReadDto, PlatformPublishDto>();
        }
    }
}
