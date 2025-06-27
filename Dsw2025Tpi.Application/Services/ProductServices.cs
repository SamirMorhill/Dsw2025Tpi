using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductService
    {
        public readonly IRepository _repository;
        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCreateModel.Response> CreateProduct(ProductCreateModel.Request request)
        {
            var product = new Product(request.sku, request.internalCode, request.name, request.description, request.price, request.stockQuantity);
            await _repository.Add(product);
            return new ProductCreateModel.Response(product.Id, product.Sku, product.Name, product.Description, product.CurrentUnitPrice, product.StockQuantity);
        }

        public async Task<List<Product>?> GetAll()
        {
            var product = await _repository.GetAll<Product>();
            return product?.ToList();
        }

        public async Task<ProductModel.Response?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            return new ProductModel.Response(product.Id);


        }

        public async Task<ProductUpdateModel.Response> ProductUpdate(Guid id, ProductUpdateModel.Request request)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            product.Sku = request.sku;
            product.Name = request.name;
            product.Description = request.description;
            product.CurrentUnitPrice = request.price;
            product.StockQuantity = (int)request.stockQuantity;

            var updateProduct = await _repository.Update(product);
    
            return new ProductUpdateModel.Response(updateProduct.Id, 
                updateProduct.Sku, 
                updateProduct.InternalCode, 
                updateProduct.Name, 
                updateProduct.Description, 
                updateProduct.CurrentUnitPrice, 
                updateProduct.StockQuantity);


        }


    }
}