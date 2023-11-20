using CatalogAPI.Models.Domain;

namespace CatalogAPI.Repositories.IRepositories
{
    public interface IBrandRepository
    {
        Task<Brand?> GetAsync(Guid id);
        Task<List<Brand>> GetAllAsync(string? sortBy = null, bool isAscending = true);
        Task<Brand> CreateAsync(Brand brand);
        Task<Brand?> UpdateNameAsync(Guid id, string brandName);
        Task<bool> DeleteAsync(Guid id);
    }
}
