namespace SchedulerService.Contracts.DTO
{
    public record ScheduledTaskPublishedDto(
        Guid ExternalMessageId,
        Guid ExternalOwnerId,
        DateTime ScheduledTime
        );
}
