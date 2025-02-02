namespace MessageService.Contracts.DTO.Message
{
    public record MessageSchedulerPublishedDto(
        Guid ExternalMessageId,
        Guid ExternalOwnerId,
        DateTime ScheduledTime,
        string Event = "Message_Published.Scheduler"
        );
}
