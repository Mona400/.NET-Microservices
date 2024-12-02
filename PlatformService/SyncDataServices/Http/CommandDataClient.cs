using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public CommandDataClient(HttpClient httpClient, IConfiguration configuration = null)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformCommand(PlatformReadDto plat)
        {

            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
                );
            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post To Command Service was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync Post To Command Service was Not OK!");
            }
           
        }
    }
}
