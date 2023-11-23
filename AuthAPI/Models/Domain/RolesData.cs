using AuthAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models.Domain
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] { "Admin", "Seller", "Buyer", "Courier" };

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AuthDbContext>();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                
            }
        }
    }
}
