using DeliveryAPI.Models.Enum;

namespace DeliveryAPI.Models.DTO.DeliveryDTO
{
    public class CreateDeliveryDto
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }
    }
}
