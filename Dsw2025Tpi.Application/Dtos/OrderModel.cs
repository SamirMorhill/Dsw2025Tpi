using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos
{
    internal class OrderModel
    {
        public record Request (Guid CustomerId, List<OrderItemModel> Items, string ShippingAddress, string BillingAddress);

        public record Respond(DateTime Date, decimal TotalAmount, OrderStatus Status);
    }
}
