using DeliveryAPI.Models.Domain;

namespace DeliveryAPI.Repositories.IRepositories
{
    public interface IDeliveryRepository
    {
        Task<Delivery?> GetByIdAsync(Guid id);
        Task<List<Delivery>> GetByOrderIdAsync(Guid orderId, bool? isRecieved, bool? isCanceled);
        Task<Delivery?> CreateAsync(Delivery delivery);
        Task<Delivery?> SetReceiveStatusAsync(Guid id, bool receiveStatus);
        Task<Delivery?> SetCancelStatusAsync(Guid id, bool cancelStatus);
    }
}
