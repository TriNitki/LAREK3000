using DeliveryAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Data
{
    public class DeliveryDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DeliveryDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CourierReceipt> CourierReceipts { get; set; }
        public DbSet<PickupReceipt> PickupReceipts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DeliveryConnectionString"));
        }
    }
}
