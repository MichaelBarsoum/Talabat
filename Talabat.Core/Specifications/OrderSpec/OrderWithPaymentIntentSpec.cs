using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpec : Specification<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId):base(O => O.PaymentIntentId == paymentIntentId) 
        {
            

        }
    }
}
