using Microsoft.Extensions.DependencyInjection;
using SchedulerService.Application.EventProcessing;
using SchedulerService.Contracts.DTO;
using SchedulerService.Domain.Repositories;
using SchedulerService.Domain.ScheduledTaskAggregate;
using System.Text.Json;

namespace SchedulerService.Infrastructure.EventProcessing
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
                    await AddScheduledTask(message);
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
                case "Message_Published.Scheduler":
                    Console.WriteLine("--> Message Published Event Detected");
                    return EventType.MessagePublished;
                default:
                    Console.WriteLine("--> Coud not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task AddScheduledTask(string notificationPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IScheduledTaskRepository>();
                var scheduledTaskPublishedDto = JsonSerializer.Deserialize<ScheduledTaskPublishedDto>(notificationPublishedMessage);


                try
                {
                    if (!await repo.ExistsByMessageId(scheduledTaskPublishedDto.ExternalMessageId))
                    {
                        var scheduledTask = ScheduledTask.Create(
                            scheduledTaskPublishedDto.ExternalMessageId, 
                            scheduledTaskPublishedDto.ExternalOwnerId,
                            scheduledTaskPublishedDto.ScheduledTime
                            );

                        await repo.AddAsync(scheduledTask);

                        Console.WriteLine("--> ScheduledTask Added!");
                    }
                    else
                    {
                        Console.WriteLine("--> ScheduledTask already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add ScheduledTask to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        MessagePublished,
        Undetermined
    }
}
