using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class OrderService
    {
        private readonly IRepository _repository;

        private static readonly List<Guid> FakeCustomers = new()
        {
            Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
            Guid.Parse("b2c3d4e5-f6a1-8901-2345-67890abcdef1"),
            Guid.Parse("c3d4e5f6-a1b2-9012-3456-7890abcdef12")
        };

        public OrderService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderModel.OrderResponse> CreateOrderAsync(OrderModel.OrderRequest request)
        {
            if (!FakeCustomers.Contains(request.CustomerId))
                throw new BadRequestException("Cliente inválido o no simulado.");

            if (request.OrderItems == null || !request.OrderItems.Any())
                throw new BadRequestException("La orden debe tener al menos un item.");

            decimal total = 0;
            var orderItems = new List<OrderItem>();

            var order = new Order
            {
                CustomerId = request.CustomerId,
                Date = DateTime.Now,
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                Note = request.Notes ?? ""
            };

            order.Status = OrderStatus.Pending;

            await _repository.Add(order);

            foreach (var item in request.OrderItems)
            {
                var product = await _repository.First<Product>(p => p.Id == item.ProductId);
                if (product == null)
                    throw new BadRequestException($"Producto {item.ProductId} no encontrado");

                if (product.StockQuantity < item.Quantity)
                    throw new BadRequestException($"Stock insuficiente para el producto {product.Name}");

                decimal subTotal = product.CurrentUnitPrice * item.Quantity;
                total += subTotal;

                orderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.CurrentUnitPrice,
                    SubTotal = subTotal
                });

                product.StockQuantity -= item.Quantity;
                await _repository.Update(product);
            }

            order.TotalAmount = total;
            order.OrderItems = orderItems;
            await _repository.Update(order);

            return new OrderModel.OrderResponse(
                    order.Id,
                    order.Date,
                    order.OrderItems.Select(i => new OrderItemModel.Response(
                          i.ProductId, i.Quantity, i.UnitPrice, i.SubTotal
                    )).ToList(),
                    order.ShippingAddress,
                    order.BillingAddress,
                    order.Note,
                    order.TotalAmount,
                    order.Status.ToString()
            );
        }

        public async Task<List<Order>?> GetAllOrders()
        {
            if (_repository is null)
            {
                throw new NoContentException("There aren't orders in the Data Base.");
            }

            var orders = await _repository.GetAll<Order>();

            return orders?.ToList();
        }

        public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
        {
            if(id == Guid.Empty || _repository is null)
            {
                throw new NotFoundException("There isn't a order with the provided ID.");
            }

            var order = await _repository.GetById<Order>(id);

            if (order is null)
            {
                throw new NotFoundException("There isn't a order in the Data Base.");
            }

            return new OrderModel.OrderResponse(
                    order.Id,
                    order.Date,
                    order.OrderItems.Select(item => new OrderItemModel.Response(
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice,
                        item.SubTotal
                    )).ToList(),
                    order.ShippingAddress,
                    order.BillingAddress,
                    order.Note!,
                    order.TotalAmount,
                    order.Status.ToString()
            );
        }

        public async Task<UpdateOrderStatusModel.UpdateOrderStatusResponse?> UpdateOrderStatus(Guid id, UpdateOrderStatusModel.UpdateOrderStatusRequest request)
        {
            if (id == Guid.Empty || _repository is null)
            {
                throw new NotFoundException("There isn't a order with the provided ID.");
            }

            if (string.IsNullOrWhiteSpace(request.Status.ToString()))
                throw new BadRequestException("The Status is obligatory");

            var order = await _repository.GetById<Order>(id);

            order.Status = request.Status;

            var orderUpdate = await _repository.Update(order);

            return new UpdateOrderStatusModel.UpdateOrderStatusResponse(
                orderUpdate.Id,
                orderUpdate.Status
                );
        }
    }
}

