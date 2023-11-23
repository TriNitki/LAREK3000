using OrderAPI.Models.DTO.AuthDTO;

namespace OrderAPI.Models.DTO.DeliveryDTO
{
    public class CourierReceiptDto
    {
        public UserDto Courier { get; set; }
        public Decimal CourierProfit { get; set; }
        public DateTime DeliveryDT { get; set; }
        public bool IsDelivered { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
