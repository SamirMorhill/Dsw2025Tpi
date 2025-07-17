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

        public async Task<ProductCreateModel.ProductResponse> CreateProduct(ProductCreateModel.ProductRequest request)
        {
            var product = new Product(
                request.Sku,
                request.InternalCode,
                request.Name,
                request.Description,
                request.CurrentUnitPrice,
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



    }
}
