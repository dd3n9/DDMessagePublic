namespace MessageService.Infrastructure.Common.Settings
{
    public class GrpcUserSettings
    {
        public const string SectionName = "GrpcUser";
        public string GrpcUser { get; set; } = null!;
    }
}
