// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace api.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for the refresh tokens linked to a user session
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ApplicationUser() { }
        public ApplicationUser(IdentityUser identityUser)
        {
            Id = identityUser.Id;
            UserName = identityUser.UserName;
            NormalizedUserName = identityUser.NormalizedUserName;
            Email = identityUser.Email;
            NormalizedEmail = identityUser.NormalizedEmail;
            EmailConfirmed = identityUser.EmailConfirmed;
            PasswordHash = identityUser.PasswordHash;
            SecurityStamp = identityUser.SecurityStamp;
            ConcurrencyStamp = identityUser.ConcurrencyStamp;
            PhoneNumber = identityUser.PhoneNumber;
            PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed;
            TwoFactorEnabled = identityUser.TwoFactorEnabled;
            LockoutEnd = identityUser.LockoutEnd;
            LockoutEnabled = identityUser.LockoutEnabled;
            AccessFailedCount = identityUser.AccessFailedCount;
        }
    }
}
