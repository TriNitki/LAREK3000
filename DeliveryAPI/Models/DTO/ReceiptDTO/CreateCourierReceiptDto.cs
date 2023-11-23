namespace DeliveryAPI.Models.DTO.ReceiptDTO
{
    public class CreateCourierReceiptDto
    {
        public Guid DeliveryId { get; set; }
        public Guid CourierId { get; set; }
        public decimal CourierProfit { get; set; }
        public DateTime DeliveryDT { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
