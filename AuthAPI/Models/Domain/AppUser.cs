using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models.Domain
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public virtual string? Address { get; set; }
    }
}
