using OrderAPI.Models.DTO.AuthDTO;

namespace OrderAPI.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto<T>?> SendAsync<T>(RequestDto request);
    }
}
