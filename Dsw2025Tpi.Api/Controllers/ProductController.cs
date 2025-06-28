using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Application.Exceptions;
namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Route("/api/products")]
    public class ProductController : ControllerBase
    {
        public readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/api/products")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModel.ProductRequest request)
        {
            var product = await _productService.CreateProduct(request);
            return Created(); 
        }

        [HttpGet("/api/products")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            if (products == null || !products.Any())
            {
                return NoContent(); 
            }
            return Ok(products);
        }

        [HttpGet("/api/product/{id}")]
        public async Task<IActionResult> GetProductById(Guid Id)
        {
            var product = await _productService._repository.GetById<Product>(Id);
            if (product == null)
            {
                throw new NotFoundException($"Product with Id {Id} not found. "); 
            }
            return Ok(product);
        }

        [HttpPut("/api/products/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid Id, [FromBody]ProductCreateModel.ProductRequest request)
        {
            var product = await _productService.ProductUpdate(Id, request);
            if (product == null)
            {
                throw new NotFoundException($"Product with Id {Id} not found. ");
            }

            var updateProduct = await _productService.ProductUpdate(Id, request);
            return Ok(updateProduct);
        }

        [HttpPatch("/api/products/{id}")]
        public async Task<IActionResult> InactiveProduct(Guid Id)
        {
            try
            {
                await _productService.InactiveProduct(Id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    } 
}
