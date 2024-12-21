using CommandsService.Data;
using CommandsService.Interfaces;
using CommandsService.Models;

namespace CommandsService.Services
{
    public class CommandServices : ICommand
    {
        private readonly AppDbContext _context;

        public CommandServices(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platform, Commands commands)
        {
            if (commands == null) 
                throw new ArgumentNullException(nameof(commands));
            commands.PlatformId = platform;
            _context.Commands.Add(commands);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) 
                throw new ArgumentNullException();
            _context.Platforms.Add(platform);

        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(x => x.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
           return _context.Platforms.ToList();
        }

        public Commands GetCommand(int platformId, int commandId)
        {
            return _context.Commands.Where(c => c.Id == commandId && c.PlatformId == platformId).FirstOrDefault();
        }

        public IEnumerable<Commands> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c=>c.PlatformId == platformId)
                .OrderBy(c=>c.Platform.Name);
        }

        public bool PlatformExist(int platformId)
        {
            return _context.Platforms.Any(x => x.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>= 0);
        }
    }
}
