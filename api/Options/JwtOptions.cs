namespace api.Options
{
    public class JwtOptions
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
        public int ExpiresInSeconds => ExpiresInMinutes * 60;
    }
}