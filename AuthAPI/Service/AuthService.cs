using AuthAPI.Models.Domain;
using AuthAPI.Models.DTO;
using AuthAPI.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IJwtService jwtService;
        private readonly IMapper mapper;

        public AuthService(UserManager<AppUser> userManager, IJwtService jwtService, IMapper mapper)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
            this.mapper = mapper;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Username);

            if (user == null)
            {
                return null;
            }


            var isLoggedIn = await userManager.CheckPasswordAsync(user, dto.Password);

            if (!isLoggedIn)
            {
                return null;
            }

            var roles = await userManager.GetRolesAsync(user);

            if (roles == null)
            {
                return null;
            }

            var jwtToken = jwtService.CreateJWTToken(user, roles.ToList());
            var response = new LoginResponseDto
            {
                JwtToken = jwtToken
            };

            return response;
        }

        public async Task<UserDto?> Register(RegisterRequestDto dto)
        {
            var identityUser = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Username,
                Address = dto.Address
            };

            var identityResult = await userManager.CreateAsync(identityUser, dto.Password);

            if (!identityResult.Succeeded)
            {
                return null;
            }

            identityResult = await userManager.AddToRolesAsync(identityUser, dto.Roles);

            if (!identityResult.Succeeded)
            {
                return null;
            }

            var createdUser = await userManager.FindByEmailAsync(dto.Username);

            if (createdUser == null)
            {
                return null;
            }

            var userDto = mapper.Map<UserDto>(createdUser);

            return userDto;
        }
    }
}
