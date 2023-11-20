using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models.Domain;
using OrderAPI.Repositories.IRepositories;

namespace OrderAPI.Repositories
{
    public class PostgresOrderRepository : IOrderRepository
    {
        private readonly OrderDbContext dbContext;

        public PostgresOrderRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order?> SetCancelStatusAsync(Guid id, bool isCanceled)
        {
            var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.IsCanceled = isCanceled;
            await dbContext.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            var createdOrder = (await dbContext.Orders.AddAsync(order)).Entity;
            await dbContext.SaveChangesAsync();
            return createdOrder;
        }

        public async Task<Order?> DeleteAsync(Guid id)
        {
            var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (existingOrder == null)
            {
                return null;
            }

            dbContext.Orders.Remove(existingOrder);
            await dbContext.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<List<Order>> GetAllByUserIdAsync(Guid userId, bool includeCanceled = true)
        {
            IQueryable<Order> orderQuery;
            if (includeCanceled)
            {
                orderQuery = dbContext.Orders.AsQueryable();
            }
            else
            {
                orderQuery = dbContext.Orders.Where(x => x.IsCanceled == false).AsQueryable();
            }

            var products = await orderQuery.Where(x => x.BuyerId == userId).ToListAsync();
            return products;
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> UpdateAsync(Guid id, Order order)
        {
            var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (existingOrder == null)
            {
                return null;
            }

            existingOrder.Quantity = order.Quantity;
            existingOrder.ProductId = order.ProductId;

            await dbContext.SaveChangesAsync();

            var updatedProduct = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            return updatedProduct;
        }
    }
}
