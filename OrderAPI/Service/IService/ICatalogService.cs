using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Models.DTO.ProductDTO;

namespace OrderAPI.Service.IService
{
    public interface ICatalogService
    {
        Task<ResponseDto<ProductDto>?> GetProductByIdAsync(Guid productId);
        Task<ResponseDto<List<ReducedProductDto>>?> GetProductsAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
    }
}
