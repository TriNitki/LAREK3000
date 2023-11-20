using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Domain;

namespace OrderAPI.Data
{
    public class OrderDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public OrderDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("OrderConnectionString"));
        }
    }
}
