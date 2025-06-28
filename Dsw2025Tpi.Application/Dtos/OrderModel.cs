using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dsw2025Tpi.Domain.Entities;


namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        public record Request(
            Guid CustomerId,
            List<OrderItemModel.Request> Items,
            string ShippingAddress,
            string BillingAddress,
            string? Notes
        );

        public record Response(
            Guid OrderId,
            DateTime Date,
            List<OrderItemModel.Response> Items,
            decimal TotalAmount,
            string Status
        );
    }
}
