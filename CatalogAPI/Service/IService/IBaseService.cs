using CatalogAPI.Models.DTO.AuthDTO;

namespace CatalogAPI.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto<T>?> SendAsync<T>(RequestDto request);
    }
}
