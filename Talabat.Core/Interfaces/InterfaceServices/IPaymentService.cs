using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Interfaces.InterfaceServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order> UpdatePaymentIntentSucceedOrFailed(string PaymentIntentId, bool Flag);
    }
}
