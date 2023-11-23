using DeliveryAPI.Models.DTO.AuthDTO;

namespace DeliveryAPI.Models.DTO.CourierDTO
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
