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
    public class ProductService
    {
        public readonly IRepository _repository;
        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCreateModel.ProductResponse> CreateProduct(ProductCreateModel.ProductRequest request)
        {
            var product = new Product(
                request.Sku, 
                request.InternalCode, 
                request.Name, 
                request.Description, 
                request.Price, 
                request.StockQuantity);
            await _repository.Add(product);
            return new ProductCreateModel.ProductResponse(
                product.Id, 
                product.Sku, 
                product.Name, 
                product.Description, 
                product.CurrentUnitPrice, 
                product.StockQuantity);
        }

        public async Task<List<Product>?> GetAll()
        {
            var product = await _repository.GetAll<Product>();
            return product?.ToList();
        }
    
        public async Task<ProductCreateModel.ProductResponse?> GetProductById(Guid Id)
        {
            var product = await _repository.GetById<Product>(Id);
            return new ProductCreateModel.ProductResponse(
                product.Id, 
                product.Sku, 
                product.Name, 
                product.Description, 
                product.CurrentUnitPrice, 
                product.StockQuantity);
        }

        public async Task<ProductCreateModel.ProductResponse> ProductUpdate(Guid Id, ProductCreateModel.ProductRequest request)
        {
            var product = await _repository.GetById<Product>(Id);
            if (product == null)
            {
                throw new NotFoundException($"Product with Id {Id} not found. "); 
            }

            product.Sku = request.Sku;
            product.Name = request.Name;
            product.Description = request.Description;
            product.CurrentUnitPrice = request.Price;
            product.StockQuantity = (int)request.StockQuantity;

            var updateProduct = await _repository.Update(product);

            return new ProductCreateModel.ProductResponse(
                updateProduct.Id,
                updateProduct.Sku,
                updateProduct.Name,
                updateProduct.Description,
                updateProduct.CurrentUnitPrice,
                updateProduct.StockQuantity);
        }

        public async Task InactiveProduct(Guid Id)
        {
            var product = await _repository.GetById<Product>(Id);
            if ( product == null)
            {
                throw new NotFoundException($"Product with Id {Id} not found. ");
            }

            product.IsActive = false;
            await _repository.Update(product);
        }
    }
}
