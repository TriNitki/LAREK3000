namespace OrderAPI.Models.DTO.OrderDTO
{
    public class CreateOrderDto
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
