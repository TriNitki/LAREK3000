using DeliveryAPI.Models.Domain;

namespace DeliveryAPI.Repositories.IRepositories
{
    public interface ICourierReceiptRepository
    {
        Task<List<CourierReceipt>> GetByCourierIdAsync(Guid courierId, bool sortByRecent, bool includeDelivered);
        Task<CourierReceipt?> GetByDeliveryIdAsync(Guid deliveryId);
        Task<Decimal> CalculateCourierProfitAsync(Guid courierId, DateTime? deliveriesFrom, DateTime? deliveriesTo);
        Task<CourierReceipt?> CreateAsync(CourierReceipt courierReceipt);
        Task<CourierReceipt?> SetDeliveryStatus(Guid deliveryId, bool deliveryStatus);
    }
}
