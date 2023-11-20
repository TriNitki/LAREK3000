using CatalogAPI.Models.DTO.AuthDTO;
using CatalogAPI.Service.IService;
using CatalogAPI.Utility;

namespace CatalogAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDto<UserDto>?> GetUserAsync(string accessToken)
        {
            return await baseService.SendAsync<UserDto>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/GetUser",
                AccessToken = accessToken
            });
        }

        public async Task<ResponseDto<UserDto>?> GetUserByIdAsync(Guid userId)
        {
            return await baseService.SendAsync<UserDto>(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + $"/api/GetUser/{userId}",
            });
        }

        public async Task<ResponseDto<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await baseService.SendAsync<LoginResponseDto>(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/Login",
            });
        }

        public async Task<ResponseDto<UserDto>?> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            return await baseService.SendAsync<UserDto>(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthAPIBase + "/api/Register",
            });
        }
    }
}
