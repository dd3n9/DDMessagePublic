using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.EventProcessing;
using NotificationService.Application.Services;
using NotificationService.Contracts.DTO;
using NotificationService.Domain.NotificationAggregate;
using NotificationService.Domain.NotificationAggregate.ValueObjects;
using NotificationService.Domain.Repositories;
using System.Text.Json;

namespace NotificationService.Infrastructure.EventProcessing
{
    internal sealed class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.MessagePublished:
                    await AddNotification(message);
                    break;
                case EventType.ScheduledStatusChangedToRunning:
                    await HandleScheduledStatusChangedToRunning(message);
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
                case "Message_Published.Notification":
                    Console.WriteLine("--> Message Published Event Detected");
                    return EventType.MessagePublished;
                case "ScheduledStatusChangedToRunning_Published":
                    Console.WriteLine("--> Scheduled StatusChanged To Running Event Detected");
                    return EventType.ScheduledStatusChangedToRunning;
                default:
                    Console.WriteLine("--> Coud not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task AddNotification(string notificationPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                var notificationPublishedDto = JsonSerializer.Deserialize<NotificationPublishedDto>(notificationPublishedMessage);


                try
                {
                    if (!await repo.ExistsByMessageId(notificationPublishedDto.ExternalMessageId))
                    {
                        var notification = Notification.Create(
                            notificationPublishedDto.Content,
                            notificationPublishedDto.DeliveryDate,
                            notificationPublishedDto.ExternalMessageId,
                            notificationPublishedDto.RecipientsEmail
                                .Select(email => new RecipientEmail(email))
                                .ToList()
                            );

                        await repo.AddAsync(notification);

                        Console.WriteLine("--> Notificaton Added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Notificaton already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
            }
        }

        private async Task HandleScheduledStatusChangedToRunning(string notificationPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var schedulerTask = JsonSerializer.Deserialize<ScheduledStatusRunningPublishedDto>(notificationPublishedMessage);

                try
                {
                    await service.SendNotificationAsync(schedulerTask.ExternalMessageId);

                    Console.WriteLine("HandleScheduledStatusChangedToRunning successfull");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not HandleScheduledStatusChangedToRunning: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        MessagePublished,
        ScheduledStatusChangedToRunning,
        Undetermined
    }
}
