using DeliveryAPI.Models.DTO.AuthDTO;

namespace DeliveryAPI.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto<UserDto>?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto<UserDto>?> GetUserByIdAsync(Guid userId);
        Task<ResponseDto<UserDto>?> GetUserAsync(string accessToken);
    }
}
