namespace Gateway.Api.Configurations
{
    public class JwtConfig
    {
        public const string SectionName = "JwtConfig";
        public string Secret { get; init; } = null!;
        public int ExpiryMinutes { get; init; }
        public TimeSpan ExpiryTimeFrame { get; init; }
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
    }
}
