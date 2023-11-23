using DeliveryAPI.Models.DTO.CourierDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;

namespace DeliveryAPI.Service.IService
{
    public interface IReceiptService
    {
        Task<CourierReceiptDto?> Create(CreateCourierReceiptDto courierReceiptDto);
        Task<PickupReceiptDto?> Create(CreatePickupReceiptDto pickupReceiptDto);
    }
}
