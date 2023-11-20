namespace OrderAPI.Models.DTO.OrderDTO
{
    public class UpdateOrderDto
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
