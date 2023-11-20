namespace OrderAPI.Models.DTO.ProductDTO
{
    public class ReducedProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double WeightInKilos { get; set; }
        public string? ManufacturerCountry { get; set; }

        public Guid SellerId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
