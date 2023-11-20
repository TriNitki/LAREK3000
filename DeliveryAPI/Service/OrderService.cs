using DeliveryAPI.Models.DTO.AuthDTO;
using DeliveryAPI.Models.DTO.OrderDTO;
using DeliveryAPI.Service.IService;
using DeliveryAPI.Utility;
using OrderAPI.Models.DTO.OrderDTO;

namespace DeliveryAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService baseService;

        public OrderService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto<OrderDto>?> CancelOrderByIdAsync(string accessToken, Guid orderId, CancelOrderDto cancelOrderDto)
        {
            return await baseService.SendAsync<OrderDto>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/Order/{orderId}",
                Data = cancelOrderDto,
                AccessToken = accessToken
            });
        }

        public async Task<ResponseDto<OrderDto>?> GetOrderByIdAsync(string accessToken, Guid orderId)
        {
            return await baseService.SendAsync<OrderDto>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/Order/{orderId}",
                AccessToken = accessToken
            });
        }

        public async Task<ResponseDto<List<ReducedOrderDto>>?> GetOrdersAsync(string accessToken, bool includeCanceled)
        {
            return await baseService.SendAsync<List<ReducedOrderDto>>(new RequestDto()
            {
                ApiType = SD.ApiType.PATCH,
                Url = SD.OrderAPIBase + "/api/Order",
                AccessToken = accessToken,
                Queries = new Dictionary<string, string?> { { "includeCanceled", includeCanceled.ToString() } }
            });
        }
    }
}
