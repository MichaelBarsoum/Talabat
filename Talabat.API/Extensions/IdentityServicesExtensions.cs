using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Core.Models.Identity;
using Talabat.Repository.IdentityContext;
using Talabat.Repository.Repositories.UnitOfWork;
using Talabat.Service.TokenService;

namespace Talabat.API.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services , IConfiguration configuration)
        {
            
            Services.AddIdentity<ApplicationUser, IdentityRole>() // Made Implement For interfaces That Identity User Implemented it 
                            .AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            };
            }); // AddAuthentication => add service for userManager & SignInManager & RollManager
            Services.AddScoped<ITokenService , TokenService>();
            Services.AddScoped<IUnitOfWork , UnitOfWork>();
            return Services;
        }
    }
}
