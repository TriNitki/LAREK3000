namespace CatalogAPI.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double WeightInKilos { get; set; }
        public string? ManufacturerCountry { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }

        public Category Category { get; set; }
        public Brand Brand { get; set; }
    }
}
