using OrderAPI.Models.DTO.AuthDTO;

namespace OrderAPI.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto<UserDto>?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto<UserDto>?> GetUserByIdAsync(Guid userId);
        Task<ResponseDto<UserDto>?> GetUserAsync(string accessToken);
    }
}
