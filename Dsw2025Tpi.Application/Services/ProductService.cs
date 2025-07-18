using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductService
    {
        private readonly IRepository _repository;
        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductModel.ProductResponse> CreateProduct(ProductModel.ProductRequest request)
        {
            var productExists = await _repository.First<Product>(p => p.Sku == request.Sku);

            if (string.IsNullOrWhiteSpace(request.Sku) || productExists is not null)
            {
                throw new ArgumentException("Ya existe un producto con el sku - El sku es invalido" +
                    "categoria #Humor.");
            } else if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("El nombre del producto no puede ser nulo o vacio.");
            }else if (request.CurrentUnitPrice <= 0)
            {
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            }else if ( request.StockQuantity < 0)
            {
                throw new ArgumentException("La cantidad de stock del producto no puede ser negativa.");
            }



            var product = new Product(
                        request.Sku,
                        request.InternalCode,
                        request.Name,
                        request.Description,
                        request.CurrentUnitPrice,
                        request.StockQuantity);

            await _repository.Add(product);
            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.Name,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity);
        }

        public async Task<List<Product>?> GetAllProducts()
        {
            if (_repository is null)
            {
                throw new ArgumentNullException("No hay productos en la base de datos.");
            }
            var products = await _repository.GetAll<Product>();

            return products?.ToList();
        }

        public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
        {
            if (id == Guid.Empty || _repository is null)
            {
                throw new ArgumentException("No hay un producto con el Id proporcionado.");
            }

            var product = await _repository.GetById<Product>(id);

            return new ProductModel.ProductResponse(product.Id,
                product.Sku,
                product.Name, 
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity);
        }




    }
}
