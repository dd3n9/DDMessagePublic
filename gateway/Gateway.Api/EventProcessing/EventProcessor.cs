using Gateway.Api.Models.DTOs;
using Gateway.Api.Repositories;
using System.Text.Json;

namespace Gateway.Api.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.ExternalUserPublished:
                    HandleExternalIdUpdate(message);
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
                case "ExternalUser_Published":
                    Console.WriteLine("--> External User Published Event Detected");
                    return EventType.ExternalUserPublished;
                default:
                    Console.WriteLine("--> Coud not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async void HandleExternalIdUpdate(string publishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var externalUserResponse = JsonSerializer.Deserialize<ExternalUserResponseDto>(publishedMessage);

                try
                {
                    await repo.UpdateUserExternalId(externalUserResponse.Email, externalUserResponse.ExternalId);
                    Console.WriteLine("--> Updated User ExternalId in AuthService!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update ExternalId: {ex.Message}");
                }
            }

        }
    }
    enum EventType
    {
        ExternalUserPublished,
        Undetermined
    }
}
