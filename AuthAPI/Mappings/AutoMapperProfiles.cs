using AuthAPI.Models.Domain;
using AuthAPI.Models.DTO;
using AutoMapper;

namespace AuthAPI.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
