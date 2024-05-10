using System.ComponentModel.DataAnnotations;
using Talabat.API.DTOs.AccountsDTO;
using Talabat.Core.Models.Order;

namespace Talabat.API.DTOs.OrderDTO
{
    public class OrderDTO
    { 

        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
