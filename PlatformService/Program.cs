
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.AsyncDataService;

using PlatformService.Data;
using PlatformService.Interfaces;
using PlatformService.Models;
using PlatformService.Services;
using PlatformService.SyncDataServices.Http;
using RabbitMQ.Client;






namespace PlatformService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Connection to database
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
                //option.UseInMemoryDatabase("InMemory");
            });
           

            #endregion

            builder.Services.AddScoped<IPlatform, PlatformServices>();
         
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           builder.Services.AddHttpClient<ICommandDataClient,CommandDataClient>();
            // Register RabbitMQ connection and model as singletons
            builder.Services.AddSingleton<IConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQHost"],
                    Port = int.Parse(configuration["RabbitMQPort"])
                };
                return factory.CreateConnection();
            });

            builder.Services.AddSingleton<IModel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            //To Run the container
            //  builder.WebHost.UseUrls("http://0.0.0.0:8080");

            var app = builder.Build();
            app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());
           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //static data 
            PreDb.PrePopulation(app);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
