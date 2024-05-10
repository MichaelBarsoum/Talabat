using AutoMapper;
using AutoMapper.Execution;
using Talabat.API.DTOs;
using Talabat.Core.Models;

namespace Talabat.API.Helpers
{
    public class ProductPictureResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            return string.Empty ;
        }
    }
}
