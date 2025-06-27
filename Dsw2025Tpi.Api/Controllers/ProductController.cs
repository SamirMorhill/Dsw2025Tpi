using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Route("api/^products")]
    public class ProductController : ControllerBase
    {
        public readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/api/products")]

        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModel.Request request)
        {
            var product = await _productService.CreateProduct(request);
            return Ok(product);
        }

        [HttpGet("/api/products")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        [HttpGet("/api/products/{id}")]

        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);

        }

        [HttpPut("/api/products/{id} ")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateModel.Request request)
        {
            var product = await _productService.ProductUpdate(id, request);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            
            var updatedProduct = await _productService.ProductUpdate(id, request);
            return Ok(updatedProduct);


        }


    }

}