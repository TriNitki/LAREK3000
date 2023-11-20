namespace OrderAPI.Models.DTO.OrderDTO
{
    public class ReducedOrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDT { get; set; }
        public bool IsCanceled { get; set; }
        public int Quantity { get; set; }

        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
