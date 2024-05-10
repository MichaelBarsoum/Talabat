using AutoMapper;
using Talabat.API.DTOs;
using Talabat.API.DTOs.AccountsDTO;
using Talabat.API.DTOs.BasketDTO;
using Talabat.API.DTOs.OrderDTO;
using Talabat.API.Helpers;
using Talabat.Core.Models;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Identity;
using Talabat.Core.Models.Order;

namespace Talabat.API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                     .ForMember(D => D.Producttype, O => O.MapFrom(M => M.Producttype.Name))
                     .ForMember(D => D.barnd, O => O.MapFrom(M => M.barnd.Name))
                     .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureResolver>());
            CreateMap<UserAddress, AddressDTO>().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemsDTO, BasketItems>().ReverseMap(); 
            CreateMap<AddressDTO, OrderAddress>();
            CreateMap<Order, OrderToReturnDTO>().ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                                                .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDTO>().ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                                                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                                                .ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                                                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
