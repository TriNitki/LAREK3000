using OrderAPI.Models.DTO.AuthDTO;
using OrderAPI.Models.DTO.ProductDTO;
using OrderAPI.Service.IService;
using OrderAPI.Utility;

namespace OrderAPI.Service
{
    public class CatalogService : ICatalogService
    {
        private readonly IBaseService baseService;

        public CatalogService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto<ProductDto>?> GetProductByIdAsync(Guid productId)
        {
            return await baseService.SendAsync<ProductDto>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CatalogAPIBase + $"/api/Product/{productId}"
            });
        }

        public async Task<ResponseDto<List<ReducedProductDto>>?> GetProductsAsync(
            string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            return await baseService.SendAsync<List<ReducedProductDto>>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CatalogAPIBase + "/api/Product",
                Queries = new Dictionary<string, string?>
                {
                    { "filterOn", filterOn},
                    { "filterQuery", filterQuery },
                    { "sortBy", sortBy },
                    { "isAscending", isAscending.ToString() },
                    { "pageNumber", pageNumber.ToString() },
                    {  "pageSize", pageSize.ToString() }
                }
            });
        }
    }
}
