using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.Domain
{
    public class CourierReceipt
    {
        [Key]
        public Guid DeliveryId { get; set; }
        public Guid CourierID { get; set; }
        public Decimal CourierProfit { get; set; }
        public DateTime DeliveryDT { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
