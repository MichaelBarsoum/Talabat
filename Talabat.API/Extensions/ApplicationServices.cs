using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.API.Profiles;
using Talabat.Core.Interfaces;
using Talabat.Repository.Contexts;
using Talabat.Repository.IdentityContext;
using Talabat.Repository.Repositories.BasketRepository;
using Talabat.Repository.Repositories;
using Talabat.API.Errors;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Service.OrderService;
using Talabat.Service.PaymentService;

namespace Talabat.API.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection ApplicationService(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddScoped(typeof(IBasketRepo), typeof(BasketRepository));
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            return Services;
        }
    }
}
