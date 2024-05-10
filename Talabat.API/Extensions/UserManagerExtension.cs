using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models.Identity;
using Talabat.Repository.IdentityContext;

namespace Talabat.API.Extensions
{
    public static class UserManagerExtension
    {

        public static async Task<ApplicationUser> GetUserAddressAsync(this UserManager<ApplicationUser> userManager,ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.userAddress).FirstOrDefaultAsync(U => U.Email == Email);
            return user;
        }
    }
}
