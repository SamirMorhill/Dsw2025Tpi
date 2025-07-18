using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
                throw new BadRequestException("Invalid SKU.");
            } else if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadRequestException("The product name can´t be null or empty.");
            }else if (request.CurrentUnitPrice <= 0)
            {
                throw new BadRequestException("The product price must be greater than zero.");
            }else if ( request.StockQuantity < 0)
            {
                throw new BadRequestException("The stock product can't be negative.");
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
                throw new NoContentException("There aren´t products in the Data Base.");
            }
            var products = await _repository.GetAll<Product>();

            return products?.ToList();
        }

        public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
        {
            if (id == Guid.Empty || _repository is null)
            {
                throw new NotFoundException("There is a product with the povided ID.");
            }

            var product = await _repository.GetById<Product>(id);

            return new ProductModel.ProductResponse(product.Id,
                product.Sku,
                product.Name, 
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity);
        }

        public async Task<ProductModel.ProductResponse?> UpdateProduct(Guid id, ProductModel.ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new BadRequestException("The SKU is obligatory.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException("The NAME is obligatory.");

            if (request.CurrentUnitPrice <= 0)
                throw new BadRequestException("The product price must be greater than zero.");

            if (request.StockQuantity < 0)
                throw new BadRequestException("The STOCK product can't be negative.");

            var product = await _repository.GetById<Product>(id);

            product.Sku = request.Sku;
            product.InternalCode = request.InternalCode;
            product.Name = request.Name;
            product.Description = request.Description;
            product.CurrentUnitPrice = request.CurrentUnitPrice;
            product.StockQuantity = (int)request.StockQuantity;

            var productUpdate = await _repository.Update(product);

            return new ProductModel.ProductResponse(
                productUpdate.Id,
                productUpdate.Sku,
                productUpdate.Name,
                productUpdate.Description,
                productUpdate.CurrentUnitPrice,
                productUpdate.StockQuantity);
        }

        

        

    }
}
