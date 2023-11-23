using DeliveryAPI.Models.Domain;

namespace DeliveryAPI.Repositories.IRepositories
{
    public interface IPickupReceiptRepository
    {
        Task<PickupReceipt?> GetByDeliveryIdAsync(Guid deliveryId);
        Task<PickupReceipt?> CreateAsync(PickupReceipt pickupReceipt);
    }
}
