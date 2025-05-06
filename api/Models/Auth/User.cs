namespace api.Models.Auth
{
    public class User
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public string? UserName { get; set; }
    }
}