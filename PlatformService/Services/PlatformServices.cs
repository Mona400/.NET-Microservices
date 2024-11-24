using PlatformService.Data;
using PlatformService.Interfaces;
using PlatformService.Models;

namespace PlatformService.Services
{
    public class PlatformServices : IPlatform
    {
        private readonly AppDbContext _dbContext;

        public PlatformServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public Platform CreatePlateform(Platform platform)
		{
			_dbContext.Platforms.Add(platform);
			_dbContext.SaveChanges();
			return platform; 
		}
		
		public void UpdatePlatform(Platform platform,int Id)
		{
			var platformEntity= _dbContext.Platforms.FirstOrDefault(x => x.Id == Id);
			platformEntity.Name=platform.Name;
            platformEntity.Publisher=platform.Publisher;
            platformEntity.Cost=platform.Cost;
           
            _dbContext.Platforms.Update(platformEntity);
			_dbContext.SaveChanges();
		}
		
		public IEnumerable<Platform> GetAllPlatform()
        {
            return _dbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int Id)
        {
            return  _dbContext.Platforms.FirstOrDefault(x => x.Id == Id);
        }
		public bool DeletePlatform(int Id)
		{
			var platformEntity= _dbContext.Platforms.FirstOrDefault(x => x.Id == Id);
            if (platformEntity != null)
            {
				_dbContext.Platforms.Remove(platformEntity);
				_dbContext.SaveChanges();
                return true;
			}
            return false;
		}

		public bool SaveChanges()
        {
            return _dbContext.SaveChanges()>=0;
        }

		
	}
}
