using AuthAPI.Models.Domain;
using AuthAPI.Models.DTO;
using AuthAPI.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AuthController(UserManager<AppUser> userManager, IAuthService authService, IMapper mapper)
        {
            this.userManager = userManager;
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await authService.Register(request);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await authService.Login(request);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByIdAsync(userId);

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpGet("GetUser/{id:Guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
    }
}
