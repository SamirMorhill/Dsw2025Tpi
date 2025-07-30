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
    }
}
