namespace MessageService.Infrastructure.Common.Settings
{
    public class GrpcScheduledTaskSettings
    {
        public const string SectionName = "GrpcScheduledTask";
        public string GrpcScheduledTask { get; set; } = null!;
    }
}
