using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.CustomMiddleWares;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Profiles;
using Talabat.Core.Interfaces;
using Talabat.Core.Models.Identity;
using Talabat.Repository.Contexts;
using Talabat.Repository.Helpers;
using Talabat.Repository.IdentityContext;
using Talabat.Repository.IdentityContext.AppUserSeed;
using Talabat.Repository.Repositories;
using Talabat.Repository.Repositories.BasketRepository;
namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TalabatContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("AppUserConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(E => E.Value.Errors.Count() > 0)
                                               .SelectMany(E => E.Value.Errors)
                                               .Select(E => E.ErrorMessage).ToList();
                    var ErrorValidationResponse = new ApiValidationResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ErrorValidationResponse);
                };
            });
            builder.Services.ApplicationService();
            builder.Services.AddIdentityServices(builder.Configuration);


            #endregion

            var app = builder.Build();

            #region Update Database Automatically
            var scope = app.Services.CreateScope(); // hold all services and put it in container
            var service = scope.ServiceProvider; // Manage life Time of object
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbcontext = service.GetRequiredService<TalabatContext>();
                await dbcontext.Database.MigrateAsync();
                var IdentityDbContext = service.GetRequiredService<AppIdentityDbContext>();
                var UserManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                await IdentityDbContext.Database.MigrateAsync();
                
                await DataSeeding.SeedAsync(dbcontext);
                
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There is an error Occure during apply Database");
            }
            #endregion            

            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
            #endregion
        }
    }
}
