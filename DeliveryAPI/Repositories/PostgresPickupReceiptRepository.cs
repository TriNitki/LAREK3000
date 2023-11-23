using DeliveryAPI.Data;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Repositories
{
    public class PostgresPickupReceiptRepository : IPickupReceiptRepository
    {
        private readonly DeliveryDbContext dbContext;

        public PostgresPickupReceiptRepository(DeliveryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PickupReceipt?> CreateAsync(PickupReceipt pickupReceipt)
        {
            var createdReceipt = (await dbContext.PickupReceipts.AddAsync(pickupReceipt)).Entity;
            await dbContext.SaveChangesAsync();
            return createdReceipt;
        }

        public async Task<PickupReceipt?> GetByDeliveryIdAsync(Guid deliveryId)
        {
            return await dbContext.PickupReceipts.FirstOrDefaultAsync(x =>  x.DeliveryId == deliveryId);
        }
    }
}
