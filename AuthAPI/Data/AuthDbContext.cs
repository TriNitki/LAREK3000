using AuthAPI.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthAPI.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration configuration;

        public AuthDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("AuthConnectionString"));
        }
    }
}
