using CatalogAPI.Data;
using CatalogAPI.Models.Domain;
using CatalogAPI.Repositories.IRepositories;
using CatalogAPI.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class PostgresProductRepository : IProductRepository
    {
        private readonly CatalogDbContext dbContext;

        public PostgresProductRepository(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var createdProduct = (await dbContext.Products.AddAsync(product)).Entity;
            await dbContext.SaveChangesAsync();

            createdProduct = await dbContext.Products.Include("Brand").Include("Category").FirstOrDefaultAsync(x => x.Id == createdProduct.Id);

            return createdProduct;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            dbContext.Products.Remove(existingProduct);
            await dbContext.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<List<Product>> GetAllAsync(
            string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, 
            int pageNumber = 1, int pageSize = 1000)
        {
            var products = dbContext.Products.Include("Brand").Include("Category").AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("ManufacturerCountry", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.ManufacturerCountry.Contains(filterQuery));
                }
                else if (filterOn.Equals("CategoryName", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.Category.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("BrandName", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.Brand.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Price) : products.OrderByDescending(x => x.Price);
                }
                else if (sortBy.Equals("WeightInKilos", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.WeightInKilos) : products.OrderByDescending(x => x.WeightInKilos);
                }
                else if (sortBy.Equals("ManufacturerCountry", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.ManufacturerCountry) : products.OrderByDescending(x => x.ManufacturerCountry);
                }
                else if (sortBy.Equals("CategoryName", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Category.Name) : products.OrderByDescending(x => x.Category.Name);
                }
                else if (sortBy.Equals("BrandName", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Brand.Name) : products.OrderByDescending(x => x.Brand.Name);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            products = products.Skip(skipResults).Take(pageSize);

            return await products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await dbContext.Products.Include("Brand").Include("Category").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> UpdateAsync(Guid id, Product product)
        {
            var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.WeightInKilos = product.WeightInKilos;
            existingProduct.ManufacturerCountry = product.ManufacturerCountry;

            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;

            await dbContext.SaveChangesAsync();

            var updatedProduct = await dbContext.Products.Include("Brand").Include("Category").FirstOrDefaultAsync(x => x.Id == id);
            return updatedProduct;
        }
    }
}
