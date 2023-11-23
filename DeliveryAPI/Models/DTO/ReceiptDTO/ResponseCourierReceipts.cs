using DeliveryAPI.Models.DTO.AuthDTO;

namespace DeliveryAPI.Models.DTO.ReceiptDTO
{
    public class ResponseCourierReceipts
    {
        public UserDto Courier { get; set; }
        public List<ReducedCourierReceiptDto> Receipts { get; set; }
    }
}
