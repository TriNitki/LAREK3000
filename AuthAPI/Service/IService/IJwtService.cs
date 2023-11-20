using AuthAPI.Models.Domain;

namespace AuthAPI.Service.IService
{
    public interface IJwtService
    {
        string CreateJWTToken(AppUser user, List<string> roles);
    }
}
