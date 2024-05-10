using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Talabat.Core.Interfaces;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepo basketRepo,
                            IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order?> CreateOrder(string BuyerEmail, string BasketId, int DeliveryMethodId, OrderAddress Shippingaddress)
        {
            var basket = await _basketRepo.GetBasketAsync(BasketId); // _basketRepo => Treat With Redis 
            var orderItems = new List<OrderItem>();
            if (basket?.items.Count > 0)
            {
                foreach (var item in basket.items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(ProductItemOrdered, item.Quantity, product.Price);
                    orderItems.Add(orderItem);
                }
            }
            var subTotal = orderItems.Sum(I => I.Price * I.Quantity);
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var ExistOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(ExistOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExistOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }
            var order = new Order(BuyerEmail, Shippingaddress, DeliveryMethod, orderItems, subTotal,basket.PaymentIntentId);
            
            await _unitOfWork.Repository<Order>().AddAsync(order); // Add Locally 
           var Result = await _unitOfWork.CompleteAsync(); // Add Remotely in db
            if (Result <= 0) return null;            
            return order;
        }

        public async Task<Order> GetOrderByIdforSpecificUser(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(BuyerEmail,OrderId);
            var Orders = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return Orders;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUser(string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail);
            var order =await _unitOfWork.Repository<Order>().GetAllAsync(spec);
            return order;
        }
    }
}
