using CatalogAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data
{
    public class CatalogDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public CatalogDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("CatalogConnectionString"));
        }
    }
}
