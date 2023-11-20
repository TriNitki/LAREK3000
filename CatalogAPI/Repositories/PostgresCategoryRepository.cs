using CatalogAPI.Data;
using CatalogAPI.Models.Domain;
using CatalogAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class PostgresCategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext dbContext;
        public PostgresCategoryRepository(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return false;
            }

            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetAllAsync(string? sortBy = null, bool isAscending = true)
        {
            var categories = dbContext.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = isAscending ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
                }
            }

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateNameAsync(Guid id, string name)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = name;

            await dbContext.SaveChangesAsync();
            return existingCategory;
        }
    }
}
