using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record ProductModel
    {
        public record Request (string sku,string name,string description,decimal price,decimal stockQuantity);

        public record Response (Guid id);


    }
}
