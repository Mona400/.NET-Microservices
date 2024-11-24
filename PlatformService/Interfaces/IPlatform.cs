using PlatformService.Models;

namespace PlatformService.Interfaces
{
    public interface IPlatform
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatform();
        Platform GetPlatformById(int Id);
        Platform CreatePlateform(Platform platform);
		void UpdatePlatform(Platform platform, int Id);
		bool DeletePlatform(int id);

	}
}
