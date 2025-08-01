using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/api/orders")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request);
                return Created($"/api/orders/{order.OrderId}",order);
            }
            catch (BadRequestException ex)
            {
                return BadRequest($"Error al crear la orden: {ex.Message}");
            }
        }

        [HttpGet("/api/orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] string? status,
                 [FromQuery] Guid? customer,
                 [FromQuery] int pageNumber = 1,
                 [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetAllOrders(status, customer, pageNumber, pageSize);

                if (orders is null)
                {
                    return NotFound("No hay ordenes registradas.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las ordenes: {ex.Message}");
            }
        }

        [HttpGet("/api/orders/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);

                if (order is null)
                {
                    return NotFound("Order not found.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound($"Mistake to find order: {ex.Message}");
            }
        }

        [HttpPut("/api/orders/{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusModel.UpdateOrderStatusRequest request)
        {
            try
            {
                var updateOrder = await _orderService.UpdateOrderStatus(id, request);
                return Ok(updateOrder);
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error al actualizar la orden: {ex.Message}");
            }
        }
    }
}
