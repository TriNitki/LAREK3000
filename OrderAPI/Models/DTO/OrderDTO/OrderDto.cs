using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Models.DTO.ProductDTO;

namespace OrderAPI.Models.DTO.OrderDTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDT { get; set; }
        public bool IsCanceled { get; set; }
        public int Quantity { get; set; }

        public UserDto Buyer { get; set; }
        public ProductDto Product { get; set; }
    }
}
