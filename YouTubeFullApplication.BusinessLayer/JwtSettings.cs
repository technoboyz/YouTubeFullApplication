namespace YouTubeFullApplication.BusinessLayer
{
    public class JwtSettings
    {
        public int AccessTokenExpirationMinutes { get; init; }
        public int RefreshTokenExiprationMinutes { get; init; }
        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
    }
}
