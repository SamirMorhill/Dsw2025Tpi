using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;

        }
        

            [HttpPost("api/product/")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModel.ProductRequest request)
        {
            
            var product = await _productService.CreateProduct(request);

            if (product is null)
            {
                return BadRequest("Error al crear el producto.");
            }

            return Created($"/api/products/{product.Id}", product);
        }



    }
}
