using MapsterMapper;
using System.Text.Json;
using UserService.AsyncDataServices;
using UserService.Models;
using UserService.Models.DTOs;
using UserService.Repositories;

namespace UserService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageBusClient _messageBusClient;

        public EventProcessor(IServiceScopeFactory scopeFactory, 
            IMessageBusClient messageBusClient)
        {
            _scopeFactory = scopeFactory;
            _messageBusClient = messageBusClient;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserPublished:
                    AddUser(message);
                    break;
                default: 
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "User_Published":
                    Console.WriteLine("--> User Published Event Detected");
                    return EventType.UserPublished;
                default:
                    Console.WriteLine("--> Couls not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async void AddUser(string userPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                var userPublishedDto = JsonSerializer.Deserialize<UserPublishedDto>(userPublishedMessage);

                try 
                {
                    if(!await repo.ExternalUserExists(userPublishedDto.Email))
                    {
                        var user = await repo.AddUserAsync(mapper.Map<User>(userPublishedDto));

                        Console.WriteLine("--> User added!");

                        var responseDto = new ExternalUserPublishedDto(user.Id, user.Email, "ExternalUser_Published");
                        
                        _messageBusClient.PublishExternalUser(responseDto);
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType 
    {
        UserPublished,
        Undetermined
    }
}
