
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.AsyncDataService;

using PlatformService.Data;
using PlatformService.Interfaces;
using PlatformService.Models;
using PlatformService.Services;
using PlatformService.SyncDataServices.Grpc;
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
            builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();
            builder.Services.AddGrpc();
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

            // Enable Routing
            app.UseRouting(); // This is necessary for routing requests to the correct endpoints


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           
            //static data 
            PreDb.PrePopulation(app);

            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcPlatformService>();
                endpoints.MapGet("/protos/platform.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("protos/platform.proto"));
                });

            });

            app.MapControllers();

            app.Run();
        }
    }
}
