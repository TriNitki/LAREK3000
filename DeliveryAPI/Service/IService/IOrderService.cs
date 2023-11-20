using DeliveryAPI.Models.DTO.AuthDTO;
using DeliveryAPI.Models.DTO.OrderDTO;
using OrderAPI.Models.DTO.OrderDTO;

namespace DeliveryAPI.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto<OrderDto>?> GetOrderByIdAsync(string accessToken, Guid orderId);
        Task<ResponseDto<List<ReducedOrderDto>>?> GetOrdersAsync(string accessToken, bool includeCanceled);
        Task<ResponseDto<OrderDto>?> CancelOrderByIdAsync(string accessToken, Guid orderId, CancelOrderDto cancelOrderDto);
    }
}
