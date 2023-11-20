using DeliveryAPI.Models.DTO.AuthDTO;
using DeliveryAPI.Models.DTO.ProductDTO;

namespace DeliveryAPI.Models.DTO.OrderDTO
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
