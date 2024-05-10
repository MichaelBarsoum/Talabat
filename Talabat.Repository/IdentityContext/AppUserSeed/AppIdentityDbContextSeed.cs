using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.IdentityContext.AppUserSeed
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Michael Nabil",
                    Email = "Acc.MichaelNabil729@Gmail.com",
                    UserName = "Acc.MichaelNabil729",
                    PhoneNumber = "012234543535"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
