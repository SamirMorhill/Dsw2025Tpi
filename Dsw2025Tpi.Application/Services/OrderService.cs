using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using System.Net.WebSockets;
using Dsw2025Tpi.Application.Exceptions;
using System.Diagnostics;


namespace Dsw2025Tpi.Application.Services
{
    public class OrderService
    {

        public readonly IRepository _repository;


        public OrderService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderModel.OrderResponse> CreateOrderAsync(OrderModel.OrderRequest request)
        {
            var customer = await _repository.GetById<Customer>(request.CustomerId);
            if (customer == null)
                throw new BadRequestException("Invalid customer ID");

            var productIds = request.Items.Select(i => i.ProductId).ToList();

            var products = (await _repository.GetFiltered<Product>(p => productIds.Contains(p.Id)))
                ?.ToList() ?? new List<Product>();

            if (products.Count != productIds.Count)
                throw new BadRequestException("One or more products not found");


            var insufficientStock = request.Items
                .Where(item =>
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return !product.HasSufficientStock(item.Quantity);
                })
                .ToList();

            if (insufficientStock.Any())
            {
                var names = string.Join(", ", insufficientStock
                    .Select(i => products.First(p => p.Id == i.ProductId).Name));
                throw new BadRequestException($"Insufficient stock for: {names}");
            }



            foreach (var item in request.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                product.DecreaseStock(item.Quantity);
                await _repository.Update(product);
            }


            var orderItems = request.Items
                .Select(item =>
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return new OrderItem(item.Quantity, product.CurrentUnitPrice, product);
                }) 
                .ToList();

            var order = new Order
            (
                 DateTime.Now,
                 request.ShippingAddress,
                 request.BillingAddress,
                 request.Notes,
                 OrderStatus.Pending
            );

            await _repository.Add(order);

            return new OrderModel.OrderResponse(
                order.Id,
                order.Date,
                orderItems.Select(oi =>
                    new OrderItemModel.Response(
                        oi.ProductId,
                        oi.Quantity,
                        oi.UnitPrice,
                        oi.SubTotal)).ToList(),
                order.TotalAmount,
                order.Status.ToString()
            );
        }
    }
}
