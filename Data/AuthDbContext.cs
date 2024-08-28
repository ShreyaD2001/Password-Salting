using AuthUser.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthUser.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthUsers> AuthUsers { get; set; }

        // Other DbSets and configurations
    }
}
