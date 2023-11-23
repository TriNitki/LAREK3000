using DeliveryAPI.Models.Enum;

namespace DeliveryAPI.Models.DTO.DeliveryDTO
{
    public class ReducedDeliveryDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public bool IsReceived { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public DateTime ShippingDT { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }
    }
}
