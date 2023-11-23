using DeliveryAPI.Models.DTO.OrderDTO;
using DeliveryAPI.Models.DTO.ReceiptDTO;
using DeliveryAPI.Models.Enum;

namespace DeliveryAPI.Models.DTO.DeliveryDTO
{
    public class DeliveryDto<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsRecieved { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public DateTime ShippingDT { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }

        public T ReceiptInfo { get; set; }

        public OrderDto Order { get; set; }
    }
}
