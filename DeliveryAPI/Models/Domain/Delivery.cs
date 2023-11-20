namespace DeliveryAPI.Models.Domain
{
    public class Delivery
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public bool IsRecieved { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public DateTime ShippingDT { get; set; }
        public string ReceiptMethod { get; set; }
        public Guid ReceiptId { get; set; }
    }
}
