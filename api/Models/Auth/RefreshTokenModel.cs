namespace api.Models.Auth
{
    public class RefreshTokenModel
    {
        public required string Token { get; set; }
        public required string UserName { get; set; }
        public required DateTime Expiration { get; set; }
    }  
}