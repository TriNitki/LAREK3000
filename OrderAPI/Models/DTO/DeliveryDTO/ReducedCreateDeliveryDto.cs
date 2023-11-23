using OrderAPI.Models.Enum;

namespace OrderAPI.Models.DTO.DeliveryDTO
{
    public class ReducedCreateDeliveryDto
    {
        public string Address { get; set; }
        public ReceiptMethodEnum ReceiptMethod { get; set; }
    }
}
