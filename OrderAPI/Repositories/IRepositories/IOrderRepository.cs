using OrderAPI.Models.Domain;

namespace OrderAPI.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllByUserIdAsync(Guid userId, bool includeCanceled = true);
        Task<Order> CreateAsync(Order order);
        Task<Order?> UpdateAsync(Guid id, Order order);
        Task<Order?> DeleteAsync(Guid id);
        Task<Order?> SetCancelStatusAsync(Guid id, bool isCanceled);
    }
}
