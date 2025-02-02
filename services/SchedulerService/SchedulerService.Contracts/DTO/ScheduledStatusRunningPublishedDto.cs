namespace SchedulerService.Contracts.DTO
{
    public record ScheduledStatusRunningPublishedDto(
        Guid ExternalMessageId,
        string Event = "ScheduledStatusChangedToRunning_Published"
        );
}
