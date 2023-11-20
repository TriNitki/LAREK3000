using DeliveryAPI.Models.DTO.AuthDTO;
using DeliveryAPI.Models.DTO.BrandDTO;
using DeliveryAPI.Models.DTO.CategoryDTO;

namespace DeliveryAPI.Models.DTO.ProductDTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double WeightInKilos { get; set; }
        public string? ManufacturerCountry { get; set; }

        public UserDto Seller { get; set; }
        public BrandDto? Brand { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
