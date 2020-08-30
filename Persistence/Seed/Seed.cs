using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            await SeedUser(userManager);
        }

        private static async Task SeedUser(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(new AppUser()
                {
                    DisplayName = "Test",
                    UserName = "test",
                    Email = "test@test.ts"
                }, "Pa$$w0rd");
            }
        }
    }
}
