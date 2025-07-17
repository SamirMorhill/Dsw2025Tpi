using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record class ProductCreateModel
    {
        public record ProductRequest(string Sku, 
            string InternalCode,
            string Name, 
            string Description,
            decimal CurrentUnitPrice, 
            decimal StockQuantity);


        public record ProductResponse(Guid Id,
            string Sku, 
            string Name, 
            string Description, 
            decimal Price, 
            decimal StockQuantity);
    }
}
