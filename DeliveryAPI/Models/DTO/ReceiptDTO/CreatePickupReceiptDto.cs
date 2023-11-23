namespace DeliveryAPI.Models.DTO.ReceiptDTO
{
    public class CreatePickupReceiptDto
    {
        public Guid DeliveryId { get; set; }
        public DateTime AvailableFromDT { get; set; }
        public DateTime AvailableToDT { get; set; }
    }
}
