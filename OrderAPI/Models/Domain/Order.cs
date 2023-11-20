namespace OrderAPI.Models.Domain
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDT { get; set; } = DateTime.Now;
        public bool IsCanceled { get; set; } = false;
        public int Quantity { get; set; }

        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
