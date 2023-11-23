using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.Domain
{
    public class CourierReceipt
    {
        [Key]
        public Guid DeliveryId { get; set; }
        public Guid CourierId { get; set; }
        public Decimal CourierProfit { get; set; }
        public DateTime DeliveryDT { get; set; }
        public bool IsDelivered { get; set; } = false;
        public string DeliveryAddress { get; set; }
    }
}
