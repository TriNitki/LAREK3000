using CatalogAPI.Models.Domain;

namespace CatalogAPI.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetAsync(Guid id);
        Task<List<Category>> GetAllAsync(string? sortBy = null, bool isAscending = true);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateNameAsync(Guid id, string name);
        Task<bool> DeleteAsync(Guid id);
    }
}
