using AuthAPI.Models.Domain;
using AuthAPI.Models.DTO;

namespace AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto?> Register(RegisterRequestDto dto);
        Task<LoginResponseDto> Login(LoginRequestDto dto);
    }
}
