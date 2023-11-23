using DeliveryAPI.Data;
using DeliveryAPI.Models.Domain;
using DeliveryAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Repositories
{
    public class PostgresDeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryDbContext dbContext;

        public PostgresDeliveryRepository(DeliveryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Delivery?> CreateAsync(Delivery delivery)
        {
            var createdDelivery = (await dbContext.Deliveries.AddAsync(delivery)).Entity;
            await dbContext.SaveChangesAsync();
            return createdDelivery;
        }

        public async Task<Delivery?> GetByIdAsync(Guid id)
        {
            return await dbContext.Deliveries.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Delivery>> GetByOrderIdAsync(Guid orderId, bool? isRecieved, bool? isCanceled)
        {
            var deliveries = dbContext.Deliveries.Where(x => x.OrderId == orderId).AsQueryable();

            if (isRecieved != null)
            {
                deliveries = deliveries.Where(x => x.IsReceived == isRecieved).AsQueryable();
            }

            if (isCanceled != null)
            {
                deliveries = deliveries.Where(x => x.IsCanceled == isCanceled).AsQueryable();
            }

            return await deliveries.ToListAsync();
        }

        public async Task<Delivery?> SetCancelStatusAsync(Guid id, bool cancelStatus)
        {
            var existingDelivery = await dbContext.Deliveries.FirstOrDefaultAsync(x => x.Id == id);

            if (existingDelivery == null)
            {
                return null;
            }

            existingDelivery.IsCanceled = cancelStatus;
            await dbContext.SaveChangesAsync();
            return existingDelivery;
        }

        public async Task<Delivery?> SetReceiveStatusAsync(Guid id, bool receiveStatus)
        {
            var existingDelivery = await dbContext.Deliveries.FirstOrDefaultAsync(x => x.Id == id);

            if (existingDelivery == null)
            {
                return null;
            }

            existingDelivery.IsReceived = receiveStatus;
            await dbContext.SaveChangesAsync();
            return existingDelivery;
        }
    }
}
