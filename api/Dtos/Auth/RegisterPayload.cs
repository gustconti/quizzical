namespace api.Dtos.Auth
{
    public class RegisterPayload
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}