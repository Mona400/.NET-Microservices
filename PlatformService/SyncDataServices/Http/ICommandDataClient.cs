using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        public Task SendPlatformCommand(PlatformReadDto plat);
    }
}
