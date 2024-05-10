using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Basket;

namespace Talabat.Core.Interfaces
{
    public interface IBasketRepo
    {
        Task<CustomerBasket?> GetBasketAsync(string id);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);
        Task<bool> DeleteBasketAsync(string BasketId);
    }
}
