using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Models.DTO.DeliveryDTO;
using OrderAPI.Service.IService;
using OrderAPI.Utility;

namespace OrderAPI.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IBaseService baseService;

        public DeliveryService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto<DeliveryDto<CourierReceiptDto>?>?> CreateCourierDelivery(CreateDeliveryDto createDeliveryDto, string accessToken)
        {
            return await baseService.SendAsync<DeliveryDto<CourierReceiptDto>?>(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.DeliveryAPIBase + "/api/Delivery/Create",
                Data = createDeliveryDto,
                AccessToken = accessToken
            });
        }

        public async Task<ResponseDto<DeliveryDto<PickupReceiptDto>?>?> CreatePickupDelivery(CreateDeliveryDto createDeliveryDto, string accessToken)
        {
            return await baseService.SendAsync<DeliveryDto<PickupReceiptDto>?>(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.DeliveryAPIBase + "/api/Delivery/Create",
                Data = createDeliveryDto,
                AccessToken = accessToken
            });
        }
    }
}
