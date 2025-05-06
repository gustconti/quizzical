// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace api.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        // You can extend this class with custom fields later
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
