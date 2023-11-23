using DeliveryAPI.Data;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Repositories
{
    public class PostgresCourierReceiptRepository : ICourierReceiptRepository
    {
        private readonly DeliveryDbContext dbContext;

        public PostgresCourierReceiptRepository(DeliveryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<decimal> CalculateCourierProfitAsync(Guid courierId, DateTime? from, DateTime? to)
        {
            var deliveries = dbContext.CourierReceipts.Where(x => x.IsDelivered == true).AsQueryable();

            if (from != null)
            {
                deliveries = deliveries.Where(x => x.DeliveryDT >= from).AsQueryable();
            }

            if (to != null)
            {
                deliveries = deliveries.Where(x => x.DeliveryDT <= to).AsQueryable();
            }

            return await deliveries.SumAsync(x => x.CourierProfit);
        }

        public async Task<CourierReceipt?> CreateAsync(CourierReceipt courierReceipt)
        {
            var createdReceipt = (await dbContext.CourierReceipts.AddAsync(courierReceipt)).Entity;
            await dbContext.SaveChangesAsync();
            return createdReceipt;
        }

        public async Task<List<CourierReceipt>> GetByCourierIdAsync(Guid courierId, bool sortByRecent, bool includeDelivered)
        {
            var deliveries = dbContext.CourierReceipts.AsQueryable();

            if (!includeDelivered)
            {
                deliveries = deliveries.Where(x => x.IsDelivered == false).AsQueryable();
            }

            if (sortByRecent)
            {
                deliveries = deliveries.OrderByDescending(x => x.DeliveryDT).AsQueryable();
            }

            return await deliveries.Where(x => x.CourierId == courierId).ToListAsync();
        }

        public async Task<CourierReceipt?> GetByDeliveryIdAsync(Guid deliveryId)
        {
            return await dbContext.CourierReceipts.FirstOrDefaultAsync(x => x.DeliveryId == deliveryId);
        }

        public async Task<CourierReceipt?> SetDeliveryStatus(Guid deliveryId, bool deliveryStatus)
        {
            var existingReceipt = await dbContext.CourierReceipts.FirstOrDefaultAsync(x => x.DeliveryId == deliveryId);

            if (existingReceipt == null)
            {
                return null;
            }

            existingReceipt.IsDelivered = deliveryStatus;
            await dbContext.SaveChangesAsync();
            return existingReceipt;
        }
    }
}
