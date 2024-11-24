namespace PlatformService.Data
{
    public static class PreDb
    {
        public static void PrePopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
               SeedDate(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedDate(AppDbContext context)
        {
            if(!context.Platforms.Any())
            {
                Console.WriteLine("---->Seeding Data");
                context.Platforms.AddRange(
                    new Models.Platform() { Name="Dot Net",Publisher="Microsoft",Cost="free"},
                    new Models.Platform() { Name="SQL Server Express",Publisher="Microsoft",Cost="free"},
                    new Models.Platform() { Name="Kubernates",Publisher="Cloud Native Computing Fundation",Cost="free"}
                    
                    );
                context.SaveChanges();
            }
            else
            {

            }
        
        }

    }
}
