// Models/RefreshToken.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities.Auth
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime? RevokedAt { get; set; } = null;
        public DateTime? UsedAt { get; set; } = null;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}
