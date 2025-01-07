using AutoMapper;
using Grpc.Core;
using PlatformService.Interfaces;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatform _platform;
        private readonly IMapper _mapper;
        public GrpcPlatformService(IPlatform platform, IMapper mapper)
        {
            _platform = platform;
            _mapper = mapper;
        }
        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request,ServerCallContext context)
        {
            var response=new PlatformResponse();
            var platforms=_platform.GetAllPlatform();
            foreach(var item in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platforms));

            }
            return Task.FromResult(response);   
        }
        
    }
}
