using System.ComponentModel.DataAnnotations;
using Talabat.Core.Models.Basket;

namespace Talabat.API.DTOs.BasketDTO
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemsDTO> items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
