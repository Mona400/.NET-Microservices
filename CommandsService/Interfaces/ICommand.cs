using CommandsService.Models;

namespace CommandsService.Interfaces
{
    public interface ICommand
    {

        bool SaveChanges();
        //platform
        IEnumerable <Platform>GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExist(int platformId);
        bool ExternalPlatformExists(int platformId);
        //command
        IEnumerable<Commands>GetCommandsForPlatform(int platformId);
        Commands GetCommand(int platformId, int commandId);
        void CreateCommand(int platform,Commands commands);
    }
}
