using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Interfaces;
using Talabat.Core.Models.Basket;

namespace Talabat.Repository.Repositories.BasketRepository
{
    public class BasketRepository : IBasketRepo
    {
        private readonly IDatabase _Database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _Database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _Database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _Database.StringGetAsync(BasketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket); 
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);
            var CreatedOrUpdated = await _Database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(1));
            if (!CreatedOrUpdated) return null;
            return await GetBasketAsync(Basket.Id);
        }
    }
}
