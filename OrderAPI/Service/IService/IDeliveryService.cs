using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Models.DTO.DeliveryDTO;

namespace OrderAPI.Service.IService
{
    public interface IDeliveryService
    {
        Task<ResponseDto<DeliveryDto<CourierReceiptDto>?>?> CreateCourierDelivery(CreateDeliveryDto createDeliveryDto, string accessToken);
        Task<ResponseDto<DeliveryDto<PickupReceiptDto>?>?> CreatePickupDelivery(CreateDeliveryDto createDeliveryDto, string accessToken);
    }
}
