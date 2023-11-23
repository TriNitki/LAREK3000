using OrderAPI.Models.Enum;

namespace OrderAPI.Models.DTO.DeliveryDTO
{
    public class CreateDeliveryDto
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }
    }
}
