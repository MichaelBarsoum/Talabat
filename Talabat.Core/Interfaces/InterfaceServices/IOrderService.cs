using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Interfaces.InterfaceServices
{
    public interface IOrderService
    {
        Task<Order?> CreateOrder(string BuyerEmail, string BasketId, int DeliveryMethodId, OrderAddress Shippingaddress);
        Task<IReadOnlyList<Order>> GetOrderForSpecificUser(string BuyerEmail);
        Task<Order> GetOrderByIdforSpecificUser(string BuyerEmail, int OrderId);
    }
}
