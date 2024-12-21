using AutoMapper;
using CommandsService.Dto;
using CommandsService.Interfaces;
using CommandsService.Models;
using System.Text.Json;


namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
          
        }

        public void ProcessEvent(string message)
        {
            Console.WriteLine($"Processing Message: {message}");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    Console.WriteLine("Event type not recognized.");
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Platform_Publish":
                    Console.WriteLine("Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine($"Unrecognized event type: {eventType.Event}");
                    return EventType.UnderDetermined;
            }

        }
        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommand>();
                var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

                Console.WriteLine($"Deserialized Platform: {platformPublishDto?.Name}, {platformPublishDto?.Id}");

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishDto);
                    Console.WriteLine($"Mapped Platform: {plat.Name}, {plat.ExternalID}");

                    if (!repo.ExternalPlatformExists((int)plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("Platform added to database.");
                    }
                    else
                    {
                        Console.WriteLine($"Platform with ExternalID {plat.ExternalID} already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding platform to DB: {ex.Message}");
                }
            }
        }


    }
    enum EventType
    {
        PlatformPublished,
        UnderDetermined
    }

}
