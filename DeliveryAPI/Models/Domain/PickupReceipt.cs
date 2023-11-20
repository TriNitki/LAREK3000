using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.Domain
{
    public class PickupReceipt
    {
        [Key]
        public Guid DeliveryId { get; set; }
        public DateTime AvailableFromDT { get; set; }
        public DateTime AvailableToDT { get; set; }
    }
}
