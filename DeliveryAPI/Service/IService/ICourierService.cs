using DeliveryAPI.Models.DTO.AuthDTO;

namespace DeliveryAPI.Service.IService
{
    public interface ICourierService
    {
        Task<decimal> CalculateProfit(Guid courierId, Guid orderId, string deliveryAddress);
        Task<UserDto?> FindCourier(Guid orderId, string accessToken);
    }
}
