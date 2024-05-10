using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification(string email):base(O => O.BuyerEmail == email)
        {
            includes.Add(O => O.DeliveryMethod);
            includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSpecification(string email , int OrderId):base(O => O.BuyerEmail == email && O.Id == OrderId)
        {
            includes.Add(O => O.DeliveryMethod);
            includes.Add(O => O.Items);
        }
    }
}
