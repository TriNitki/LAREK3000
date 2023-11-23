using DeliveryAPI.Models.Enum;

namespace DeliveryAPI.Models.Domain
{
    public class Delivery
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public bool IsReceived { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public DateTime ShippingDT { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }
    }
}
