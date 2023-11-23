using DeliveryAPI.Models.DTO.AuthDTO;
using DeliveryAPI.Service.IService;
using System;

namespace DeliveryAPI.Service
{
    public class CourierService : ICourierService
    {
        private readonly IAuthService authService;

        public CourierService(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<decimal> CalculateProfit(Guid courierId, Guid orderId, string deliveryAddress)
        {
            return new Random().Next(25, 50) * 10;
        }

        public async Task<UserDto?> FindCourier(Guid orderId, string accessToken)
        {
            var response = await authService.GetCouriers(accessToken);

            if (response == null || !response.IsSuccess || response.Result == null) 
            { 
                return null;
            }

            var couriers = response.Result;
            var courierIndex = new Random().Next(0, couriers.Count);

            var courierDto = couriers[courierIndex];
            return courierDto;
        }
    }
}
