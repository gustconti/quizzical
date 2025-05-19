using api.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API configuration
            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User) // RefreshToken has one ApplicationUser
                .WithMany(u => u.RefreshTokens) // ApplicationUser has many RefreshTokens
                .HasForeignKey(rt => rt.UserId) // Foreign key for UserId in RefreshToken
                .OnDelete(DeleteBehavior.Cascade); // Optional: To configure cascade delete behavior
        }
    }
}
