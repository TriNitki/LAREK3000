using CatalogAPI.Data;
using CatalogAPI.Models.Domain;
using CatalogAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class PostgresBrandRepository : IBrandRepository
    {
        private readonly CatalogDbContext dbContext;

        public PostgresBrandRepository(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            await dbContext.Brands.AddAsync(brand);
            await dbContext.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBrand == null) 
            {
                return false;
            }

            dbContext.Brands.Remove(existingBrand);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Brand>> GetAllAsync(string? sortBy = null, bool isAscending = true)
        {
            var brands = dbContext.Brands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    brands = isAscending ? brands.OrderBy(x => x.Name) : brands.OrderByDescending(x => x.Name);
                }
            }

            return await brands.ToListAsync();
        }

        public async Task<Brand?> GetAsync(Guid id)
        {
            return await dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Brand?> UpdateNameAsync(Guid id, string brandName)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBrand == null)
            {
                return null;
            }

            existingBrand.Name = brandName;

            await dbContext.SaveChangesAsync();
            return existingBrand;
        }
    }
}
