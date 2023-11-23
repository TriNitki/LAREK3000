using DeliveryAPI.Models.DTO.DeliveryTimeDTO;
using DeliveryAPI.Service.IService;

namespace DeliveryAPI.Service
{
    public class DeliveryTimeService : IDeliveryTimeService
    {
        public async Task<AvailableGapDto> CalculateAvailableGap(Guid orderId, string deliveryAddress)
        {
            var rnd = new Random();
            var availableFrom = DateTime.Now.AddDays(rnd.Next(2, 5));
            var availableTo = availableFrom.AddDays(4);

            var availableGapDto = new AvailableGapDto()
            {
                AvailableFromDT = availableFrom,
                AvailableToDT = availableTo
            };

            return availableGapDto;
        }

        public async Task<DateTime> CalculateCourierDeliveryDT(DateTime shippingDT, string deliveryAddress)
        {
            return shippingDT.AddDays(new Random().Next(4, 6));
        }

        public async Task<DateTime> CalculateShippingDT(Guid orderId, string deliveryAddress)
        {
            return DateTime.Now.AddDays(new Random().Next(4, 6));
        }
    }
}
