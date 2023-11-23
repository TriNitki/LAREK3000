using DeliveryAPI.Models.DTO.DeliveryTimeDTO;

namespace DeliveryAPI.Service.IService
{
    public interface IDeliveryTimeService
    {
        Task<DateTime> CalculateShippingDT(Guid orderId, string deliveryAddress);
        Task<AvailableGapDto> CalculateAvailableGap(Guid orderId, string deliveryAddress);
        Task<DateTime> CalculateCourierDeliveryDT(DateTime shippingDT, string deliveryAddress);
    }
}