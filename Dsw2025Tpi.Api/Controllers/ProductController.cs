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


        [HttpPost("api/products")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel.ProductRequest request)
        {

            try
            {
                var product = await _productService.CreateProduct(request);

                return Created($"/api/products/{product.Id}", product);

            } catch (Exception ex)
            {
                return BadRequest($"Error al crear el producto: {ex.Message}");
            }



            
        }

        [HttpGet("/api/products")]
        public async Task<IActionResult> GetAllProducts()
        {

            try
            {
                var products = await _productService.GetAllProducts();

                if (products is null || !products.Any())
                {
                    return NotFound("No hay productos disponibles.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los productos: {ex.Message}");
            }
        }


        [HttpGet("/api/products/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {

            try
            {
                var product = await _productService.GetProductById(id);
                if (product is null)
                {
                    return NotFound("Producto no encontrado.");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound($"Error al obtener el producto: {ex.Message}");

            }
        }


        [HttpPut("/api/products/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.ProductRequest request)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(id, request);

                return Ok(updatedProduct);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el producto: {ex.Message}");

            }


        }

        [HttpPatch("/api/products/{id} ")]
        public async Task<IActionResult> DisabledProduct(Guid id, [FromBody] ProductDisabledModel.ProductDisabledRequest request)
        {

            try
            {
                var productDisabled = await _productService.DisabledProduct(id, request);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest($"Error al deshabilitar el producto: {ex.Message}");
            }


        }

    }
}
