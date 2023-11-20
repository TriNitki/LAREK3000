using DeliveryAPI.Models.DTO.AuthDTO;

namespace DeliveryAPI.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto<T>?> SendAsync<T>(RequestDto request);
    }
}
