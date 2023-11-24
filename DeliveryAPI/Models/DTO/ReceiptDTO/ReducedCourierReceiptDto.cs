namespace DeliveryAPI.Models.DTO.ReceiptDTO
{
    public class ReducedCourierReceiptDto
    {
        public Guid DeliveryId { get; set; }
        public Decimal CourierProfit { get; set; }
        public DateTime DeliveryDT { get; set; }
        public bool IsDelivered { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
