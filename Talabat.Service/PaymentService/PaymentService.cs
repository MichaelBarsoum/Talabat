using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Stripe;
using Stripe.Climate;
using Talabat.Core.Interfaces;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Order;
using Talabat.Core.Specifications.OrderSpec;
using Order = Talabat.Core.Models.Order.Order;

namespace Talabat.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepo basketRepo, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _basketRepo.GetBasketAsync(BasketId);
            if (basket is null) return null;
            var ShippingPrice = 0M;
            if (basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            if (basket.items.Count > 0)
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Core.Models.Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var SubTotal = basket.items.Sum(item => item.Quantity * item.Price);

            // Create Payment intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create payment intent
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }

                };
                paymentIntent = await Service.CreateAsync(Options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else // update
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                };
                paymentIntent = await Service.UpdateAsync(basket.PaymentIntentId, Options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentSucceedOrFailed(string PaymentIntentId, bool Flag)
        {
            var spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var orderstatus = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (Flag)
                orderstatus.Status = OrderStatus.PaymentReceived;
            else
                orderstatus.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().UpdateAsync(orderstatus);
            _unitOfWork.CompleteAsync();
            return orderstatus;
        }
    }
}
